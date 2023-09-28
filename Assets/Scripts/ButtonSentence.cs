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
        }
        if (inConstructor)
        {
            sentenceScrambleTab.ReturnVariant(variant);

            Destroy(gameObject);
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
        }
    
    }
    public void pointerExit()
    {
        if (sentenceScrambleTab.buttonChoose != null && inConstructor)
        {
            variant = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant = GetComponentInChildren<TextMeshProUGUI>().text;

            GetComponentInChildren<TextMeshProUGUI>().text = variant;
            sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text= sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant;
        }

    }
    public void pointerUp()
    {
        if (inConstructor)
        {
            variant = GetComponentInChildren<TextMeshProUGUI>().text;
            sentenceScrambleTab.buttonChoose.GetComponent<ButtonSentence>().variant = sentenceScrambleTab.buttonChoose.GetComponentInChildren<TextMeshProUGUI>().text;

            sentenceScrambleTab.buttonChoose = null;
        }
      
    }
    public void pointerDown()
    {
        if (inConstructor)
        {
            sentenceScrambleTab.buttonChoose = GetComponent<Button>();
        }
      
    }


}
