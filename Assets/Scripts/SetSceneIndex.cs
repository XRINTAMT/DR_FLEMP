using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetSceneIndex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            SpawnPlayerLobby.indexScene = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("Scene index: " + SpawnPlayerLobby.indexScene);
        }
    }
}
