using System.Collections;
using System.Collections.Generic;
using Autohand;
using Photon.Pun;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] Grabbable grabbableTablet;
    [SerializeField] ChecklistMechanic checklistMechanic;
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
        //    WhaitPlayer();
        //}
      
    }

    //void WhaitPlayer()
    //{
    //    if (playerRole==PlayerRole.Player)
    //    {
    //        pv.RPC("WhaitPlayerRPC", RpcTarget.All);
    //    }
    //}

    //[PunRPC]
    //void WhaitPlayerRPC()
    //{
    //    playersList.waitingCountOfPlayers++;
    //}

    public void SetRoles() 
    {
        if (pv.IsMine && playerRole != PlayerRole.Viewer)
        {
            if (playerRole == PlayerRole.OffGoing) 
            {
                //playersList.showCase.SetActive(true);
                checklistMechanic.Oncoming = false;
            }
            if (playerRole == PlayerRole.OnComing)
            {
                //playersList.showCase.SetActive(false);
                checklistMechanic.Oncoming = true;
            }

            playersList.textRole.text = "You role " + playerRole + "\nYou can start";
            playersList.textRole.transform.parent.gameObject.SetActive(true);
        }
        if (!pv.IsMine)
        {
            grabbableTablet.enabled = false;
        }

    }

    private void Update()
    {

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
