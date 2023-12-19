using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class HandLayerController : MonoBehaviour
{
    Grabbable grabbable;
    Hand handRight;
    Hand handLeft;
    bool grabRight,grabLeft;
    // Start is called before the first frame update
    void Awake()
    {
        handRight = FindObjectOfType<AutoHandPlayer>().handRight;
        handLeft = FindObjectOfType<AutoHandPlayer>().handLeft;
        grabbable = GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);
    }

    public void OnGrab(Hand hand, Grabbable grabbable) 
    {
        if (!hand.left)
            grabRight = true;
        if (hand.left)
            grabLeft = true;

        ChangeHandsLayer(26);

    }

    public void OnRelease(Hand hand, Grabbable grabbable)
    {
        if (!hand.left)
            grabRight = false;
        if (hand.left)
            grabLeft = false;

        if (!grabRight && !grabLeft)
            ChangeHandsLayer(29);
    }

    void ChangeHandsLayer(int layer) 
    {
        foreach (Renderer rend in handRight.GetComponentsInChildren<Renderer>(true))
        {
            rend.gameObject.layer = layer;
        }

        foreach (Renderer rend in handLeft.GetComponentsInChildren<Renderer>(true))
        {
            rend.gameObject.layer = layer;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
