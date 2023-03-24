using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NurseTabletPopupManager : MonoBehaviour
{
    [SerializeField] GameObject ExplanationWindow;
    [SerializeField] GameObject QuestionWindow;
    [SerializeField] Text ExplanationText;
    [SerializeField] Text QuestionText;
    [SerializeField] Text[] AnswerText;
    int id;

    public void ShowExplaination(string _text)
    {
        QuestionWindow.SetActive(false);
        ExplanationWindow.SetActive(true);
        ExplanationText.text = _text;
    }

    public void ShowQuestion(string _question, string[] _answer, int _questionID)
    {
        ExplanationWindow.SetActive(false);
        QuestionWindow.SetActive(true);
        QuestionText.text = _question;
        for(int i = 0; i < AnswerText.Length; i++)
        {
            AnswerText[i].text = _answer[i];
        }
        id = _questionID;
    }

    public void HidePopup()
    {
        ExplanationWindow.SetActive(false);
        QuestionWindow.SetActive(false);
    }

    public void ProcessAnswer(int optionNumber)
    {
        QuestionWindow.SetActive(false);
    }
}
