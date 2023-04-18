using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NurseTabletRecord : MonoBehaviour
{
    public Text MainText;
    [SerializeField] public GameObject AudioHintButton;
    [SerializeField] private NurseTabletPopupManager PopupManager;
    [SerializeField] private ChecklistMechanic ChecklistManager;
    public Toggle checkbox;
    [SerializeField] private string explainationText;
    [SerializeField] private string[] answerText;
    [SerializeField] private int id;
    private string questionText;

    public void Start()
    {
        
    }

    public void SetData(string _questionText, string[] _answerText, string _explainText)
    {
        explainationText = _explainText;
        questionText = _questionText;
        MainText.text = _questionText;
        answerText = _answerText;
    }

    public void ShowAudioHint()
    {
        AudioHintButton.SetActive(true);
    }

    public void ShowExplaination()
    {
        PopupManager.ShowExplaination(explainationText);
    }

    public void CheckboxChecked()
    {
        if (checkbox.isOn)
        {
            if (ChecklistManager.Oncoming)
            {
                PopupManager.ShowQuestion(questionText, answerText, id);
            }
        }


        if (ChecklistManager.indicate)
            ChecklistManager.DisableIndicate();
    }
}
