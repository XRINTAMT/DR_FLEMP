using System;
using System.Collections;
using System.Collections.Generic;
using AICharacter;
using ChatGPT_Patient;
using OpenAI;
using UnityEngine;
using WebSocketSharp;
using Meta.WitAi.TTS.Utilities;
using CharacterInfo = AICharacter.CharacterInfo;

public class ChatCharacter : MonoBehaviour
{
    [SerializeField] private InteractionHandler STTInput;
    [SerializeField] private CharacterInfo info;
    private ChatHistory _history;
    [SerializeField] private EmbeddingDB _embeddingDB;
    private OpenAIApi _openAI;
    public bool targeted = false;
    [SerializeField] private int numberOfHistoryEntries = 3;
    [SerializeField] private int numberOfDeepHistoryEntries = 3;
    [SerializeField] private SittingWheelChair _wheelChair;
    [SerializeField] private TTSSpeaker _speaker;
    [SerializeField] private WitAutoReactivation WitReact;
    private string[] sentences;

    void Start()
    {
        _history = new ChatHistory();
        _openAI = new OpenAIApi("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");
        STTInput = FindAnyObjectByType<InteractionHandler>();
    }

    public void setTargeted(bool t)
    {
        targeted = t;
    }

    public void RecieveSTT()
    {
        string stt = STTInput.LastNonNullPhrase;
        
        if (!targeted) return;
        
        STTInput.LastNonNullPhrase = "";
        if (stt.IsNullOrEmpty())
        {
            return;
        }
        Debug.Log(info.name+": "+stt);
        StartCoroutine(openAIChat(stt));

    }

    public List<ChatMessage> ConstructPrompt(ChatMessage userMessage, List<float> embedding)
    {
        var deepHistory = _history.GetDeepHistory(numberOfDeepHistoryEntries, 2, embedding);
        var history = _history.GetHistory(numberOfHistoryEntries);
        var background = new ChatMessage()
        {
            Role = "system",
            Content = "You are this character: "+ info.description,
        };
        var instruction = new ChatMessage()
        {
            Role = "system",
            Content = "Give only short and specific answers to any questions. Answer with 1 single sentence.",
        };

        var prompt = new List<ChatMessage>();
        prompt.AddRange(deepHistory);
        prompt.AddRange(history);
        prompt.Add(background);
        prompt.Add(userMessage);
        prompt.Add(instruction);
        
        foreach(var m in prompt) Debug.Log(m.Content);

        return prompt;
    }

    public IEnumerator openAIChat(string userMessage)
    {
        Debug.Log("openAIChat started");
        var embeddingTask = _embeddingDB.GetEmbedding(userMessage);
        
        while (!embeddingTask.IsCompleted)
        {
            yield return null;
        };

        var userChatMessage = new ChatMessage()
        {
            Role = "user",
            Content = userMessage,
        };

        var prompt = ConstructPrompt(userChatMessage, embeddingTask.Result);

        StartCoroutine(SendPrompt(prompt, userChatMessage, embeddingTask.Result));

    }


    public IEnumerator SendPrompt(List<ChatMessage> messages, ChatMessage userMessage, List<float> embedding)
    {
        Debug.Log("sendPrompt started");
        var completionTask = _openAI.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model ="gpt-3.5-turbo-0613",
            Messages = messages,
            Functions = new List<FunctionDescription>
            {
                new FunctionDescription
                {
                    Name = "get_into_wheelchair",
                    Description = "Move from sitting on the bench to sitting in the wheelchair",
                    Parameters = new Parameters()
                    {
                        Type = "object",
                        Properties = new Dictionary<string, Property>(),
                        Required = new List<string>()
                    }
                }
            }
        });

        
        Debug.Log("waiting on response started");
        while (!completionTask.IsCompleted)
        {
            yield return null;
        };

        var response = completionTask.Result.Choices[0].Message;
        Debug.Log("response recieved: "+response.Content);


        if (response.FunctionCall != null)
        {
            var functionReturn = new ChatMessage()
            {
                Role = "function",
                Name = response.FunctionCall?.Name,
            };
            switch(response.FunctionCall?.Name) 
            {
                case "get_into_wheelchair":
                    functionReturn.Content = GetIntoWheelchar().ToString();
                    break;
                default:
                    Debug.Log("No function called "+response.FunctionCall?.Name);
                    break;
            }
            messages.Add(functionReturn);
            StartCoroutine(SendPrompt(messages, userMessage, embedding));
        }
        else
        {
            _history.NewEntry(userMessage, response, embedding);
            SendResponseToTTS(response.Content);
        }

    }

    private void SendResponseToTTS(string response)
    {
        Debug.Log("should be pronounced using TTS: "+response);
        sentences = response.Split(new char[] { '\n', '.', '?', ';', '!' });
        StartCoroutine(PlayAndWait());

        IEnumerator PlayAndWait()
        {
            foreach (string sentence in sentences)
            {
                if (sentence == string.Empty)
                    continue;
                //_speaker.AudioSource.clip = PhrasesPool.Draw();
                _speaker.AudioSource.Play();
                _speaker.Speak(sentence);

                while (!_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                while (_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                WitReact.temporarilyIgnore = true;
            }
            WitReact.temporarilyIgnore = false;
            Debug.Log("Giving control back to the stt");
        }
    }

    private bool GetIntoWheelchar()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}