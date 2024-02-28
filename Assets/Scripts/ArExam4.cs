using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArExam4 : MonoBehaviour
{
    [SerializeField] ArObjectsPool arObjectsPool;
    [SerializeField] PlacePoint[] placePoints;
    [SerializeField] Collider [] platesColliders;
    [SerializeField] Transform [] spawnItemPoints;
    [SerializeField] Text[] textPlates;
    [SerializeField] Transform itemsPivot;
    [SerializeField] Transform platesPivot;
    [SerializeField] AudioSource audioSource;
    public int correct;
    public int incorrect;
    public UnityEvent complete;    
    InstScript instScript;
    CollisionIgnores collisionIgnores;
    string language;
    // Start is called before the first frame update
    void Start()
    {
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");

        instScript = FindObjectOfType<InstScript>();
        collisionIgnores = GetComponent<CollisionIgnores>();
        List <Collider> coll1 = new List<Collider>();
      
        for (int i = 0; i < arObjectsPool.items.Length; i++)
        {
            GameObject obj = Instantiate(arObjectsPool.items[i].item, spawnItemPoints[i].position, Quaternion.identity);
            obj.transform.parent = spawnItemPoints[i];
            obj.transform.localPosition = Vector3.zero;
            coll1.Add(obj.GetComponent<Collider>());

            textPlates[i].text = arObjectsPool.items[i].function;
        }
      
        collisionIgnores.cols1 = coll1.ToArray();
        collisionIgnores.cols2 = platesColliders;

        for (int i = 0; i < placePoints.Length; i++)
        {
            placePoints[i].OnPlace.AddListener(OnPlace);
            placePoints[i].OnRemove.AddListener(OnRemove);
        }

 
    }

    public void Activate(bool state) 
    {
        itemsPivot.gameObject.SetActive(state);
        platesPivot.gameObject.SetActive(state);
    }
   
    public void OnPlace(PlacePoint placePoint, Grabbable grabbable) 
    {
        //placePoint.GetComponentInParent<Grabbable>().enabled = false;
        Debug.Log(grabbable.name);
        Debug.Log(placePoint.name);
        if (placePoint.name==grabbable.name)
        {
            placePoint.transform.parent.gameObject.SetActive(false);
            Debug.Log("correct");
            audioSource.Play();
            correct++;
            
        }
        if (placePoint.name != grabbable.name)
        {
            Debug.Log("incorrect");
            incorrect++;
        }
        if (correct==arObjectsPool.items.Length)
        {
            complete?.Invoke();
            int totalScore = ((correct / (correct + incorrect) * 100));
            Debug.Log("Complete " + totalScore);
        }

    }


    public void OnRemove(PlacePoint placePoint, Grabbable grabbable)
    {
        //placePoint.GetComponentInParent<Grabbable>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (platesPivot.transform.position!=instScript.arTable.transform.position + new Vector3(0, 0.5f, 0.2f))
        {
            platesPivot.transform.position = instScript.arTable.transform.position + new Vector3(0, 0.5f, 0.2f);
        }
        if (itemsPivot.transform.position != instScript.arTable.transform.position + new Vector3(0, 0.05f, 0f))
        {
            itemsPivot.transform.position = instScript.arTable.transform.position + new Vector3(0, 0.05f, 0f);
        }
    }
}
