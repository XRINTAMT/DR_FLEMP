using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand.Demo;
using Autohand;
using UnityEngine.SceneManagement;

public class XRMovementControls : MonoBehaviour
{
    [SerializeField] XRHandPlayerControllerLink MovementControls;
    [SerializeField] AutoHandPlayer AHPlayer;
    [SerializeField] GameObject TeleportRight;
    [SerializeField] GameObject TeleportLeft;
    [SerializeField] XRControllerEvent rightController;
    [SerializeField] XRControllerEvent leftController;
    [SerializeField] GameObject nurseTablet;
    [SerializeField] GameObject hips;
    [SerializeField] Vector3 positionRight;
    [SerializeField] Vector3 positionLeft;
    [SerializeField] Vector3 rotationRight;
    [SerializeField] Vector3 rotationLeft;
    public static TypeMovement movementType;
    public static HandMovement movementHand;
    bool teleportEnable;
    bool startTeleportation;
    bool pressAxis;
    

    void Awake()
    {
        SwitchLocomotion(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "MovementType", 0));
        AHPlayer.maxMoveSpeed = PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "walkingSpeed", 2);
        if (SceneManager.GetActiveScene().name=="Lobby")
            SwitchLocomotion(2);
    }

    void SetNurseTablet() 
    {
        if (nurseTablet)
        {
            if (movementHand  == HandMovement.Left)
            {
                hips.transform.localScale = new Vector3(1, 1, 1);
                //nurseTablet.transform.localPosition = positionLeft;
                //nurseTablet.transform.localEulerAngles = rotationLeft;
            }

            if (movementHand == HandMovement.Right)
            {
                hips.transform.localScale = new Vector3(-1, -1, 1);
                //nurseTablet.transform.localPosition = positionRight;
                //nurseTablet.transform.localEulerAngles = rotationRight;
            }
        }
    
    }
    public void SwitchLocomotion(int type)
    {
        switch (type)
        {
            case (0):
                movementType = TypeMovement.Teleport;
                if (movementHand == HandMovement.Left)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.none;
                    MovementControls.turnAxis = Common2DAxis.none;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(true);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = true;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = true;

                    rightController.enabled = false;
                    leftController.enabled = true;
                    startTeleportation = true;

                    SetNurseTablet();
                }
                if (movementHand == HandMovement.Right)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.none;
                    MovementControls.turnAxis = Common2DAxis.none;

                    TeleportRight.SetActive(true);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = true;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = true;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = true;
                    leftController.enabled = false;
                    startTeleportation = true;

                    SetNurseTablet();
                }
                break;

            case (1):
                movementType = TypeMovement.Move;
                if (movementHand == HandMovement.Left)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                if (movementHand == HandMovement.Right)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                break;

            case (2):
                movementType = TypeMovement.Mixed;

                if (movementHand == HandMovement.Left)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(true);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = true;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = true;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                if (movementHand == HandMovement.Right)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(true);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = true;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = true;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                break;
        }

    }
    public void SwitchMovementHand(int type)
    {
        switch (type)
        {

            case (1):
                movementHand = HandMovement.Right;

                if (movementType == TypeMovement.Teleport)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.none;
                    MovementControls.turnAxis = Common2DAxis.none;

                    TeleportRight.SetActive(true);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = true;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = true;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;


                    rightController.enabled = true;
                    leftController.enabled = false;
                    startTeleportation = true;

                    SetNurseTablet();
                }
                if (movementType == TypeMovement.Move)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                if (movementType == TypeMovement.Mixed)
                {
                    MovementControls.moveController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(true);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = true;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = true;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }

                break;
            case (0):
                movementHand = HandMovement.Left;

                if (movementType == TypeMovement.Teleport)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.none;
                    MovementControls.turnAxis = Common2DAxis.none;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(true);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = true;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = true;

                    rightController.enabled = false;
                    leftController.enabled = true;
                    startTeleportation = true;

                    SetNurseTablet();
                }
                if (movementType == TypeMovement.Move)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(false);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = false;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }
                if (movementType == TypeMovement.Mixed)
                {
                    MovementControls.moveController = AHPlayer.handLeft.GetComponent<XRHandControllerLink>();
                    MovementControls.turnController = AHPlayer.handRight.GetComponent<XRHandControllerLink>();

                    MovementControls.moveAxis = Common2DAxis.primaryAxis;
                    MovementControls.turnAxis = Common2DAxis.primaryAxis;

                    TeleportRight.SetActive(true);
                    TeleportLeft.SetActive(false);

                    TeleportRight.GetComponent<Teleporter>().enabled = true;
                    TeleportLeft.GetComponent<Teleporter>().enabled = false;

                    TeleportRight.GetComponent<XRTeleporterLink>().enabled = true;
                    TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;

                    rightController.enabled = false;
                    leftController.enabled = false;
                    startTeleportation = false;

                    SetNurseTablet();
                }

                break;
           
        }

    }

    public void PressAxis() 
    {
        pressAxis = true;

    }
    public void UnpressAxis()
    {
        pressAxis = false;
    }
    public void TeleportationMovement() 
    {
        if (MovementControls.turnAxis == Common2DAxis.none)
            MovementControls.turnAxis = Common2DAxis.primaryAxis;

        if (movementHand == HandMovement.Left)
        {
            //AHPlayer.GetComponent<Rigidbody>().isKinematic = true;
            //if (moveDirection.x > 0 || moveDirection.x < 0) moveDirection.x = 0;
            //if (moveDirection.z > 0 || moveDirection.z < 0) moveDirection.z = 0;
            //if (moveDirection.y > 0 || moveDirection.y < 0) moveDirection.y = 0;

            //AHPlayer.GetComponent<Rigidbody>().isKinematic = false;

            if (TeleportLeft.GetComponent<XRTeleporterLink>().enabled)
                TeleportLeft.GetComponent<XRTeleporterLink>().enabled = false;
            if (pressAxis && !teleportEnable)
            {
                TeleportLeft.GetComponent<Teleporter>().StartTeleport();
                teleportEnable = true;
            }
            if (!pressAxis && teleportEnable)
            {
                TeleportLeft.GetComponent<Teleporter>().Teleport();
                teleportEnable = false;
            }
        }
        if (movementHand == HandMovement.Right)
        {
            //AHPlayer.GetComponent<Rigidbody>().isKinematic = true;
            //if (moveDirection.x > 0 || moveDirection.x < 0) moveDirection.x = 0;
            //if (moveDirection.z > 0 || moveDirection.z < 0) moveDirection.z = 0;
            //if (moveDirection.y > 0 || moveDirection.y < 0) moveDirection.y = 0;

            //AHPlayer.GetComponent<Rigidbody>().isKinematic = false;

            if (TeleportRight.GetComponent<XRTeleporterLink>().enabled)
                TeleportRight.GetComponent<XRTeleporterLink>().enabled = false;
            if (pressAxis && !teleportEnable)
            {
                TeleportRight.GetComponent<Teleporter>().StartTeleport();
                teleportEnable = true;
            }
            if (!pressAxis && teleportEnable)
            {
                TeleportRight.GetComponent<Teleporter>().Teleport();
                teleportEnable = false;
            }
        }

    }
    public void SetMovementSpeed(float speed)
    {
        AHPlayer.maxMoveSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTeleportation) 
        {
            TeleportationMovement();
        }
    }

    public enum TypeMovement
    {
        Teleport,
        Move,
        Mixed
    }
    public enum HandMovement
    {
        Left,
        Right,
    }

}
