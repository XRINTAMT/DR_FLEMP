using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSTTLanguage : MonoBehaviour
{
    [SerializeField] GameObject[] STTs;
    void Start()
    {
        STTs[PlayerPrefs.GetInt("StudyLanguage", 0)].SetActive(true);
    }

}
