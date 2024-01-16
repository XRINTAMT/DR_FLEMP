using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;
using ChatGPT_Patient;
using WebSocketSharp;

public class EquipmentQuizBehaviour : MonoBehaviour
{
    [SerializeField] private Text answerText;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource incorrectSound;
    [SerializeField] private InteractionHandler STTInput;

    [System.Serializable]
    public class AnswerOption
    {
        public string name;
        public string[] possibleAnswers;
    }

    public List<AnswerOption> answerOptions = new List<AnswerOption>();

    private string rightAnswer;

    void Start()
    {
        GenerateRandomRightAnswer();
        UpdateAnswerText();

    }

    void GenerateRandomRightAnswer()
    {
        if (answerOptions.Count > 0)
        {
            AnswerOption randomOption = answerOptions[Random.Range(0, answerOptions.Count)];
            rightAnswer = randomOption.name;
        }
    }

    void UpdateAnswerText()
    {
        answerText.text = "Expected Answer: " + rightAnswer;
    }

    public void Answer()
    {
        string _userAnswer = STTInput.LastNonNullPhrase;

        STTInput.LastNonNullPhrase = "";
        if (_userAnswer.IsNullOrEmpty())
        {
            return;
        }

        Debug.Log("Answer: " + _userAnswer);
        bool _isCorrect = CheckAnswer(_userAnswer);

        if (_isCorrect)
        {
            PlayCorrectSound();
        }
        else
        {
            PlayIncorrectSound();
        }

        GenerateRandomRightAnswer();
        UpdateAnswerText();
    }

    bool CheckAnswer(string _userAnswer)
    {
        foreach (AnswerOption _option in answerOptions)
        {
            // Consider any possible answer for the selected option as correct
            foreach (string _possibleAnswer in _option.possibleAnswers)
            {
                // You might want to implement a more robust comparison here (case-insensitive, ignoring spaces, etc.)
                if (_userAnswer.ToLower().Trim().Contains(_possibleAnswer.ToLower().Trim()) && _option.name == rightAnswer)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void PlayCorrectSound()
    {
        correctSound.Play();
    }

    void PlayIncorrectSound()
    {
        incorrectSound.Play();
    }
}
