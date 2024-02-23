using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;

public class VocabIntroController : MonoBehaviour
{
    [SerializeField] ArObjectsPool arObjectsPool;
    [SerializeField] Text titleItem;
    [SerializeField] Text functionItem;
    [SerializeField] Button buttonRepeatAudio;
    [SerializeField] Button buttonNext;
    [SerializeField] GameObject canvas;
    GameObject instItem;
    AudioSource audioSource;
    int soundIndex;
    int itemIndex;
    string language;
    // Start is called before the first frame update
    void Start()
    {
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");
        audioSource = GetComponent<AudioSource>();

        buttonRepeatAudio.onClick.AddListener(RepeatAudio);
        buttonNext.onClick.AddListener(SetNewItem);

        SetNewItem();

    }

    void RepeatAudio() 
    {
        if (language == "English")
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioEnglish);
        if (language == "English")
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioGerman);
    }
    void SetNewItem()
    {

        //for (int i = 0; i < arObjectsPool.items.Length; i++)
        //    arObjectsPool.items[i].item.SetActive(false);
        canvas.transform.parent = null;
        canvas.SetActive(false);
        canvas.GetComponent<ObjectUI>().item = null;

        if (instItem) Destroy(instItem);
        instItem = Instantiate(arObjectsPool.items[itemIndex].item);

        canvas.transform.parent = instItem.transform;
        //canvas.transform.localPosition = Vector3.zero;
        //canvas.transform.localEulerAngles = Vector3.zero;
        canvas.GetComponent<ObjectUI>().item = instItem;
        canvas.SetActive(true);

        //titleItem.GetComponent<LocalizedText>().LocalizationKey = arObjectsPool.items[itemIndex].keyTitle;
        //titleItem.GetComponent<LocalizedText>().Localize();
        //functionItem.GetComponent<LocalizedText>().LocalizationKey = arObjectsPool.items[itemIndex].keyFunction;
        //functionItem.GetComponent<LocalizedText>().Localize();

        titleItem.text = arObjectsPool.items[itemIndex].title;
        functionItem.text = arObjectsPool.items[itemIndex].function;
        soundIndex = itemIndex;

        itemIndex++;
        if (itemIndex == arObjectsPool.items.Length)
            itemIndex = 0;
    }

}
