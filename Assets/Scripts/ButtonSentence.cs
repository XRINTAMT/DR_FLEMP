using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSentence : MonoBehaviour
{
    public bool inConstructor;
    SentenceScrambleTab sentenceScrambleTab;
    public string variant;
    bool startChoose;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetVariant);
        sentenceScrambleTab = FindObjectOfType<SentenceScrambleTab>();
        variant = GetComponentInChildren<TextMeshProUGUI>().text;
    }
    void SetVariant()
    {
        if (!inConstructor)
        {
            sentenceScrambleTab.SetVariant(variant);

            Destroy(gameObject);
            return;
        }
        if (inConstructor && !startChoose)
        {
            sentenceScrambleTab.ReturnVariant(variant);

            Destroy(gameObject);
            return;
        }
        if (inConstructor && startChoose)
        {
            startChoose = false;
            return;
        }

    }
    public void pointerEnter()
    {
        if (sentenceScrambleTab.buttonChoose != null && inConstructor)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text = variant;

            variant = GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;

        
           
            if (sentenceScrambleTab.buttonChoose!=GetComponent<Button>())
            {
                GetComponent<Image>().color = Color.green;
                sentenceScrambleTab.buttonChoose.GetComponent<Image>().color = Color.white;
                sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().button = GetComponent<Button>();
            }
            if (sentenceScrambleTab.buttonChoose == GetComponent<Button>())
            {
                GetComponent<Image>().color = Color.green;
            }

        }

    }
    public void pointerExit()
    {
        if (sentenceScrambleTab.buttonChoose != null && inConstructor)
        {
            variant = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant = GetComponentInChildren<TextMeshProUGUI>().text;

            GetComponentInChildren<TextMeshProUGUI>().text = variant;
            sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text = sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant;


            GetComponent<Image>().color = Color.white;
            sentenceScrambleTab.buttonChoose.GetComponent<Image>().color = Color.green;
        }
        if (inConstructor && GetComponent<Button>()== sentenceScrambleTab.buttonChoose)
        {
            startChoose = true;
        }
    }
    public void pointerUp()
    {
        if (inConstructor)
        {
            variant = GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;
            if (button)
            {
                button.GetComponent<Image>().color = Color.white;
                button = null;

            }
            sentenceScrambleTab.buttonChoose = null;
        }

        startChoose = false;
    }
    public void pointerDown()
    {
        if (inConstructor)
        {
            GetComponent<Image>().color = Color.green;
            sentenceScrambleTab.buttonChoose = GetComponent<Button>();
        }

    }


}
