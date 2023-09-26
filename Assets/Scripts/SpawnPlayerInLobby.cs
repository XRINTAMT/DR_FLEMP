using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class SpawnPlayerInLobby : MonoBehaviour
{
    [SerializeField] Transform[] pos;
    public static int indexPos;
    Transform player;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<AutoHandPlayer>().transform.root;

        if (indexPos!=0)
            player.position = pos[indexPos - 1].position;

    }

}
