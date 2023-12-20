using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignFiller : MonoBehaviour
{
    [SerializeField] string scenarioName;
    private int language;

    // Start is called before the first frame update
    void Start()
    {
        language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
        CSVParser SignData = new CSVParser("Scenarios/" + scenarioName + "/FloatingHints");
        ShowcaseSign[] Signs = FindObjectsOfType<ShowcaseSign>();

        foreach (ShowcaseSign _sign in Signs)
        {
            foreach (string[] data in SignData.rowData)
            {
                if(data[0] == _sign.Tag)
                {
                    _sign.Fill(data[2 + language*2]);
                    break;
                }
            }
        }

        
    }
}
