using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class SolutionsList
{
    public List<string> solutions = new List<string>();
}

[Serializable]
public class AdditionalWordsList
{
    public List<string> additionalWords = new List<string>();
}
public class SentenceScrambleTab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDescription;
    [SerializeField] TextMeshProUGUI textConstructedSentence;
    [SerializeField] GameObject buttonSentencePrefab;
    [SerializeField] Transform contentConstructor;
    [SerializeField] Transform content;

    public List<string> descriptions = new List<string>();
    public List<SolutionsList> solutionsList = new List<SolutionsList>();
    public List<AdditionalWordsList> additionalWordList = new List<AdditionalWordsList>();
    //[HideInInspector]
    public List<bool> completion;
    public string constructedSentenceTrue;
    public string constructedSentenceCheck;
    List<string> allSentence = new List<string>();
    int indexList;
    [HideInInspector]
    public Button buttonChoose;

    // Start is called before the first frame update
    void Start()
    {
        SetNewList();
    }

    public void SetNewList() 
    {

        allSentence.Clear();

        textDescription.text = descriptions[indexList];
        //textConstructedSentence.text = "";
        constructedSentenceTrue = "";
        constructedSentenceCheck = "";

        foreach (var buttons in contentConstructor.GetComponentsInChildren<Button>())
            Destroy(buttons.gameObject);
        foreach (var buttons in content.GetComponentsInChildren<Button>())
            Destroy(buttons.gameObject);


        for (int i = 0; i < solutionsList[indexList].solutions.Count; i++)
        {
            allSentence.Add(solutionsList[indexList].solutions[i]);
            constructedSentenceTrue = constructedSentenceTrue + " " + solutionsList[indexList].solutions[i];
        }
        for (int i = 0; i < additionalWordList[indexList].additionalWords.Count; i++)
        {
            allSentence.Add(additionalWordList[indexList].additionalWords[i]);
        }

        Shuffle(allSentence);

       


        for (int i = 0; i < allSentence.Count; i++)
        {
            var button = Instantiate(buttonSentencePrefab, content);
            button.GetComponentInChildren<TextMeshProUGUI>().text = allSentence[i];
        }

    }
    public void SetVariant(string variant) 
    {
        var button = Instantiate(buttonSentencePrefab, contentConstructor);
        button.GetComponentInChildren<TextMeshProUGUI>().text = variant;
        button.GetComponent<ButtonSentence>().inConstructor = true;
        //textConstructedSentence.text = textConstructedSentence.text + " " + variant;

        constructedSentenceCheck = "";
        foreach (TextMeshProUGUI textMeshProUGUI in contentConstructor.GetComponentsInChildren<TextMeshProUGUI>())
        {
            constructedSentenceCheck = constructedSentenceCheck + " " + textMeshProUGUI.text;
        }
    }
    public void ReturnVariant(string variant)
    {
        var button = Instantiate(buttonSentencePrefab, content);
        button.GetComponentInChildren<TextMeshProUGUI>().text = variant;
        button.GetComponent<ButtonSentence>().inConstructor = false;
        //textConstructedSentence.text = textConstructedSentence.text + " " + variant;
        constructedSentenceCheck = "";
        foreach (TextMeshProUGUI textMeshProUGUI in contentConstructor.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (textMeshProUGUI.text!=variant)
            {
                constructedSentenceCheck = constructedSentenceCheck + " " + textMeshProUGUI.text;
            }
        }
    }


    public void CheckSentence() 
    {
        if (constructedSentenceCheck == constructedSentenceTrue)
        {
            completion[indexList] = true;
        }
        indexList++;
        SetNewList();
    }

    public void Shuffle<T>(List<T> values)
    {
        System.Random rand = new System.Random();

        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }

  

}
