using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocabIntroController : MonoBehaviour
{
    [Serializable]
    public class Item
    { 
        public string name;
        public string description;
        public GameObject item;
        public AudioClip audioClip;
    }

    [SerializeField] Item [] items;
    [SerializeField] Text nameItem;
    [SerializeField] Button buttonRepeatAudio;
    [SerializeField] Button buttonNext;
    AudioSource audioSource;
    int countIndex;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        buttonRepeatAudio.onClick.AddListener(RepeatAudio);
        buttonNext.onClick.AddListener(NextItem);

        items[countIndex].item.SetActive(true);
        nameItem.text = items[countIndex].name;
        audioSource.PlayOneShot(items[countIndex].audioClip);
    }

    void RepeatAudio() 
    {
        audioSource.PlayOneShot(items[countIndex].audioClip);
    }
    void NextItem()
    {
        countIndex++;

        for (int i = 0; i < items.Length; i++)
            items[i].item.SetActive(false);

        items[countIndex].item.SetActive(true);
        nameItem.text=items[countIndex].name;
        audioSource.PlayOneShot(items[countIndex].audioClip);
    }

}
