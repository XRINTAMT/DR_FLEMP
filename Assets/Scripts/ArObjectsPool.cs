using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
 public class Item
 {
    public string title;
    public string function;
    public GameObject item;
    public AudioClip audioPronunciation;
 }

public class ArObjectsPool : MonoBehaviour
{

    public Item [] items;
}
