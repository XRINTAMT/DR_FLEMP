using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] int maxPlayer = 3;
    [SerializeField] string sceneOnLoad;

    public bool connectedToServerOnStart;
    public bool automaticJoinRoom;


    void Start()
    {
        if (connectedToServerOnStart)
        {
            //PhotonNetwork.OfflineMode = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to server " + PhotonNetwork.CloudRegion);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect server");
    }

    public void ConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        LoadScene();
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


    private void LoadScene()
    {
        SceneManager.LoadScene(sceneOnLoad);
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
