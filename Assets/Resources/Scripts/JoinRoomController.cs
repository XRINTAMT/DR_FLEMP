using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomController : MonoBehaviour
{
    PhotonManager photonManager;
    CodeLock codeLock;
    List<RoomInfo> roomInfo => photonManager.roomInfo;

    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        codeLock = GetComponent<CodeLock>();
    }

    public void CreateNameRoom() 
    {
        codeLock.showCode.text ="" + Random.Range(1000, 9999);
    }

    public void CreateRoom()
    {
        Debug.Log("Create room" + codeLock.showCode.text);
        photonManager.CreateRoom(codeLock.showCode.text);
    }

    public void CheckPassword()
    {
        for (int i = 0; i < roomInfo.Count; i++)
        {
            if (roomInfo[i].Name == codeLock.showCode.text)
            {
                PhotonNetwork.JoinRoom(codeLock.showCode.text);
                return;
            }
        }

        codeLock.showCode.text = "Room not found";
    }

}
