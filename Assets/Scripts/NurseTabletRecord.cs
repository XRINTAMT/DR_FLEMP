using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NurseTabletRecord : MonoBehaviour
{
    [SerializeField] private Text MainText;
    [SerializeField] private GameObject AudioHintButton;
    [SerializeField] private NurseTabletPopupManager PopupManager;
    [SerializeField] private ChecklistMechanic ChecklistManager;
    [SerializeField] private Toggle checkbox;
    [SerializeField] private string explainationText;
    [SerializeField] private string[] answerText;
    [SerializeField] private int id;
    private string questionText;

    public void Start()
    {
        questionText = MainText.text;
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
    }
}
