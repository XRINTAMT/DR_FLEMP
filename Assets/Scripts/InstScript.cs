using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
public class InstScript : MonoBehaviour
{
    //public GameObject pointPrefab;
    public Collider iPad;
    public GameObject pointLeft, pointRight, instObject;
    private Vector3 instObjectPosition;
    private float instObjectScale;
    private GameObject inst;
    public Hand handRight;
    public Hand handLeft;
    bool editMode;

    // Use this for initialization
    void Start()
    {
        pointRight.GetComponent<Grabbable>().onGrab.AddListener(SetHand);
        pointLeft.GetComponent<Grabbable>().onGrab.AddListener(SetHand);
        pointRight.GetComponent<Grabbable>().onRelease.AddListener(ReleaseHand);
        pointLeft.GetComponent<Grabbable>().onRelease.AddListener(ReleaseHand);
    }

    public void TableEditMode(bool state) 
    {
        if (inst) inst.GetComponent<Collider>().enabled = !state;
        editMode = state;
        pointRight.gameObject.SetActive(state);
        pointLeft.gameObject.SetActive(state);
    }
  
    //public void SpawnPoints() 
    //{
    //    pointRight = Instantiate(pointPrefab, handRight.transform.position, Quaternion.identity);
    //    pointLeft = Instantiate(pointPrefab, handLeft.transform.position, Quaternion.identity);
    //}
    //public void CLosePoints()
    //{
    //    if (pointLeft && pointRight)
    //    {
    //        Destroy(pointRight);
    //        Destroy(pointLeft);
    //        inst.GetComponent<Collider>().enabled = true;

    //        Physics.IgnoreCollision(inst.GetComponent<Collider>(), iPad, true);
    //        return;
    //    }
    //}
    public void SetHand(Hand hand, Grabbable grabbable) 
    {
        if (!hand.left) 
        {
            handRight = hand;
            pointLeft.GetComponent<Grabbable>().enabled = false;
            //pointLeft.transform.parent = pointRight.transform;
            //pointRight.GetComponent<Renderer>().enabled = true;
        }
        
        if (hand.left) 
        {
            handLeft = hand;
            pointRight.GetComponent<Grabbable>().enabled = false;
            //pointRight.transform.parent = pointLeft.transform;
            //pointLeft.GetComponent<Renderer>().enabled = true;
        }
        //if (handRight && handLeft)
        //{
        //    if (pointLeft && pointRight)
        //    {
        //        Destroy(pointRight);
        //        Destroy(pointLeft);
        //        return;
        //    }
        //    if (!pointLeft && !pointRight)
        //    {
        //        SpawnPoints();
        //        return;
        //    }
        //}

    }
    public void ReleaseHand(Hand hand, Grabbable grabbable)
    {
        if (!hand.left) 
        {
            handRight = null;
            //pointRight.GetComponent<Renderer>().enabled = false;
        }

        if (hand.left) 
        {
            handLeft = null;
            //pointLeft.GetComponent<Renderer>().enabled = false;
        }

        pointRight.GetComponent<Grabbable>().enabled = true;
        pointLeft.GetComponent<Grabbable>().enabled = true;

        //pointRight.transform.parent = transform;
        //pointLeft.transform.parent = transform;
        //if (!handRight && !handLeft)
        //{
        //    inst.GetComponent<Collider>().enabled = true;

        //    Physics.IgnoreCollision(inst.GetComponent<Collider>(),iPad, true);
        //}

    }
    void UpdateScale() 
    {
        float distance = Vector2.Distance(pointLeft.transform.position, pointRight.transform.position);

        float sideX = Mathf.Abs(pointRight.transform.position.x - pointLeft.transform.position.x);
        float sideY = Mathf.Abs(pointRight.transform.position.y - pointLeft.transform.position.y);
        float sideZ = Mathf.Abs(pointRight.transform.position.z - pointLeft.transform.position.z);

        if (handRight)
        {
            pointLeft.transform.position = new Vector3(pointLeft.transform.position.x, pointRight.transform.position.y/* + sideY*/, pointLeft.transform.position.z);
        }

        if (handLeft)
        {
            pointRight.transform.position = new Vector3(pointRight.transform.position.x, pointLeft.transform.position.y/* + sideY*/, pointRight.transform.position.z);
        }

        //pointRight.transform.position = pointLeft.transform.position;
        //instObjectPosition = new Vector3((point1.transform.position.x + point2.transform.position.x) / 2, (point2.transform.position.y + ground.transform.position.y) / 2, (point1.transform.position.z + point2.transform.position.z) / 2);
        instObjectPosition = new Vector3((pointLeft.transform.position.x + pointRight.transform.position.x) / 2, (pointLeft.transform.position.y + pointRight.transform.position.y) / 2, (pointLeft.transform.position.z + pointRight.transform.position.z) / 2);

        //instObjectScale = Mathf.Sqrt(
        //    (pointRight.transform.position.x - pointRight.transform.position.x) * (pointRight.transform.position.x - pointRight.transform.position.x) +
        //    (pointRight.transform.position.y - ground.transform.position.y) * (pointRight.transform.position.y - ground.transform.position.y) +
        //    (pointRight.transform.position.z - pointRight.transform.position.z) * (pointRight.transform.position.z - pointRight.transform.position.z)
        //);

        if (inst == null)
        {
            inst = Instantiate(instObject);
            Physics.IgnoreCollision(inst.GetComponent<Collider>(), iPad, true);
        }
        if (inst.GetComponent<Collider>().enabled) inst.GetComponent<Collider>().enabled = false;
      
        inst.transform.position = instObjectPosition;
        //inst.transform.localScale = new Vector3(sideA, instObjectScale, sideB);
        //inst.transform.localScale = new Vector3(sideX, sideY, sideZ);
        inst.transform.localScale = new Vector3(sideX, 0.1f, sideZ);

    }
    // Update is called once per frame
    void Update()
    {
        if (editMode)
        {
            //pointLeft.transform.position = handLeft.transform.position;
            //pointRight.transform.position = handRight.transform.position;

            UpdateScale();
        }


       
    }
}