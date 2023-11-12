using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSTTLanguage : MonoBehaviour
{
    [SerializeField] GameObject[] STTs;
    void Start()
    {
        STTs[PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0)].SetActive(true);
    }

}
