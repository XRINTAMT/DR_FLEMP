using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject multiplayer;
    [SerializeField] List<Transform> spawnPoint;

    void Start()
    {
        int randPoint = Random.Range(0, spawnPoint.Count);
        if (player!=null)
        {
            player.transform.position = spawnPoint[randPoint].position;
            player.transform.rotation = spawnPoint[randPoint].rotation;
        }
        PhotonNetwork.Instantiate(multiplayer.name, spawnPoint[randPoint].position, spawnPoint[randPoint].rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
