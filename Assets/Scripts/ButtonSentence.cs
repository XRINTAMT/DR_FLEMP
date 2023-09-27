using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSentence : MonoBehaviour
{
    public bool inConstructor;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetVariant);
    }
    void SetVariant() 
    {
        if (!inConstructor)
        {
            FindObjectOfType<SentenceScrambleTab>().SetVariant(GetComponentInChildren<TextMeshProUGUI>().text);

            Destroy(gameObject);
        }
        if (inConstructor)
        {
            FindObjectOfType<SentenceScrambleTab>().ReturnVariant(GetComponentInChildren<TextMeshProUGUI>().text);

            Destroy(gameObject);
        }

    }
    public void pointerEnter() 
    {
        Debug.Log("Enter");
    
    }
    public void pointerDown()
    {
        Debug.Log("Down");

    }


}
