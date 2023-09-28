using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
using ScenarioTaskSystem;

public class SittingWheelChair : MonoBehaviour
{
    public Transform siitingPos;
    HeadCharacterFollow headCharacterFollow;

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
            Task _wheelchairTask;
            if (siitingPos.TryGetComponent<Task>(out _wheelchairTask)){
                _wheelchairTask.Complete();
            }
        }
    }
    // Update is called once per frame

}
