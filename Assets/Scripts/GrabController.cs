using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    Grabbable grabbable;
    public Grabbable nurseTablet;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(OnGrab);
    }

    void OnGrab(Hand hand, Grabbable grabbable) 
    {
        hand.TryGrab(nurseTablet);
    }
    // Update is called once per frame
  
}
