using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayersList : MonoBehaviour
{   

    public Text textRole;
    public List<PlayerInfo> playersList = new List<PlayerInfo>();
    public bool serverOnComputer;
    PhotonView pv;  
    bool setRoles;
    int randomRole;
    int waitingCountOfPlayers=2;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if (serverOnComputer)
            waitingCountOfPlayers = 3;
      
    }
    void SetRoles() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomRole = Random.Range(1, 3);
            pv.RPC("SetPlayerRole", RpcTarget.All, randomRole);
        }
    }

    [PunRPC]
    void SetPlayerRole(int randomRole)
    {
        this.randomRole=randomRole;
    }
    private void Update()
    {

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == waitingCountOfPlayers && !setRoles)
        {
            SetRoles();
            setRoles = true;
        }

        if (playersList.Count==2 && randomRole!=0)
        {
            switch (randomRole)
            {
                case 1:
                    playersList[playersList.Count - 1].playerRole = PlayerInfo.PlayerRole.OffGoing;
                    playersList[playersList.Count - 2].playerRole = PlayerInfo.PlayerRole.OnComing;
                    break;
                case 2:
                    playersList[playersList.Count - 1].playerRole = PlayerInfo.PlayerRole.OnComing;
                    playersList[playersList.Count - 2].playerRole = PlayerInfo.PlayerRole.OffGoing;
                    break;
                default:
                    break;
            }
           randomRole=0;
        }
    }

}
