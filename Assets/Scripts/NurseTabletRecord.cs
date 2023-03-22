using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseTabletRecord : MonoBehaviour
{
    [SerializeField] private GameObject AudioHintButton;
    public void ShowAudioHint()
    {
        AudioHintButton.SetActive(true);
    }
}
