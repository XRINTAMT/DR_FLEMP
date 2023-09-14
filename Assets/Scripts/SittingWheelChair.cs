using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class SittingWheelChair : MonoBehaviour
{
    public Transform siitingPos;
    public HeadCharacterFollow headCharacterFollow;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var headFollower in GetComponentsInChildren<HeadCharacterFollow>())
        {
            headCharacterFollow = headFollower;
        }

    }
  
    public void Sit() 
    {
        if (headCharacterFollow.inArea)
        {
            transform.parent = siitingPos;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }
    }
    // Update is called once per frame

}
