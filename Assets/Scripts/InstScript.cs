using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using UnityEngine;

public class InstScript : MonoBehaviour
{
    [SerializeField] XRHandControllerLink controllerLink;
    [SerializeField] Collider iPad;
    [SerializeField] float rotationSpeed;
    [SerializeField] public float angle;
    public GameObject arTable;
    public GameObject pointLeft, pointRight;
    private Vector3 instObjectPosition;
    Hand handRight;
    Hand handLeft;
    bool editMode;

    void Start()
    {
        pointRight.GetComponent<Grabbable>().onGrab.AddListener(SetHand);
        pointLeft.GetComponent<Grabbable>().onGrab.AddListener(SetHand);
        pointRight.GetComponent<Grabbable>().onRelease.AddListener(ReleaseHand);
        pointLeft.GetComponent<Grabbable>().onRelease.AddListener(ReleaseHand);
        Physics.IgnoreCollision(arTable.GetComponent<Collider>(), iPad, true);
        TableEditMode(true);
    }

    public void TableEditMode(bool state) 
    {
        if (arTable) arTable.GetComponent<Collider>().enabled = !state;
        editMode = state;
        pointRight.gameObject.SetActive(state);
        pointLeft.gameObject.SetActive(state);
    }
  
    public void SetHand(Hand hand, Grabbable grabbable) 
    {
        if (!hand.left) 
        {
            handRight = hand;
            pointLeft.GetComponent<Grabbable>().enabled = false;
        }
        
        if (hand.left) 
        {
            handLeft = hand;
            pointRight.GetComponent<Grabbable>().enabled = false;
        }

    }
    public void ReleaseHand(Hand hand, Grabbable grabbable)
    {
        if (!hand.left) 
        {
            handRight = null;
        }

        if (hand.left) 
        {
            handLeft = null;
        }

        pointRight.GetComponent<Grabbable>().enabled = true;
        pointLeft.GetComponent<Grabbable>().enabled = true;
    }

    private void UpdateScale()
    {
        Vector3 direction = pointRight.transform.position - pointLeft.transform.position;
        Vector3 newPointRightPosition = pointLeft.transform.position + (Quaternion.Euler(0, angle, 0) * direction);

        float sideX = Mathf.Abs(newPointRightPosition.x - pointLeft.transform.position.x);
        float sideZ = Mathf.Abs(newPointRightPosition.z - pointLeft.transform.position.z);
        instObjectPosition = (pointLeft.transform.position + newPointRightPosition) / 2;

        Quaternion tableRotation = Quaternion.Euler(0, -angle, 0);

        if (arTable.GetComponent<Collider>().enabled) arTable.GetComponent<Collider>().enabled = false;

        arTable.transform.localPosition = (pointLeft.transform.localPosition + pointRight.transform.localPosition) / 2f;
        arTable.transform.localScale = new Vector3(sideX, 0.04f, sideZ);
        arTable.transform.rotation = tableRotation;
    }

    private void UpdateCorners()
    {
        if (handRight)
        {
            pointLeft.transform.position = new Vector3(pointLeft.transform.position.x, pointRight.transform.position.y/* + sideY*/, pointLeft.transform.position.z);
        }

        if (handLeft)
        {
            pointRight.transform.position = new Vector3(pointRight.transform.position.x, pointLeft.transform.position.y/* + sideY*/, pointRight.transform.position.z);
        }
    }

    void Update()
    {
        if (editMode)
        {
            if (controllerLink != null)
            {
                Vector2 inputVector = controllerLink.GetAxis2D(Common2DAxis.primaryAxis);
                float xInput = inputVector.x;
                if (Mathf.Abs(xInput) > 0.1f)
                {
                    angle += xInput * rotationSpeed * Time.deltaTime;
                }
            }
            UpdateCorners();
            UpdateScale();
        }
    }
}