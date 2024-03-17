using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using Autohand;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
    [SerializeField] ArExam arExam;
    [SerializeField] GameObject scoreUi;
    [SerializeField] Text scoreExam3;
    [SerializeField] Text scoreExam4;
    [SerializeField] Text localScoreCorrect;
    [SerializeField] Text localScoreIncorrect;


    [SerializeField] List<GameObject> spawnItems = new List<GameObject>();
    [SerializeField] List<Vector3> platesStartPosition = new List<Vector3>();
    [SerializeField] List<Vector3> platesStartRotation = new List<Vector3>();

    public float correct;
    public float incorrect;
    public UnityEvent complete;    
    InstScript instScript;
    CollisionIgnores collisionIgnores;
    public int language;
    public bool replay;
    // Start is called before the first frame update
    void Start()
    {
        instScript = FindObjectOfType<InstScript>();
        collisionIgnores = GetComponent<CollisionIgnores>();
        language= PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);

  
        List<Collider> coll1 = new List<Collider>();

        for (int i = 0; i < arObjectsPool.items.Length; i++)
        {
            GameObject obj = Instantiate(arObjectsPool.items[i].item, spawnItemPoints[i].position, Quaternion.identity);
            obj.transform.parent = spawnItemPoints[i];
            obj.transform.localPosition = Vector3.zero;
            spawnItems[i]=obj;
            //coll1.Add(obj.GetComponent<Collider>());
            foreach (var coll in obj.GetComponentsInChildren<Collider>())
            {
                coll1.Add(coll);
            }

            if (language == 0)
            {
                textPlates[i].text = arObjectsPool.items[i].functionEnglish;
                if (textPlates[i].text =="")
                {
                    textPlates[i].text = "NoneEnglishText";
                }
               
            }
            else
            {
                textPlates[i].text = arObjectsPool.items[i].functionGerman;
                if (textPlates[i].text == "")
                {
                    textPlates[i].text = "NoneGermanText";
                }
            }
        }


        collisionIgnores.cols1 = coll1.ToArray();
        collisionIgnores.cols2 = platesColliders;


        for (int i = 0; i < coll1.Count; i++)
            for (int j = 0; j < platesColliders.Length; j++)
                Physics.IgnoreCollision(coll1[i], platesColliders[j], true);

        for (int i = 0; i < placePoints.Length; i++)
        {
            placePoints[i].OnPlace.AddListener(OnPlace);
            //placePoints[i].OnRemove.AddListener(OnRemove);
        }
        if (platesPivot.transform.position != instScript.arTable.transform.position + new Vector3(0, 0.5f, 0.2f))
        {
            platesPivot.transform.position = instScript.arTable.transform.position + new Vector3(0, 0.5f, 0.2f);
        }
        if (itemsPivot.transform.position != instScript.arTable.transform.position + new Vector3(0, 0.05f, 0f))
        {
            itemsPivot.transform.position = instScript.arTable.transform.position + new Vector3(0, 0.05f, 0.2f);
        }
        if (platesPivot.transform.rotation != instScript.arTable.transform.rotation)
        {
            platesPivot.transform.rotation = instScript.arTable.transform.rotation;
        }
        if (itemsPivot.transform.rotation != instScript.arTable.transform.rotation)
        {
            itemsPivot.transform.rotation = instScript.arTable.transform.rotation;
        }

        for (int i = 0; i < placePoints.Length; i++)
        {
            platesStartPosition[i] = placePoints[i].transform.parent.localPosition;
            platesStartRotation[i] = placePoints[i].transform.parent.localEulerAngles;
        }
    }

    public void Replay() 
    {

        for (int i = 0; i < placePoints.Length; i++)
        {
            placePoints[i].enabled = false;
            placePoints[i].Remove();
        }
        for (int i = 0; i < spawnItems.Count; i++)
        {
            spawnItems[i].GetComponent<Rigidbody>().isKinematic = true;
            spawnItems[i].transform.parent = spawnItemPoints[i];
            spawnItems[i].transform.localPosition = Vector3.zero;
            spawnItems[i].transform.localEulerAngles = Vector3.zero;
            spawnItems[i].GetComponent<Rigidbody>().isKinematic = true;
        }
        for (int i = 0; i < placePoints.Length; i++)
        {
            placePoints[i].transform.parent.gameObject.SetActive(false);
            placePoints[i].transform.parent.localPosition = platesStartPosition[i];
            placePoints[i].transform.parent.localEulerAngles = platesStartRotation[i];
            placePoints[i].transform.parent.gameObject.SetActive(true);
            placePoints[i].enabled = true;
        }

        correct = 0;
        incorrect = 0;

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
            float ts = (correct / (correct + incorrect)) * 100;
            //scoreUi.transform.parent = platesPivot.transform;
            //scoreUi.transform.localPosition = new Vector3(0, 0, 0);
            scoreExam3.text = ""+arExam.totalScore;
            scoreExam4.text = "" + Mathf.RoundToInt(ts) + "%";


            localScoreCorrect.text = Mathf.RoundToInt(correct).ToString();
            localScoreIncorrect.text = Mathf.RoundToInt(incorrect).ToString();

            //scoreUi.SetActive(true);
            Debug.Log("Complete " + ts);
        }

    }

    public void OnRemove(PlacePoint placePoint, Grabbable grabbable)
    {
        //placePoint.GetComponentInParent<Grabbable>().enabled = true;
    }

    public void ReturnToMenu() 
    {
        SceneManager.LoadScene(0);
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
        if (platesPivot.transform.rotation != instScript.arTable.transform.rotation)
        {
            platesPivot.transform.rotation = instScript.arTable.transform.rotation;
        }
        if (itemsPivot.transform.rotation != instScript.arTable.transform.rotation)
        {
            itemsPivot.transform.rotation = instScript.arTable.transform.rotation;
        }

        if (replay)
        {
            Replay();
            replay = false;
        }
    }

   
}
