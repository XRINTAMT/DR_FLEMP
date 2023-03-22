using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    PhotonView pv;
    PlayerInfo myPlayerInfo;
    PlayersList playersList;
    //public string playerName;
    public PlayerRole playerRole;


    void Start()
    {
        pv = GetComponent<PhotonView>();
        myPlayerInfo = GetComponent<PlayerInfo>();    
        playersList = FindObjectOfType<PlayersList>();
        playersList.playersList.Add(myPlayerInfo);

        //if (pv.IsMine) 
        //{
        //    isMine = true;
        //    playerName = "Name" + Random.Range(10, 99);
        //    playersList.connectedPlayerName.text = playerName + " your name";
        //    SendPlayerName(viewId, playerName);
        //}
        WhaitPlayer();
    }

    void WhaitPlayer()
    {

        if (playerRole==PlayerRole.Player)
        {
            pv.RPC("WhaitPlayerRPC", RpcTarget.All);
        }

    }

    [PunRPC]
    void WhaitPlayerRPC()
    {
        playersList.waitingCountOfPlayers++;
    }
    //public void SendPlayerName(int viewId, string playerName)
    //{
    //    if (pv.IsMine)
    //    {
    //        pv.RPC("SendPlayerNameRPC", RpcTarget.All, viewId, playerName);
    //    }
    //}

    //[PunRPC]
    //void SendPlayerNameRPC(int viewId, string playerName)
    //{
    //    if (pv.ViewID == viewId)
    //    {
    //        myPlayerInfo.playerName = playerName;
    //    }
    //}

    private void Update()
    {
        if (pv.IsMine && playerRole != PlayerRole.Viewer && !playersList.textRole.transform.parent.gameObject.activeSelf &&playersList.waitingCountOfPlayers==2)
        {
          
            if (playersList.randomRole==1)
            {
                playerRole = PlayerRole.OffGoing;
            }
            if (playersList.randomRole == 2)
            {
                playerRole = PlayerRole.OnComing;
            }  
            
            playersList.textRole.text = "You role " + playerRole + "\nYou can start";
            playersList.textRole.transform.parent.gameObject.SetActive(true);
        }
    }

    public enum PlayerRole
    {
        None,
        Viewer,
        Player,
        OffGoing,
        OnComing
    }

  
}
