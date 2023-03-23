using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayersList : MonoBehaviour
{
    public GameObject showCase;
    public Text textRole;
    public List<PlayerInfo> playersList = new List<PlayerInfo>();    
    public int randomRole;
    public int waitingCountOfPlayers;
    PhotonView pv;  
    bool setRoles;

    public int viewId1;
    public int viewId2;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
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

        if (PhotonNetwork.IsMasterClient && waitingCountOfPlayers < 2)
        {
            if (playersList.Count>=2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].playerRole == PlayerInfo.PlayerRole.Player)
                        waitingCountOfPlayers++;
                }
            }
        }

        if (PhotonNetwork.IsMasterClient && !setRoles && waitingCountOfPlayers == 2)
        {
            SetRoles();
            setRoles = true;
        }

        if (randomRole != 0)
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

            playersList[playersList.Count - 1].SetRoles();
            playersList[playersList.Count - 2].SetRoles();

            randomRole =0;
        }
    }

}
