using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSentence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetVariant);
    }
    void SetVariant() 
    {
        FindObjectOfType<SentenceScrambleTab>().SetVariant(GetComponentInChildren<TextMeshProUGUI>().text);

        Destroy(gameObject);
    }

}
