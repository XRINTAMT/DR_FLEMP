using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Pun;

public class SetMicrophone : MonoBehaviourPun
{
    private void Start()
    {
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            GetComponent<Recorder>().UnityMicrophoneDevice = devices[0];
        }
    }
}
