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
        Dictionary<string, int> Rooms = new Dictionary<string, int>
        {
            { "MRDT Room", 1 },
            { "Diabetes Amnesis Room", 1 },
            { "ShowcaseScene", 2 },
            { "RecordedShiftChange", 1 },
            { "MultiplayerScene", 1 },
            { "B11", 3 },
            { "B12", 3 },
            { "B21", 3 },
            { "B22", 3 }
        };

        player = FindObjectOfType<AutoHandPlayer>().transform.root;

        player.position = pos[Rooms[nameScene]].position;

        //if (indexScene != 0)
        //    player.position = pos[indexScene - 1].position;

    }
}
