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
    [SerializeField] AudioSource audioSource;
    GameObject instItem;
    
    int soundIndex;
    int itemIndex;
    string language;
    InstScript instScript;
    // Start is called before the first frame update
    void Start()
    {
        instScript = FindObjectOfType<InstScript>();
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");

        buttonRepeatAudio.onClick.AddListener(RepeatAudio);
        buttonNext.onClick.AddListener(SetNewItem);

        //SetNewItem();

    }

    void RepeatAudio() 
    {
        if (language == "English")
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioEnglish);
        if (language == "English")
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioGerman);
    }

    public void Skip() 
    {
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.transform.parent = null;
        if (instItem) Destroy(instItem);
        instItem = null;
    }

    public void SetNewItem()
    {

        //for (int i = 0; i < arObjectsPool.items.Length; i++)
        //    arObjectsPool.items[i].item.SetActive(false);
        canvas.transform.parent = null;
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<ObjectUI>().item = null;

        if (instItem) Destroy(instItem);
        instItem = Instantiate(arObjectsPool.items[itemIndex].item, instScript.arTable.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);

        canvas.transform.parent = instItem.transform;
        //canvas.transform.localPosition = Vector3.zero;
        //canvas.transform.localEulerAngles = Vector3.zero;
        canvas.GetComponent<ObjectUI>().item = instItem;
        canvas.GetComponent<Canvas>().enabled = true;

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
