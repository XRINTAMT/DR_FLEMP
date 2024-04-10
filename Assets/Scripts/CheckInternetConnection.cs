using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckInternetConnection : MonoBehaviour
{
    public UnityEvent OnConnected;
    public UnityEvent OnFailed;

    void OnEnable()
    {
        StartCoroutine(PingGoogle());
    }

    System.Collections.IEnumerator PingGoogle()
    {
        Debug.Log("Trying to connect to google");
        string googleIPAddress = "8.8.8.8";
        Ping ping = new Ping(googleIPAddress);
        float startTime = Time.time;
        while (!ping.isDone && Time.time - startTime < 2)
        {
            yield return null;
        }
        if (ping.isDone)
        {
            Debug.Log("Connected to Google");
            if (OnConnected != null)
                OnConnected.Invoke();
        }
        else
        {
            Debug.Log("No connection");
            if (OnFailed != null)
                OnFailed.Invoke();
        }
        Debug.Log("DOne");
    }
}
