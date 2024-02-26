using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class SolutionsList
{
    public List<string> solutions = new List<string>();
}

[Serializable]
public class SolutionsListGroup
{
    public string name;
    public List<SolutionsList> group = new List<SolutionsList>();
}

[Serializable]
public class AdditionalWordsList
{
    public List<string> additionalWords = new List<string>();
}

public class SentenceScrambleTab : MonoBehaviour
{
    List<string> descriptions = new List<string>();
    //List<SolutionsList> solutionsList = new List<SolutionsList>();
    public List<SolutionsListGroup> solutionsListGroup = new List<SolutionsListGroup>();
    public List<AdditionalWordsList> additionalWordList = new List<AdditionalWordsList>();
    //[HideInInspector]
    public List<string> allWords = new List<string>();
    public List<string> correctSentences = new List<string>();
    public string sentence;
    List<bool> completion = new List<bool>();
    int indexList;    
    GridController gridController;
    AudioSource audioSource;
    public AudioClip audioCorrect;
    public AudioClip audioUncorrect;
    [SerializeField] UnityEvent OnCompletion;
    [SerializeField] Button next;
    [SerializeField] Button check;
    int[] langugeRaw = new int[] {0,1,2};
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English") == "English")
        {
            langugeRaw[0] = 0;
            langugeRaw[1] = 1;
            langugeRaw[2] = 2;
        }
        if (PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English") == "German")
        {
            langugeRaw[0] = 3;
            langugeRaw[1] = 4;
            langugeRaw[2] = 5;
        }

        gridController = GetComponent<GridController>();
        Parse();
        SetNewList();
        audioSource = GetComponent<AudioSource>();

    }

    void Parse() 
    {
        CSVParser cSVParser = new CSVParser("Scenarios/" + "B12_Recorded" + "/B12_Sentence_Construction");

        for (int i = 1; i < cSVParser.rowData.Count; i++)
        {
            descriptions.Add(cSVParser.rowData[i][langugeRaw[0]]);
            //solutionsList.Add(new SolutionsList());
            solutionsListGroup.Add(new SolutionsListGroup());
            additionalWordList.Add(new AdditionalWordsList());
            completion.Add(false);
        }
        for (int i = 1; i < cSVParser.rowData.Count; i++)
        {
            string inputString = cSVParser.rowData[i][langugeRaw[1]];
            var groups = inputString.Split(';');
            //var words = inputString.Split(' ');

            for (int j = 0; j < groups.Length; j++)
            {
                solutionsListGroup[i-1].group.Add(new SolutionsList());
                var words = groups[j].Split(' ');
                for (int k = 0; k < words.Length; k++)
                    solutionsListGroup[i - 1].group[j].solutions.Add(words[k]);
            }

            //for (int j = 0; j < words.Length; j++)
            //    solutionsList[i - 1].solutions.Add(words[j]);
        }
        for (int i = 1; i < cSVParser.rowData.Count; i++)
        {
            string inputString = cSVParser.rowData[i][langugeRaw[2]];
            var words = inputString.Split(' ');

            for (int j = 0; j < words.Length; j++)
                additionalWordList[i - 1].additionalWords.Add(words[j]);


            //for (int j = 0; j < words.Length; j++) 
            //{
            //    for (int k = 0; k < additionalWordList[i - 1].additionalWords.Count; k++)
            //    {
            //        if (additionalWordList[i - 1].additionalWords[k] != words[j])
            //        {
            //            additionalWordList[i - 1].additionalWords.Add(words[j]);
            //        }
            //    }
            //}

        }

        
    }


    void Clear() 
    {
        allWords.Clear();
        gridController.rowsSentence.Clear();
        gridController.rowsChoose.Clear();

        correctSentences.Clear();
        //correctSentence = "";
        sentence = "";

        foreach (var buttons in gridController.contentSentence.GetComponentsInChildren<Button>(true))
            Destroy(buttons.gameObject);
        foreach (var buttons in gridController.contentChoose.GetComponentsInChildren<Button>(true))
            Destroy(buttons.gameObject);

        foreach (var rows in gridController.contentSentence.GetComponentsInChildren<RowSentence>(true))
            Destroy(rows.gameObject);
        foreach (var rows in gridController.contentChoose.GetComponentsInChildren<RowSentence>(true))
            Destroy(rows.gameObject);
    }
    void SetNewList() 
    {
        Clear();
        gridController.textDescription.color = Color.white;
        gridController.textDescription.text = descriptions[indexList];
        gridController.rowsSentence.Add(Instantiate(gridController.rowSentence, gridController.contentSentence));
        gridController.rowsChoose.Add(Instantiate(gridController.rowChoose, gridController.contentChoose));


        //for (int i = 0; i < solutionsList[indexList].solutions.Count; i++)
        //{
        //    allWords.Add(solutionsList[indexList].solutions[i]);
        //    correctSentences[i] = correctSentence + " " + solutionsList[indexList].solutions[i];

        //}
        for (int i = 0; i < solutionsListGroup[indexList].group.Count; i++)
        {
            correctSentences.Add("");
            for (int j = 0; j < solutionsListGroup[indexList].group[i].solutions.Count; j++)
            {
                if (!allWords.Contains(solutionsListGroup[indexList].group[i].solutions[j]))
                    allWords.Add(solutionsListGroup[indexList].group[i].solutions[j]);
 
                correctSentences[^1] = correctSentences[^1] + " " + solutionsListGroup[indexList].group[i].solutions[j];
            }
        }



        for (int i = 0; i < additionalWordList[indexList].additionalWords.Count; i++)
        {
            if (!allWords.Contains(additionalWordList[indexList].additionalWords[i]))
            {
                //allWords.Add(additionalWordList[indexList].additionalWords[i]);
                allWords.Add(additionalWordList[indexList].additionalWords[i]);
            }
           
        }

        Shuffle(allWords);


        for (int i = 0; i < allWords.Count; i++)
            gridController.InstantiateWordChoose(allWords[i]);

    }

    public void UpdateSentence()
    {
        sentence = "";
        for (int i = 0; i < gridController.rowsSentence.Count; i++)
        {
            for (int j = 0; j < gridController.rowsSentence[i].word.Count; j++)
                sentence = sentence + " " + gridController.rowsSentence[i].word[j].variant;
        }
    }

    public void CheckBefore() 
    {
        gridController.textDescription.text = "";
        for (int i = 0; i < correctSentences.Count; i++)
        {
            if (sentence == correctSentences[i])
            {
                //check.gameObject.SetActive(false);
                //next.gameObject.SetActive(true);
                CheckSentence();
                return;
            }
        }
        audioSource.PlayOneShot(audioUncorrect);
        gridController.textDescription.text = correctSentences[0];
        gridController.textDescription.color= Color.green;
        check.gameObject.SetActive(false);
        next.gameObject.SetActive(true);

    }
    public void CheckSentence()
    {
        for (int i = 0; i < correctSentences.Count; i++)
        {
            if (sentence == correctSentences[i])
            {
                completion[indexList] = true;
                descriptions.RemoveAt(indexList);
                //solutionsList.RemoveAt(indexList);
                solutionsListGroup.RemoveAt(indexList);
                additionalWordList.RemoveAt(indexList);
                completion.RemoveAt(indexList);

                if (descriptions.Count <= indexList)
                    indexList = 0;

                if (descriptions.Count == 0)
                {
                    Clear();
                    OnCompletion.Invoke();
                    gridController.textDescription.text = "Its Done!";
                    //SceneManager.LoadScene("Lobby");
                    return;
                }
                CheckCompletion();
                SetNewList();
                audioSource.PlayOneShot(audioCorrect);
                check.gameObject.SetActive(true);
                next.gameObject.SetActive(false);
                return;
            }
        }

        indexList++;

        if (descriptions.Count <= indexList)
            indexList = 0;
        SetNewList();
        //audioSource.PlayOneShot(audioUncorrect);

        check.gameObject.SetActive(true);
        next.gameObject.SetActive(false);
        //else
        //{
        //    indexList++;

        //    if (descriptions.Count <= indexList)
        //        indexList = 0;
        //    SetNewList();
        //    audioSource.PlayOneShot(audioUncorrect);
        //}
    }

    private void CheckCompletion()
    {
        //for (int i = 0; i < completion.Count; i++)
        //{
        //    if (!completion[i])
        //        return;
        //}
        //OnCompletion.Invoke();
    }
    void Shuffle<T>(List<T> values)
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
