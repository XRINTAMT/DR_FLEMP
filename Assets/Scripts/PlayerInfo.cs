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
    public PlayerRole playerRole;


    void Start()
    {
        pv = GetComponent<PhotonView>();
        myPlayerInfo = GetComponent<PlayerInfo>();    
        playersList = FindObjectOfType<PlayersList>();
        playersList.playersList.Add(myPlayerInfo);
    }


    public void SetRoles() 
    {
        if (pv.IsMine && playerRole != PlayerRole.Viewer)
        {
            if (playerRole == PlayerRole.OffGoing) 
            {
                playersList.showCase.SetActive(true);
                checklistMechanic.Oncoming = false;
            }
            if (playerRole == PlayerRole.OnComing)
            {
                playersList.showCase.SetActive(false);
                checklistMechanic.Oncoming = true;
            }
            grabbableTablet.enabled = true;
            playersList.textRole.text = "You role " + playerRole + "\nYou can start";
            playersList.textRole.transform.parent.gameObject.SetActive(true);
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
