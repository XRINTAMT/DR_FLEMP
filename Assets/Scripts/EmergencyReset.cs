using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class EmergencyReset : MonoBehaviour
{
    Transform Player;
    Vector3 DefaultPosition;
    Quaternion DefaultRotation;

    void Start()
    {
        Player = FindObjectOfType<AutoHandPlayer>().transform;
        DefaultPosition = Player.localPosition;
        DefaultRotation = Player.localRotation;
    }

    public void ResetPosition()
    {
        Debug.Log("Player position: " + Player.localPosition);
        Debug.Log("Moving to: " + DefaultPosition);
        Player.localPosition = DefaultPosition;
        Player.localRotation = DefaultRotation;
    }
}
