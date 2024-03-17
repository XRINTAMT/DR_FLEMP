using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class ArTrigger : MonoBehaviour
{
    public PlacePoint placePoint;
    public Grabbable grabbablePlate;
    Grabbable grabbable;
    // Start is called before the first frame update
    void Start()
    {
        placePoint.OnHighlight.AddListener(OnHighlight);
        placePoint.OnStopHighlight.AddListener(OnUnhighlight);

        grabbablePlate.onRelease.AddListener(OnRelease);

    }
    void OnRelease(Hand hand, Grabbable grabbable) 
    {
        if (this.grabbable)
        {
            placePoint.Place(this.grabbable);
        }
    }
    void OnHighlight(PlacePoint placePoint, Grabbable grabbable)
    {
        this.grabbable = grabbable;
    }
    void OnUnhighlight(PlacePoint placePoint, Grabbable grabbable)
    {
        this.grabbable = null;
    }
   
}
