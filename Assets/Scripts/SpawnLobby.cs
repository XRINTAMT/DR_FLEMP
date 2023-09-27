using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class SpawnLobby : MonoBehaviour
{
    [SerializeField] Transform[] pos;
    public static int indexScene;
    Transform player;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<AutoHandPlayer>().transform.root;

        if (indexScene != 0)
            player.position = pos[indexScene - 1].position;

    }
}
