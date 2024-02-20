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

    
    public void SetTab(int language) 
    {
        if (language == 0)
        {
            englishPanel.SetActive(true);
            germanPanel.SetActive(false);
        }
        if (language == 1)
        {
            englishPanel.SetActive(false);
            germanPanel.SetActive(true);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
