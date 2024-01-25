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
    Transform currentTransformToGoTo;
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

    Dictionary<string, int>[] Campaigns = new Dictionary<string, int>[4];
    int[] CampaignDefaults = new int[4] { 0, 0, 4, 4 };

    void Awake()
    {
        Campaigns[0] = new Dictionary<string, int>
        {
            { "MRDT Room", 1 }
        };
        Campaigns[1] = new Dictionary<string, int>
        {
            { "ShowcaseScene", 2 },
            { "RecordedShiftChange", 1 },
            { "MultiplayerScene", 1 }
        };
        Campaigns[2] = new Dictionary<string, int>
        {
            { "B11", 3 },
            { "B12", 3 }
        };
        Campaigns[3] = new Dictionary<string, int>
        {
            { "B21", 3 },
            { "B22", 3 }
        };

        if (!nameScene.Equals("Lobby"))
        {
            currentTransformToGoTo = pos[Rooms[nameScene]];
            GoToTransformRoot();
        }

        
        
        //if (indexScene != 0)
        //    player.position = pos[indexScene - 1].position;
    }

    public void GoToNext()
    {
        int CurrentCampaign = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "SelectedStage", 1);
        Debug.Log("Current campaign: " + CurrentCampaign);
        CurrentCampaign -= 1;
        Debug.Log("Current campaign: " + CurrentCampaign);
        int _posToGoTo = CampaignDefaults[CurrentCampaign];
        Debug.Log("Shifting to the next one in " + Campaigns[CurrentCampaign]);
        foreach (KeyValuePair<string, int> kvp in Campaigns[CurrentCampaign])
        {
            Debug.Log("Checking " + PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + kvp.Key);
            Debug.Log(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + kvp.Key, 0));
            if (PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + kvp.Key, 0) != 0)
            {
                Debug.Log("Completed " + kvp.Key);
                _posToGoTo = kvp.Value;
            }
            else
            {
                break;
            }
        }
        currentTransformToGoTo = pos[_posToGoTo];
        GoToTransform();
    }

    private void GoToTransformRoot()
    {
        player = FindObjectOfType<AutoHandPlayer>().transform.root;
        player.position = currentTransformToGoTo.position;
        Invoke("TurnPlayer", 0.01f);
    }

    private void GoToTransform()
    {
        player = FindObjectOfType<AutoHandPlayer>().transform;
        player.position = currentTransformToGoTo.position;
        Invoke("TurnPlayer", 0.01f);
    }

    private void TurnPlayer()
    {
        Transform head = FindObjectOfType<AutoHandPlayer>().headCamera.transform;
        Debug.Log("Turning the player to " + new Vector3(player.localRotation.eulerAngles.x, currentTransformToGoTo.rotation.eulerAngles.y - head.localRotation.eulerAngles.y, player.localRotation.eulerAngles.z));
        player.localRotation = Quaternion.Euler(new Vector3(player.localRotation.eulerAngles.x, currentTransformToGoTo.rotation.eulerAngles.y - head.localRotation.eulerAngles.y, player.localRotation.eulerAngles.z));
    }

    
}
