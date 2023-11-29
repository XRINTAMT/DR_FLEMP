using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
using ScenarioTaskSystem;

public class SittingWheelChair : MonoBehaviour
{
    public Transform siitingPos;
    public HeadCharacterFollow headCharacterFollow;
    [field:SerializeField] public bool RightPatient { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var headFollower in GetComponentsInChildren<HeadCharacterFollow>())
        {
            headCharacterFollow = headFollower;
        }

    }
  
    public string Sit() 
    {
        if (headCharacterFollow.inArea)
        {
            float dist = Vector3.Distance(this.transform.position, siitingPos.position);
            if (dist > 2.0)
            {
                return "Sit in Wheelchair Fail: Wheelchair is too far away";
            }
            transform.parent = siitingPos;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            Rigidbody _wheelchairRB = siitingPos.parent.GetComponent<Rigidbody>();
            if(_wheelchairRB != null)
            {
                _wheelchairRB.mass = 1100;
            }
            Task _wheelchairTask;
            if (siitingPos.TryGetComponent<Task>(out _wheelchairTask)){
                _wheelchairTask.Complete(RightPatient ? 1 : 0);
                return "Sit in Wheelchair Success";
            }
        }
        return "Sit in Wheelchair Fail: Which character are you talking to?";
    }

    public void SitFromButton()
    {
        transform.parent = siitingPos;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        Rigidbody _wheelchairRB = siitingPos.parent.GetComponent<Rigidbody>();
        if (_wheelchairRB != null)
        {
            _wheelchairRB.mass = 1100;
        }
        Task _wheelchairTask;
        if (siitingPos.TryGetComponent<Task>(out _wheelchairTask))
        {
            _wheelchairTask.Complete(RightPatient ? 1 : 0);
        }
    }
    // Update is called once per frame

}
