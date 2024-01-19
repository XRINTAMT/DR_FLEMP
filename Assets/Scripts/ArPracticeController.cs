using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArPracticeController : MonoBehaviour
{
    [Serializable]
    public class DesriptionList
    {
        public string[] description;
        public string inputName;
        public string correctName;
        public int chooseAnswer;
        public int correctAnswer;
    }
    [SerializeField] Text textName;
    [SerializeField] Text textDescription1;
    [SerializeField] Text textDescription2;
    [SerializeField] Text textDescription3;
    public GameObject[] poolObjects;
    public List<DesriptionList> descriptionsList;
    int currentObjIndex;
    public ArItemTeaching arItemTeaching;
    // Start is called before the first frame update
    void Start()
    {
        Button button1 = textDescription1.GetComponentInParent<Button>();
        Button button2 = textDescription2.GetComponentInParent<Button>();
        Button button3 = textDescription3.GetComponentInParent<Button>();

        //button1.onClick.AddListener(() => SetChooseAnswer(1));
        //button2.onClick.AddListener(() => SetChooseAnswer(2));
        //button3.onClick.AddListener(() => SetChooseAnswer(3));

        //arItemTeaching = FindObjectOfType<ArItemTeaching>(true);
        
        //SetStartObj();
    }
    public void DisableAllObjects()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].SetActive(false);
        }

    }
    void SetInputName(string name) 
    {
        descriptionsList[currentObjIndex].inputName = name;
    }
    void SetChooseAnswer(int index)
    {
        descriptionsList[currentObjIndex].chooseAnswer = index;
    }
    public void CheckNext()
    {
        //if (descriptionsList[currentObjIndex].correctName == descriptionsList[currentObjIndex].inputName && descriptionsList[currentObjIndex].correctAnswer == descriptionsList[currentObjIndex].chooseAnswer)
        //{
        //    descriptionsList.RemoveAt(currentObjIndex);
        //    currentObjIndex++;
        //    if (currentObjIndex >= poolObjects.Length - 1)
        //    {
        //        currentObjIndex = 0;
        //    }
        //}
        SetNext();

    }
    public void SetStartObj()
    {
        DisableAllObjects();
        currentObjIndex = 0;
        //arItemTeaching.DisableAllObjects();
        poolObjects[0].SetActive(true);
        textName.text = descriptionsList[0].correctName;
        textDescription1.text = descriptionsList[0].description[0];
        textDescription2.text = descriptionsList[0].description[1];
        textDescription3.text = descriptionsList[0].description[2];

        currentObjIndex++;

    }
    void SetNext() 
    {

        if (currentObjIndex == 0)
        {
            poolObjects[poolObjects.Length - 1].SetActive(false);
            poolObjects[currentObjIndex].SetActive(true);
        }
        if (currentObjIndex >= 1)
        {
            poolObjects[currentObjIndex - 1].SetActive(false);
            poolObjects[currentObjIndex].SetActive(true);
        }
        //poolObjects[currentObjIndex].SetActive(true);
        textName.text = descriptionsList[currentObjIndex].correctName;
        textDescription1.text = descriptionsList[currentObjIndex].description[0];
        textDescription2.text = descriptionsList[currentObjIndex].description[1];
        textDescription3.text = descriptionsList[currentObjIndex].description[2];

        if (currentObjIndex == poolObjects.Length)
        {
            currentObjIndex = 0;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
