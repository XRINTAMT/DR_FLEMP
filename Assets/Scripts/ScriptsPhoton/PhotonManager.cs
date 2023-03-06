using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string region;
    [SerializeField] int maxPlayer=3;
    [SerializeField] string sceneOnLoad;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("You connected to " + PhotonNetwork.CloudRegion);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("You Disconnected");
    }

    public void CreateRoom() 
    {
        if (!PhotonNetwork.IsConnected)
            return;
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayer;
        string roomName = "Room" + Random.Range(1, 100);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        LoadScene();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxPlayer });
        LoadScene();
    }



    private void LoadScene() 
    {
        SceneManager.LoadScene(sceneOnLoad);
    }
    void Update()
    {
        
    }
}
