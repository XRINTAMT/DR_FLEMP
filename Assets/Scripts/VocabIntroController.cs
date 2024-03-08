using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VocabIntroController : MonoBehaviour
{
    [SerializeField] ArObjectsPool arObjectsPool;
    [SerializeField] Text titleItem;
    [SerializeField] Text functionItem;
    [SerializeField] Button buttonRepeatAudio;
    [SerializeField] Button buttonNext;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject cannvasPanel;
    [SerializeField] AudioSource audioSource;
    GameObject instItem;
    int language;
    int soundIndex;
    public int itemIndex;
    InstScript instScript;
    public UnityEvent complete;
    // Start is called before the first frame update
    void Start()
    {
        instScript = FindObjectOfType<InstScript>();
        language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
        buttonRepeatAudio.onClick.AddListener(RepeatAudio);
        buttonNext.onClick.AddListener(SetNewItem);

        //SetNewItem();

    }

    void RepeatAudio() 
    {
        if (language==0)
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioEnglish);
        else
            audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioGerman);

        //if (language==1)
        //    audioSource.PlayOneShot(arObjectsPool.items[soundIndex].titleAudioGerman);
    }

    public void Skip() 
    {
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.transform.parent = null;
        cannvasPanel.SetActive(false);
        if (instItem) Destroy(instItem);
        instItem = null;
    }

    public void SetNewItem()
    {

        if (itemIndex == arObjectsPool.items.Length)
        {
            complete?.Invoke();
            Skip();
            itemIndex = 100;
            buttonNext.onClick.RemoveAllListeners();
            canvas.GetComponent<Canvas>().enabled = false;
            cannvasPanel.SetActive(false);
            return;
        }
       
        //for (int i = 0; i < arObjectsPool.items.Length; i++)
        //    arObjectsPool.items[i].item.SetActive(false);
        canvas.transform.parent = null;
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<ObjectUI>().item = null;

        if (instItem) Destroy(instItem);
        instItem = Instantiate(arObjectsPool.items[itemIndex].item, instScript.arTable.transform.position + new Vector3(0, 0.05f, 0), instScript.arTable.transform.rotation);

        canvas.transform.parent = instItem.transform;
        //canvas.transform.localPosition = Vector3.zero;
        //canvas.transform.localEulerAngles = Vector3.zero;
        canvas.GetComponent<ObjectUI>().item = instItem;
        //canvas.GetComponent<Canvas>().enabled = true;

        if (language == 0)
        {
            titleItem.text = arObjectsPool.items[itemIndex].titleEnglish;
            functionItem.text = arObjectsPool.items[itemIndex].functionEnglish;
        }
        if (language == 1)
        {
            titleItem.text = arObjectsPool.items[itemIndex].titleGerman;
            functionItem.text = arObjectsPool.items[itemIndex].functionGerman;
        }
      
        soundIndex = itemIndex;

        itemIndex++;
    }

}
