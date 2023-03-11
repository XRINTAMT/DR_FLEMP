using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    RoomOptions roomOptions = new RoomOptions();  
    public List<RoomInfo> roomInfo = new List<RoomInfo>();
    [SerializeField] int maxPlayers = 3; 
  
    public bool connectedToServerOnStart;
    public bool automaticJoinRoom;
   
    void Start()
    {
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        if (connectedToServerOnStart)
            ConnectToServer();
    }

    #region Connect to server
    public void ConnectToServer() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECT TO SERVER");  
        //PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("DISCONNECT SERVER");
    }
    public override void OnJoinedLobby()
    {
        //if (automaticJoinRoom)
        //    ConnectToRandomRoom();
    }
    #endregion

    #region Create room
    public void CreateRoom(string nameRoom) 
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(nameRoom, roomOptions);
        Debug.Log("Create room " + nameRoom);
    }
    //public override void OnCreatedRoom()
    //{
    //    SceneManager.LoadScene("MultiplayerScene");
    //}
    #endregion

    #region Join room
    public void ConnectToRandomRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join room FALSE");
        //CreateRoom("2345");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {   
        Debug.Log("Join room TRUE");
        SceneManager.LoadScene("MultiplayerScene");
    }
    #endregion

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
            roomInfo.Add(info);
    }

    private void Update()
    {
        if (automaticJoinRoom && PhotonNetwork.IsConnectedAndReady)
        {
            ConnectToRandomRoom();
            automaticJoinRoom = false;
        }
    }
}
