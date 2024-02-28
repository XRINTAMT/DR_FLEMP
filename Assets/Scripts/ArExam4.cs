using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
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
    InstScript instScript;
    CollisionIgnores collisionIgnores;
    // Start is called before the first frame update
    void Start()
    {

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
            Debug.Log("correct");
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
