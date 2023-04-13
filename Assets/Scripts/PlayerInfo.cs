using System.Collections;
using System.Collections.Generic;
using Autohand;
using Photon.Pun;
using UnityEngine;
using UnityTemplateProjects;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] Grabbable grabbableTablet;
    [SerializeField] ChecklistMechanic checklistMechanic;
    [SerializeField] Vector3 OnComingLocation;
    [SerializeField] Vector3 OffGoingLocation;
    public GameObject AutoHandPlayer;
    [SerializeField] GameObject microphone;
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

        //if (microphone && !pv.IsMine)
        //    microphone.SetActive(false);

        if (playerRole==PlayerRole.Viewer && pv.IsMine)
        {
            AutoHandPlayer autoHandPlayer = FindObjectOfType<AutoHandPlayer>();
            autoHandPlayer.headCamera.gameObject.AddComponent<SimpleCameraController>();
            autoHandPlayer.GetComponent<Rigidbody>().isKinematic=true;
            foreach (Renderer rend in autoHandPlayer.transform.root.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
   
        }
    }

    void MoveAHPlayer(GameObject _ahPlayer, Vector3 moveTo)
    {
        AutoHandPlayer AhObj = _ahPlayer.GetComponentInChildren<AutoHandPlayer>();
        Vector3 resultingCoords = moveTo - AhObj.transform.localPosition;
        AutoHandPlayer.transform.position = resultingCoords;
        Debug.Log("To move to: " + moveTo);
        Debug.Log("AH with local pos: " + AhObj.transform.localPosition);
        Debug.Log("move to: " + resultingCoords);
    }

    public void SetRoles() 
    {
        if (pv.IsMine && playerRole != PlayerRole.Viewer)
        {
            if (playerRole == PlayerRole.OffGoing) 
            {
                for (int i = 0; i < playersList.showCase.Count; i++)
                {
                    playersList.showCase[i].SetActive(true);
                }
                checklistMechanic.Oncoming = false;
                MoveAHPlayer(AutoHandPlayer, OffGoingLocation);
            }
            if (playerRole == PlayerRole.OnComing)
            {
                for (int i = 0; i < playersList.showCase.Count; i++)
                {
                    playersList.showCase[i].SetActive(false);
                }
                checklistMechanic.Oncoming = true;
                MoveAHPlayer(AutoHandPlayer, OnComingLocation);
            }

            playersList.textRole.text = "Your role: " + playerRole + "\nYou can start";
            playersList.textRole.transform.parent.gameObject.SetActive(true);
        }
        if (!pv.IsMine)
        {
            if (playerRole == PlayerRole.OffGoing)
            {
                checklistMechanic.Oncoming = false;
                MoveAHPlayer(AutoHandPlayer, OffGoingLocation);
            }
            if (playerRole == PlayerRole.OnComing)
            {
                checklistMechanic.Oncoming = true;
                MoveAHPlayer(AutoHandPlayer, OnComingLocation);
            }
            grabbableTablet.enabled = false;
            grabbableTablet.GetComponent<SnapToHolster>().enabled = false;
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
