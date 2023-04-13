using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayersList : MonoBehaviour
{
    public List<GameObject> showCase;
    public Text textRole;
    public List<PlayerInfo> playersList = new List<PlayerInfo>();
    public List<PlayerInfo> playersReady = new List<PlayerInfo>();
    PhotonView pv;
    int randomRole;
    int waitingCountOfPlayers;
    int viewIdPlayer1;
    int viewIdPlayer2;
    bool setRoles;
    bool forceSet;
    int countOfList;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void ForceSetRoles()
    {
        forceSet = true;
    }

    void SetRoles()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomRole = Random.Range(1, 3);
            pv.RPC("SetPlayerRole", RpcTarget.All, randomRole, viewIdPlayer1, viewIdPlayer2);
        }
    }

    public void RestartGame(bool sameRoles)
    {
        if (!sameRoles)
        {
            randomRole = (randomRole == 1) ? 2 : 1;
        }
        pv.RPC("SetPlayerRole", RpcTarget.All, randomRole, viewIdPlayer1, viewIdPlayer2);
        pv.RPC("ResetChecklists", RpcTarget.All);
    }

    [PunRPC]
    void ResetChecklists()
    {
        ChecklistMechanic[] Tablets = FindObjectsOfType<ChecklistMechanic>();
        foreach (ChecklistMechanic tablet in Tablets)
        {
            tablet.Reset();
        }
    }

    [PunRPC]
    void SetPlayerRole(int randomRole, int view1, int view2)
    {
        this.randomRole = randomRole;
        viewIdPlayer1 = view1;
        viewIdPlayer2 = view2;
        setRoles = true;
    }

    private void Update()
    {

        if ((playersList.Count != countOfList && playersReady.Count < 2) || forceSet)
        {
            if (playersList[playersList.Count - 1].playerRole == PlayerInfo.PlayerRole.Player)
            {
                playersReady.Add(playersList[playersList.Count - 1]);
            }

            if ((playersReady.Count == 2 && PhotonNetwork.IsMasterClient) || forceSet)
            {
                viewIdPlayer1 = playersReady[playersReady.Count - 1].GetComponent<PhotonView>().ViewID;
                if(!forceSet)
                    viewIdPlayer2 = playersReady[playersReady.Count - 2].GetComponent<PhotonView>().ViewID;
                SetRoles();
                forceSet = false;
            }
            countOfList = playersList.Count;
        }

        if (setRoles)
        {
            if (randomRole == 1)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                }
            }
            if (randomRole == 2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                }
            }

            playersList[playersList.Count - 1].SetRoles();
            if(playersList.Count > 1)
                playersList[playersList.Count - 2].SetRoles();
            setRoles = false;
        }
    }

}
