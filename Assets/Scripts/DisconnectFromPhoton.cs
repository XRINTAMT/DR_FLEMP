using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectFromPhoton : MonoBehaviour
{
    void Start()
    {
        PhotonManager PhotonManagerObject = FindObjectOfType<PhotonManager>();

        if (PhotonManagerObject != null)
        {
            PhotonManagerObject.Leave();
        }
    }
}
