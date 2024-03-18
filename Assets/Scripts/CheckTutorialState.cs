using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorialState : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    bool completion;
    // Start is called before the first frame update
    void Start()
    {
        completion = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Tutorial", 0) == 1;
        if (!completion)
        {
            tutorial.SetActive(true);
        }
    }
  
}
