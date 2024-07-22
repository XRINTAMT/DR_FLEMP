using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneLoader : MonoBehaviour
{
    public GameObject fadeUI;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void LoadScene()
    {
        fadeUI.SetActive(true);
        cam.enabled = true;
    }
    public void LoadScene(string targetSceneName)
    {
        PhotonManager PhotonManagerObject = FindObjectOfType<PhotonManager>();

        if (PhotonManagerObject != null)
        {
            StartCoroutine(WaitForDisconnect(targetSceneName));
        }
        else
        {
            Load(targetSceneName);
        }
    }

    IEnumerator WaitForDisconnect(string targetSceneName)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return 0;
        Load(targetSceneName);
    }

    private void Load(string targetSceneName)
    {
        //fadeUI.SetActive(true);
        //cam.enabled = true;
        if (string.IsNullOrEmpty(targetSceneName))
            return;
        SceneManager.LoadScene(targetSceneName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
