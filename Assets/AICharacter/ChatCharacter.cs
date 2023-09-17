using System;
using System.Collections;
using System.Collections.Generic;
using AICharacter;
using ChatGPT_Patient;
using UnityEngine;
using WebSocketSharp;
using CharacterInfo = AICharacter.CharacterInfo;

public class ChatCharacter : MonoBehaviour
{
    [SerializeField] private InteractionHandler STTInput;
    [SerializeField] private CharacterInfo info;
    private ChatHistory _history;
    public bool targeted = false;
    void Start()
    {
        _history = new ChatHistory();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
