using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    ManagerScene managerScene;
    [SerializeField] int maxPlayer = 3;
    public bool connectedToServerOnStart;
    public bool automaticJoinRoom;


    void Start()
    {
        managerScene = FindObjectOfType<ManagerScene>();

        if (connectedToServerOnStart)
            ConnectToServer();
    }

    public void ConnectToServer() 
    {
        //PhotonNetwork.OfflineMode = false;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECT TO SERVER");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("DISCONNECT SERVER");
    }

    public void ConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        managerScene.LoadScene("MultiplayerScene");
        Debug.Log("Create room");
    }
   

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join room FALSE");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayer;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    void Update()
    {
        if (automaticJoinRoom && PhotonNetwork.IsConnectedAndReady)
        {
            ConnectToRoom();
            automaticJoinRoom = false;
        }
    }
}
