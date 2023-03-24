using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistMechanic : MonoBehaviour
{
    [field: SerializeField] public bool Oncoming {set; get; }
    [SerializeField] int[] correctAnswers;
    [SerializeField] Toggle[] checkBoxes;
    //[SerializeField] NurseTabletRecord[] TabletRecords;
    int[] givenAnswers;

    public void Awake()
    {

    }

    public void SaveAnswer(int _id, int _answer)
    {
        givenAnswers[_id] = _answer;
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
