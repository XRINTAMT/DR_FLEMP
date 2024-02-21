using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
 public class Item
 {
    public string title;
    public string function;
    public string keyTitle;
    public string keyFunction;
    public GameObject item;
    public AudioClip titleAudioEnglish;
    public AudioClip titleAudioGerman;
}

public class ArObjectsPool : MonoBehaviour
{

    public Item [] items;
}
