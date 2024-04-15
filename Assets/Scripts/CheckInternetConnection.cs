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
        Debug.Log("Trying to connect to Cloudflare...");
        string IPAddress = "1.1.1.1";
        Ping ping = new Ping(IPAddress);
        float startTime = Time.time;
        while (!(ping.time > 0) && Time.time - startTime < 2)
        {
            yield return null;
        }
        if (ping.time > 0)
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
