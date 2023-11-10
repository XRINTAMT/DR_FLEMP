using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilesBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    //[SerializeField] TMP_InputField inputField;
    List <string> options = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

        int count = PlayerPrefs.GetInt("CountPlayerID",0);
        for (int i = 0; i < count; i++)
            options.Add("Player " + (options.Count+1));
        dropdown.AddOptions(options);
        PlayerPrefs.GetInt("CurrentPlayerID", 0);

        dropdown.onValueChanged.AddListener(ChoosePlayer);

    }
    public void CreatePlayer() 
    {
        options.Add("Player " + (options.Count+1));
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        PlayerPrefs.SetInt("CurrentPlayerID", (options.Count-1));
        PlayerPrefs.SetInt("CountPlayerID", options.Count);
    }

    public void ChoosePlayer(int index) 
    {
        PlayerPrefs.SetInt("CurrentPlayerID", index);
        Debug.Log(PlayerPrefs.GetInt("CurrentPlayerID", 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
