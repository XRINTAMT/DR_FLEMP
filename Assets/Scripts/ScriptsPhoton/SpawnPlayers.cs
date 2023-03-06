using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerNetworkPrefab;
    [SerializeField] List<Transform> spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        int randSpawnPoint = Random.Range(0, spawnPoints.Count);
        if (playerNetworkPrefab!=null)
            PhotonNetwork.Instantiate(playerNetworkPrefab.name, spawnPoints[randSpawnPoint].position, spawnPoints[randSpawnPoint].rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
