using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using UnityEngine;

public class NewSyringeMechanic : MonoBehaviour
{
    [SerializeField] GameObject piston;
    //[SerializeField] CapsuleCollider innen;

    public GameObject bottle;
    ConfigurableJoint configurableJointPiston;
    Grabbable grabbableSyringe;
    Grabbable grabbablePiston;
    bool inBottle;
    bool updatePistonPos;
    public Canvas canvas;
    public bool syringeGrab;
    public Hand syringeHandGrab;
    public Hand bottleHandGrab;
    AutoHandPlayer autoHandPlayer;
    // Start is called before the first frame update
    void Start()
    {
        autoHandPlayer = FindObjectOfType<AutoHandPlayer>();

        grabbableSyringe = GetComponent<Grabbable>();
        grabbableSyringe.onGrab.AddListener(GrabSyringe);
        grabbableSyringe.onRelease.AddListener(ReleaseSyringe);

        grabbablePiston = piston.GetComponent<Grabbable>();
        grabbablePiston.onGrab.AddListener(UpdatePistonPos);
        grabbablePiston.onRelease.AddListener(StopUpdatePistonPos);
        configurableJointPiston = piston.GetComponent<ConfigurableJoint>();
    }

    void UpdatePistonPos(Hand hand, Grabbable grabbable)
    {
        updatePistonPos = true;
        piston.GetComponent<Rigidbody>().mass = 1500;
        piston.GetComponent<Rigidbody>().drag = 1500;
        piston.GetComponent<Rigidbody>().angularDrag = 1500;

        bottle.GetComponent<Rigidbody>().mass = 1000;
        bottle.GetComponent<Rigidbody>().drag = 1000;
        bottle.GetComponent<Rigidbody>().angularDrag = 1000;

        //if (grabbablePiston.GetComponent<FixedJoint>())
        //    Destroy(grabbablePiston.GetComponent<FixedJoint>());
    }
    void StopUpdatePistonPos(Hand hand, Grabbable grabbable)
    {
        updatePistonPos = false;

        if (!grabbablePiston.gameObject.GetComponent<FixedJoint>())
            grabbablePiston.gameObject.AddComponent<FixedJoint>();
        grabbablePiston.GetComponent<FixedJoint>().connectedBody = grabbableSyringe.GetComponent<Rigidbody>();
        grabbablePiston.GetComponent<FixedJoint>().breakForce = 600;
        piston.GetComponent<Rigidbody>().mass = 1;
        piston.GetComponent<Rigidbody>().drag = 0;
        piston.GetComponent<Rigidbody>().angularDrag = 0.05f;
        //bottle.GetComponent<Rigidbody>().isKinematic = false;

        bottle.GetComponent<Rigidbody>().mass = 1;
        bottle.GetComponent<Rigidbody>().drag = 0;
        bottle.GetComponent<Rigidbody>().angularDrag = 0.05f;
    }



    void GrabSyringe(Hand hand, Grabbable grabbable)
    {
        if (GetComponent<FixedJoint>())
            Destroy(GetComponent<FixedJoint>());

        if (GetComponent<ConfigurableJoint>())
            Destroy(GetComponent<ConfigurableJoint>());
        syringeHandGrab = hand;
        syringeGrab = true;
        grabbablePiston.enabled = false;
    }
    void ReleaseSyringe(Hand hand, Grabbable grabbable)
    {
        if (inBottle)
        {
            gameObject.AddComponent<FixedJoint>();
            GetComponent<FixedJoint>().connectedBody = bottle.GetComponent<Rigidbody>();
            transform.parent = bottle.transform;
            grabbablePiston.enabled = true;
        }
        if (!inBottle)
        {
            grabbablePiston.enabled = false;
        }
        syringeHandGrab.PlayHapticVibration(0);
        syringeHandGrab = null;
        syringeGrab = false;

    }


    public void Vibration(int time)
    {
        //hand.PlayHapticVibration(time);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AreaHaptic")
        {
            if (syringeHandGrab) syringeHandGrab.PlayHapticVibration(1000);
         
        }
        if (other.tag == "Indicate")
        {
            canvas.gameObject.SetActive(true);
            inBottle = true;
        }
        if (other.tag == "AreaLimit")
        {
            bottle = other.transform.parent.gameObject;

            if (autoHandPlayer.handLeft == syringeHandGrab)
            {
                bottleHandGrab = autoHandPlayer.handRight;
            }
            if (autoHandPlayer.handRight == syringeHandGrab)
            {
                bottleHandGrab = autoHandPlayer.handLeft;
            }
            if (bottleHandGrab)
            {
                bottleHandGrab.GetComponent<XRHandControllerLink>().enabled = false;
            }

            //innen.isTrigger = true;
            grabbableSyringe.GetComponent<Stabber>().enabled = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Indicate" || other.tag == "AreaLimit")
        {
            //grabbableSyringe.GetComponent<Stabber>().enabled = true;
            //innen.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AreaHaptic")
        {
            syringeHandGrab.PlayHapticVibration(0);
        }
        if (other.tag == "Indicate")
        {
            canvas.gameObject.SetActive(false);
            inBottle = false;
        }
        if (other.tag == "AreaLimit")
        {
            bottle = null;
            syringeHandGrab.PlayHapticVibration(0);
            if (bottleHandGrab)
            {
                bottleHandGrab.GetComponent<XRHandControllerLink>().enabled = true;
            }

            bottleHandGrab = null;


            //innen.isTrigger = true;
            grabbableSyringe.GetComponent<Stabber>().enabled = true;

            if (syringeGrab)
            {
                //innen.isTrigger = false;
            }

            if (GetComponent<ConfigurableJoint>())
            {
                Destroy(GetComponent<ConfigurableJoint>());
            }
            grabbableSyringe.GetComponent<Stabber>().enabled = false;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (updatePistonPos)
        {
            configurableJointPiston.targetPosition = new Vector3(piston.transform.localPosition.y / -20, 0, 0);
            //syringeHandGrab.PlayHapticVibration(1000);
        }

        //if (!syringeGrab && !innen.isTrigger)
        //{
        //    innen.isTrigger = true;
        //}
    }
}