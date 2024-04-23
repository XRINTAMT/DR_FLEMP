using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand.Demo;
using Autohand;
using static XRMovementControls;

public class XRMovementControlsTutorial : MonoBehaviour
{
    //[SerializeField] XRHandPlayerControllerLink MovementControls;
    //[SerializeField] GameObject Teleport;
    //[SerializeField] Teleporter teleporterHand;
    //[SerializeField] XRTeleporterLink xRTeleporterLinkHand;
    //[SerializeField] AutoHandPlayer autoHandPlayer;

    void Start()
    {
        Teleporter[] array = FindObjectsOfType<Teleporter>(true);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].transform.parent.GetComponent<Hand>() && array[i].transform.parent.GetComponent<Hand>().left)
            {
                array[i].GetComponent<XRTeleporterLink>().role = UnityEngine.XR.XRNode.LeftHand;
            }
         
            if (array[i].transform.parent.GetComponent<Hand>() && !array[i].transform.parent.GetComponent<Hand>().left)
            {
                array[i].GetComponent<XRTeleporterLink>().role = UnityEngine.XR.XRNode.RightHand;
            }

            foreach (SpriteRenderer sr in array[i].gameObject.GetComponentsInChildren<SpriteRenderer>(true))
            {
                sr.gameObject.SetActive(true);
            }
        }
    }

    public void SwitchLocomotionTeleport()
    {
        FindObjectOfType<XRMovementControls>().SwitchLocomotion(0);
    }
    public void SwitchLocomotionMove()
    {
        FindObjectOfType<XRMovementControls>().SwitchLocomotion(1);
    }
    public void SwitchLocomotionMixed()
    {
        FindObjectOfType<XRMovementControls>().SwitchLocomotion(2);
    }
    //public void SwitchLocomotionHand()
    //{
    //    autoHandPlayer.teleporterL = teleporterHand;
    //    autoHandPlayer.xrTeleporterLinkLeft = xRTeleporterLinkHand;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
