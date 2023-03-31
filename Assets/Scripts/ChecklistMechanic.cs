using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ChecklistMechanic : MonoBehaviour
{
    [field: SerializeField] public bool Oncoming {set; get; }
    [SerializeField] int[] correctAnswers;
    [SerializeField] Toggle[] checkBoxes;
    [SerializeField] string scenarioName;
    [SerializeField] NurseTabletRecord[] TabletRecords;
    int[] givenAnswers;

    public void Awake()
    {
        TabletRecords = GetComponentsInChildren<NurseTabletRecord>();
        CSVParser Scenario = new CSVParser("Scenarios/"+ scenarioName + "/NursingTablets");
        int i = -1;
        foreach(string[] row in Scenario.rowData)
        {
            if(i != -1 && TabletRecords.Length > i)
            {
                string[] _answers = new string[3];
                int _correct = Random.Range(0, 3);
                correctAnswers[i] = _correct;
                for(int j = 0; j < _correct; j++)
                {
                    _answers[j] = row[3 + j];
                }
                _answers[_correct] = row[2];
                for (int j = _correct+1; j <= 2; j++)
                {
                    _answers[j] = row[2 + j];
                }
                TabletRecords[i].SetData(row[0],_answers,row[1]);
            }
            i++;
        }
    }

    public void SaveAnswer(int _id, int _answer)
    {
        givenAnswers[_id] = _answer;
        if(_answer == correctAnswers[_id])
        {
            //run code if the correct answer is given
        }
        else
        {
            //run code if the wrong answer is given
        }
        CheckCompletion();
    }

    public void CheckCompletion() {
        foreach (Toggle _checkbox in checkBoxes)
        {
            if (!_checkbox.isOn)
                return;
            //check in the answer is correct and probably save the score somewhere
        }
        //Everything is done
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
