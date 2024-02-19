using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTabController : MonoBehaviour
{
    public GameObject englishPanel;
    public GameObject germanPanel;
    // Start is called before the first frame update
    void Start()
    {
        string language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English"); 
        englishPanel.SetActive(language == "English");
        germanPanel.SetActive(language == "German");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
