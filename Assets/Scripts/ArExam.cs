using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArExam : MonoBehaviour
{
    [SerializeField] ArObjectsPool arObjectsPool;
    [SerializeField] List <Button> buttonsAudio;
    [SerializeField] Button buttonApply;
    public int totalScore;
    [SerializeField] int itemScore;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject instItem;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip Correct;
    [SerializeField] AudioClip Incorrect;
    int itemIndex;
    int chooseIndex;
    public int v1,v2,v3;
    public Button correctButton;
    public Button chooseButton;
    string language;
    InstScript instScript;
    // Start is called before the first frame update
    void Start()
    {
        instScript = FindObjectOfType<InstScript>();
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");
        buttonApply.onClick.AddListener(Apply);
        //SetNewItem();
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
        for (int i = 0; i < buttonsAudio.Count; i++)
            buttonsAudio[i].interactable = true;

        itemScore = 3;
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.transform.parent = null;
        canvas.GetComponent<ObjectUI>().item = null;

        //arObjectsPool.items[itemIndex].item.SetActive(true);
        if (instItem) Destroy(instItem);
        instItem = Instantiate(arObjectsPool.items[itemIndex].item,instScript.arTable.transform.position+new Vector3(0, 0.05f, 0),Quaternion.identity);
        canvas.transform.parent = instItem.transform;
        //canvas.transform.localPosition = Vector3.zero;
        //canvas.transform.localEulerAngles = Vector3.zero;
        canvas.GetComponent<ObjectUI>().item = instItem;
        //canvas.GetComponent<Canvas>().enabled = true;

        ShuffleButtonsAudio(buttonsAudio);

        itemIndex++;

        if (itemIndex == arObjectsPool.items.Length)
            itemIndex = 0;

    }

    void ShuffleButtonsAudio<T>(List<T> values)
    {
        v1 = itemIndex;

        v2 = Random.Range(0, arObjectsPool.items.Length);
        while (v2 == v1)
            v2 = Random.Range(0, arObjectsPool.items.Length);

        v3 = Random.Range(0, arObjectsPool.items.Length);
        while (v3 == v1 || v3 == v2)
            v3 = Random.Range(0, arObjectsPool.items.Length);
       

        System.Random rand = new System.Random();

        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }


        buttonsAudio[0].onClick.RemoveAllListeners();
        buttonsAudio[1].onClick.RemoveAllListeners();
        buttonsAudio[2].onClick.RemoveAllListeners();

        buttonsAudio[0].onClick.AddListener(() => PlaySound(v1, buttonsAudio[0]));
        buttonsAudio[1].onClick.AddListener(() => PlaySound(v2, buttonsAudio[1]));
        buttonsAudio[2].onClick.AddListener(() => PlaySound(v3, buttonsAudio[2]));

        //buttonsAudio[0].transform.GetChild(1).GetComponent<Text>().text = arObjectsPool.items[v1].title;
        //buttonsAudio[1].transform.GetChild(1).GetComponent<Text>().text = arObjectsPool.items[v2].title;
        //buttonsAudio[2].transform.GetChild(1).GetComponent<Text>().text = arObjectsPool.items[v3].title;

        correctButton = buttonsAudio[0];
    }
    void PlaySound(int index, Button button) 
    {
        if (language=="English")
            audioSource.PlayOneShot(arObjectsPool.items[index].titleAudioEnglish);
        if (language == "German")
            audioSource.PlayOneShot(arObjectsPool.items[index].titleAudioGerman);
        chooseButton = button;
        chooseIndex = index;
    
    
    }
    void Apply() 
    {
        chooseButton.interactable = false;

        if (chooseButton != correctButton)
        {
            audioSource2.PlayOneShot(Incorrect);
        }

        if (chooseButton==correctButton)
        {
            totalScore = totalScore + itemScore;
            audioSource2.PlayOneShot(Correct);
            SetNewItem();
            return;
        }
        //if (chooseButton != correctButton)
        //{
        //    itemScore--;

        //    return;
        //}

        if (itemScore==1)
        {
            totalScore = totalScore + itemScore;
            SetNewItem();
            return;
        }
        itemScore--;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
