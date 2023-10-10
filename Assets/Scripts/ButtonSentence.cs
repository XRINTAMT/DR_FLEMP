using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSentence : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool inConstructor;
    SentenceScrambleTab sentenceScrambleTab;
    public string variant;
    public GameObject spawnPoint;
    GridController gridController;
    float buttonWidth;
    ButtonSentence thisbButton;


    // Start is called before the first frame update
    void Start()
    {
        sentenceScrambleTab = FindObjectOfType<SentenceScrambleTab>();
        variant = GetComponentInChildren<TextMeshProUGUI>().text;
        gridController = FindObjectOfType<GridController>();
        buttonWidth = GetComponent<RectTransform>().rect.width;
    }

    public void pointerEnter()
    {
        if (sentenceScrambleTab.buttonChoose != null && inConstructor)
        {
            sentenceScrambleTab.buttonChoose.variant = text.text;
            variant = sentenceScrambleTab.buttonChoose.text.text;
   

            //text.text = sentenceScrambleTab.buttonChoose.variant;
            //sentenceScrambleTab.buttonChoose.text.text = variant;

            //variant = text.text;
            //sentenceScrambleTab.buttonChoose.variant = sentenceScrambleTab.buttonChoose.text.text;

            GetComponent<Image>().color = Color.green;
        }
    }
    public void pointerExit()
    {
        if (inConstructor )
        {

            if (sentenceScrambleTab.buttonChoose != null)
            {

                variant = text.text;
                sentenceScrambleTab.buttonChoose.variant = sentenceScrambleTab.buttonChoose.text.text;

                //variant = sentenceScrambleTab.buttonChoose.text.text;
                //sentenceScrambleTab.buttonChoose.variant = text.text;

                //text.text = variant;
                //sentenceScrambleTab.buttonChoose.text.text = sentenceScrambleTab.buttonChoose.variant;

                if (sentenceScrambleTab.buttonChoose != GetComponent<ButtonSentence>())
                    GetComponent<Image>().color = Color.white;
            }
        }

        if (thisbButton!=null)
        {
            if (!sentenceScrambleTab.buttonChoose)
                sentenceScrambleTab.buttonChoose = GetComponent<ButtonSentence>();
        }
        thisbButton = null;

    }
    public void pointerUp()
    {
        if (!inConstructor)
        {
            sentenceScrambleTab.SetVariant(variant);
            Destroy(gameObject);
            return;
        }

        if (inConstructor && !sentenceScrambleTab.buttonChoose && thisbButton)
        {
            gridController.Remove(this);
            sentenceScrambleTab.ReturnVariant(variant);
            Destroy(gameObject);
            return;
        }

        if (inConstructor && sentenceScrambleTab.buttonChoose)
        {
            var buttonsSentence = FindObjectsOfType<ButtonSentence>();
            for (int i = 0; i < buttonsSentence.Length; i++) 
            {
                buttonsSentence[i].GetComponent<Image>().color = Color.white;

                if (buttonsSentence[i].text.text != buttonsSentence[i].variant)
                    buttonsSentence[i].text.text = buttonsSentence[i].variant;
            }

            sentenceScrambleTab.buttonChoose = null;
            return;
        }
    }
    public void pointerDown()
    {
        GetComponent<Image>().color = Color.green;
        thisbButton = this;
    }



    private void Update()
    {

        //if (buttonWidth != GetComponent<RectTransform>().rect.width)
        //{
        //    if (gridController.WordsLenghts(this) > gridController.contentWidth)
        //    {
        //        Debug.Log(3);
        //    }
        //    gridController.UpdatePostions();
        //    buttonWidth = GetComponent<RectTransform>().rect.width;
        //}
        if (buttonWidth != GetComponent<RectTransform>().rect.width)
        {

            if (inConstructor)
            {
                gridController.UpdatePostions(gridController.rowsSentence);
            }
            if (!inConstructor)
            {
                gridController.UpdatePostions(gridController.rowsChoose);
            }

            buttonWidth = GetComponent<RectTransform>().rect.width;
        }
    }
}

