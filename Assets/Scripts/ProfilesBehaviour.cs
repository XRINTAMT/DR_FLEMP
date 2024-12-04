using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilesBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdownPatients;
    [SerializeField] TMP_Dropdown dropdownLanguage;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] string [] names;
    List <string> options = new List<string>();
    int playerId;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetString("NamesPlayer", "");
        //int count = PlayerPrefs.GetInt("CountPlayerID",0);
        //for (int i = 0; i < count; i++)
        //    options.Add("Player " + (options.Count+1));
        string nameValue = PlayerPrefs.GetString("NamesPlayer", "");
        if (nameValue!="")
        {
            names = nameValue.Split();

            for (int i = 0; i < names.Length; i++)
            {
                options.Add(names[i]);
            }
            dropdownPatients.AddOptions(options);
        }
        

     
      
        playerId=PlayerPrefs.GetInt("CurrentPlayerID", 0);

        dropdownPatients.onValueChanged.AddListener(ChoosePlayer);        
        dropdownLanguage.onValueChanged.AddListener(ChooseLanguage);
    }
    public void CreatePlayer() 
    {
        //options.Add("Player " + (options.Count+1));
        if (names.Length==0)
        {
            PlayerPrefs.SetString("NamesPlayer", inputField.text);

            string nameValue = PlayerPrefs.GetString("NamesPlayer", "");
            if (nameValue != "")
            {
                names = nameValue.Split();
                options.Clear();
                for (int i = 0; i < names.Length; i++)
                {
                    options.Add(names[i]);
                }
                dropdownPatients.ClearOptions();
                dropdownPatients.AddOptions(options);
            }

            PlayerPrefs.SetInt("CurrentPlayerID", (options.Count - 1));
            PlayerPrefs.SetInt("CountPlayerID", options.Count);
            return;
        }
        if (names.Length > 0)
        {
            PlayerPrefs.SetString("NamesPlayer", PlayerPrefs.GetString("NamesPlayer") + "\n" + inputField.text);

            string nameValue = PlayerPrefs.GetString("NamesPlayer", "");
            if (nameValue != "")
            {
                names = nameValue.Split();
                options.Clear();
                for (int i = 0; i < names.Length; i++)
                {
                    options.Add(names[i]);
                }
                dropdownPatients.ClearOptions();
                dropdownPatients.AddOptions(options);
            }

            PlayerPrefs.SetInt("CurrentPlayerID", (options.Count - 1));
            PlayerPrefs.SetInt("CountPlayerID", options.Count);
            return;
        }
     
        
    }

    public void ChoosePlayer(int index) 
    {
        PlayerPrefs.SetInt("CurrentPlayerID", index);
        Debug.Log(PlayerPrefs.GetInt("CurrentPlayerID", 0));
    }
    public void ChooseLanguage(int value)
    {
        switch (value)
        {
            case 0:
                PlayerPrefs.SetString("Language", "English");
                break;
            case 1:
                PlayerPrefs.SetString("Language", "German");
                break;
            case 2:
                PlayerPrefs.SetString("Language", "Lithuanian");
                break;
            case 3:
                PlayerPrefs.SetString("Language", "Latvian");
                break;
            case 4:
                PlayerPrefs.SetString("Language", "Swedish");
                break;
            default:
                break;
        }
    }

  
}
