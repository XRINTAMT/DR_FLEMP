using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingWheelChair : MonoBehaviour
{
    public Transform siitingPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Sit() 
    {
        transform.parent = siitingPos;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
