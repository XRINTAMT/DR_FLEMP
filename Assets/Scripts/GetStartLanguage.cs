using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

public class GetStartLanguage : MonoBehaviour
{
    string language;
    // Start is called before the first frame update
    void Start()
    {
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");
        StartLang();
    }
    void StartLang() 
    {
        LocalizationManager.Language = language;
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
