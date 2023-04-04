using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayersList : MonoBehaviour
{
    //public GameObject showCase;
    public Text textRole;
    public List<PlayerInfo> playersList = new List<PlayerInfo>();    
    public int randomRole;
    public int waitingCountOfPlayers;
    PhotonView pv;  
    int viewIdPlayer1;
    int viewIdPlayer2;
    bool setRoles;
    bool RpcSetRoles;
    int RpcRandRole;
    int RpcView1, RpcView2;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    void SetRoles() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomRole = Random.Range(1, 3);
            pv.RPC("SetPlayerRole", RpcTarget.All, randomRole, viewIdPlayer1, viewIdPlayer2);
        }
    }
    [PunRPC]
    void SetPlayerRole(int randomRole, int view1, int view2)
    {
        RpcRandRole = randomRole;
        RpcView1 = view1;
        RpcView2 = view2;
        RpcSetRoles = true;

        //if (randomRole == 1)
        //{
        //    for (int i = 0; i < playersList.Count; i++)
        //    {
        //        if (playersList[i].GetComponent<PhotonView>().ViewID == view1)
        //            playersList[i].playerRole= PlayerInfo.PlayerRole.OffGoing;
        //        if (playersList[i].GetComponent<PhotonView>().ViewID == view2)
        //            playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
        //    }
        //}
        //if (randomRole == 2)
        //{
        //    for (int i = 0; i < playersList.Count; i++)
        //    {
        //        if (playersList[i].GetComponent<PhotonView>().ViewID == view1)
        //            playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
        //        if (playersList[i].GetComponent<PhotonView>().ViewID == view2)
        //            playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
        //    }
        //}

        //playersList[playersList.Count - 1].SetRoles();
        //playersList[playersList.Count - 2].SetRoles();
    }
    private void Update()
    {

        if (PhotonNetwork.IsMasterClient && waitingCountOfPlayers < 2)
        {
            if (playersList.Count>=2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].playerRole == PlayerInfo.PlayerRole.Player) 
                        waitingCountOfPlayers++;
                }

                viewIdPlayer1 = playersList[playersList.Count - 1].GetComponent<PhotonView>().ViewID;
                viewIdPlayer2 = playersList[playersList.Count - 2].GetComponent<PhotonView>().ViewID;
            }
        }

        if (PhotonNetwork.IsMasterClient && !setRoles && waitingCountOfPlayers == 2)
        {
            if (playersList.Count>=2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].playerRole == PlayerInfo.PlayerRole.Player) 
                        waitingCountOfPlayers++;
                }

                viewIdPlayer1 = playersList[playersList.Count - 1].GetComponent<PhotonView>().ViewID;
                viewIdPlayer2 = playersList[playersList.Count - 2].GetComponent<PhotonView>().ViewID;
            }
        }
        if (playersList.Count>1 && RpcSetRoles)
        {
            if (RpcRandRole == 1)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == RpcView1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == RpcView2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                }
            }
            if (RpcRandRole == 2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == RpcView1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == RpcView2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                }
            }

            playersList[playersList.Count - 1].SetRoles();
            playersList[playersList.Count - 2].SetRoles();

            RpcSetRoles = false;
        }

        //if (randomRole != 0)
        //{
        //    switch (randomRole)
        //    {
        //        case 1:
        //            playersList[playersList.Count - 1].playerRole = PlayerInfo.PlayerRole.OffGoing;
        //            playersList[playersList.Count - 2].playerRole = PlayerInfo.PlayerRole.OnComing;
        //            break;
        //        case 2:
        //            playersList[playersList.Count - 1].playerRole = PlayerInfo.PlayerRole.OnComing;
        //            playersList[playersList.Count - 2].playerRole = PlayerInfo.PlayerRole.OffGoing;
        //            break;
        //        default:
        //            break;
        //    }

        //    playersList[playersList.Count - 1].SetRoles();
        //    playersList[playersList.Count - 2].SetRoles();

        //    randomRole =0;
        //}
    }

}
