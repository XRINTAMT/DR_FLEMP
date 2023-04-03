using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using Photon.Pun;
using UnityEngine;

public class SynsFingersState : MonoBehaviour
{
    PhotonView pv;
    AutoHandPlayer autoHandPlayer;
    Hand handRight;
    Hand handLeft;
    public List<Finger> playerFingersRight = new List<Finger>();
    public List<Finger> playerFingersLeft = new List<Finger>();
    public List<Finger> multiplayerFingersRight = new List<Finger>();
    public List<Finger> multiplayerFingersLeft = new List<Finger>();
    XRControllerEvent gripLeft;
    XRControllerEvent triggerLeft;
    XRControllerEvent axisLeft;
    XRControllerEvent gripRight;
    XRControllerEvent triggerRight;
    XRControllerEvent axisRight;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            handRight = FindObjectOfType<AutoHandPlayer>().handRight;
            handLeft = FindObjectOfType<AutoHandPlayer>().handLeft;

            for (int i = 0; i < handRight.fingers.Length; i++)
            {
                playerFingersRight.Add(handRight.fingers[i]);
            }
            for (int i = 0; i < handLeft.fingers.Length; i++)
            {
                playerFingersLeft.Add(handLeft.fingers[i]);
            }

        }
        //for left hand
        gripLeft = new XRControllerEvent();
        gripLeft.link = handLeft.GetComponent<XRHandControllerLink>();
        gripLeft.button = CommonButton.gripButton;
        gripLeft.Pressed.AddListener(() => SetFingerStateLeftHand());
        gripLeft.Released.AddListener(() => SetFingerStateLeftHand());

        triggerLeft = new XRControllerEvent();
        triggerLeft.link = handRight.GetComponent<XRHandControllerLink>();
        triggerLeft.button = CommonButton.triggerButton;
        triggerLeft.Pressed.AddListener(() => SetFingerStateLeftHand());
        triggerLeft.Released.AddListener(() => SetFingerStateLeftHand());

        axisLeft = new XRControllerEvent();
        axisLeft.link = handRight.GetComponent<XRHandControllerLink>();
        axisLeft.button = CommonButton.primary2DAxisTouch;
        axisLeft.Pressed.AddListener(() => SetFingerStateLeftHand());
        axisLeft.Released.AddListener(() => SetFingerStateLeftHand());

        ////for right hand
        gripRight = new XRControllerEvent();
        gripRight.link = handRight.GetComponent<XRHandControllerLink>();
        gripRight.button = CommonButton.gripButton;
        gripRight.Pressed.AddListener(() => SetFingerStateRightHand());
        gripRight.Released.AddListener(() => SetFingerStateRightHand());

        triggerRight = new XRControllerEvent();
        triggerRight.link = handRight.GetComponent<XRHandControllerLink>();
        triggerRight.button = CommonButton.triggerButton;
        triggerRight.Pressed.AddListener(() => SetFingerStateRightHand());
        triggerRight.Released.AddListener(() => SetFingerStateRightHand());

        axisRight = new XRControllerEvent();
        axisRight.link = handRight.GetComponent<XRHandControllerLink>();
        axisRight.button = CommonButton.primary2DAxisTouch;
        axisRight.Pressed.AddListener(() => SetFingerStateRightHand());
        axisRight.Released.AddListener(() => SetFingerStateRightHand());
    }


    IEnumerator SetFingerStateLeftHand()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < playerFingersLeft.Count; i++)
        {
            multiplayerFingersLeft[i].bendOffset = playerFingersLeft[i].bendOffset;
        }
        pv.RPC("SetFingerStateLeftHandRPC", RpcTarget.All, pv.ViewID, multiplayerFingersLeft[0].bendOffset, multiplayerFingersLeft[1].bendOffset, multiplayerFingersLeft[2].bendOffset, multiplayerFingersLeft[3].bendOffset, multiplayerFingersLeft[4].bendOffset);
    }

    [PunRPC]
    void SetFingerStateLeftHandRPC(int viewId, float finger0, float finger1,float finger2, float finger3, float finger4)
    {
        if (viewId==pv.GetComponent<PhotonView>().ViewID)
        {
            multiplayerFingersLeft[0].bendOffset = finger0;
            multiplayerFingersLeft[1].bendOffset = finger1;
            multiplayerFingersLeft[2].bendOffset = finger2;
            multiplayerFingersLeft[3].bendOffset = finger3;
            multiplayerFingersLeft[4].bendOffset = finger4;
        }
    }

    IEnumerator SetFingerStateRightHand()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < playerFingersRight.Count; i++)
        {
            multiplayerFingersRight[i].bendOffset = playerFingersRight[i].bendOffset;
        }
        pv.RPC("SetFingerStateRightHandRPC", RpcTarget.All, pv.ViewID, multiplayerFingersLeft[0].bendOffset, multiplayerFingersLeft[1].bendOffset, multiplayerFingersLeft[2].bendOffset, multiplayerFingersLeft[3].bendOffset, multiplayerFingersLeft[4].bendOffset);

    }

    [PunRPC]
    void SetFingerStateRightHandRPC(int viewId, float finger0, float finger1, float finger2, float finger3, float finger4)
    {
        if (viewId == pv.GetComponent<PhotonView>().ViewID)
        {
            multiplayerFingersRight[0].bendOffset = finger0;
            multiplayerFingersRight[1].bendOffset = finger1;
            multiplayerFingersRight[2].bendOffset = finger2;
            multiplayerFingersRight[3].bendOffset = finger3;
            multiplayerFingersRight[4].bendOffset = finger4;
        }
    }
    void Update()
    {
       
    }
}
