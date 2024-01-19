using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArItemTeaching : MonoBehaviour
{
    [SerializeField] Text textName;
    [SerializeField] Text textDescription;
    public GameObject[] poolObjects;
    public string[] names;
    public string[] descriptions;
    public int countObject;
    public bool next;
    public ArPracticeController arPracticeController;
    // Start is called before the first frame update
    void Start()
    {
        //SetStartObject();
        //arPracticeController = FindObjectOfType<ArPracticeController>(true);
    }
    public void SetStartObject()
    {

        DisableAllObjects();
        countObject = 0;
        //arPracticeController.DisableAllObjects();
        poolObjects[0].SetActive(true);
        textName.text = names[0];
        textDescription.text = descriptions[0];

        countObject++;
    }

    public void DisableAllObjects() 
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].SetActive(false);
        }

    }
    public void SetObject() 
    {
        if (countObject == 0) 
        {
            poolObjects[poolObjects.Length - 1].SetActive(false);
            poolObjects[countObject].SetActive(true);
        }
        if (countObject >= 1)
        {
            poolObjects[countObject - 1].SetActive(false);
            poolObjects[countObject].SetActive(true);
        }

        textName.text = names[countObject];
        textDescription.text = descriptions[countObject];

        countObject++;

        if (countObject == poolObjects.Length)
        {
            countObject = 0;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (next)
        {
            SetObject();
            next = false;

        }
    }
}
