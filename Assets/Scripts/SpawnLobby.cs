using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class SpawnLobby : MonoBehaviour
{
    [SerializeField] Transform[] pos;
    public static int indexScene;
    public static string nameScene;

    Transform player;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<AutoHandPlayer>().transform.root;

        if (nameScene == "ShowcaseScene") 
        {
            player.position = pos[0].position;
        }

        if (nameScene == "MultiplayerScene") 
        {
            player.position = pos[1].position;
        }

        if (nameScene == "B11" || nameScene == "B22") 
        {
            player.position = pos[4].position;
        }

        if (nameScene == "B12" || nameScene == "B21") 
        {
            player.position = pos[3].position;
        }
       



        //if (indexScene != 0)
        //    player.position = pos[indexScene - 1].position;

    }
}
