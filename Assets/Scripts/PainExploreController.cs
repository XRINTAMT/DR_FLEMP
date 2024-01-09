using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
using UnityEngine.UI;

public class PainExploreController : MonoBehaviour
{
    [SerializeField] GameObject panelEnd;
    [SerializeField] Grabbable pain1;
    [SerializeField] Grabbable pain2;
    [SerializeField] Grabbable pain3;
    bool inspectedPain1;
    bool inspectedPain2;
    bool inspectedPain3;
    Grabbable currentPain=null;
    // Start is called before the first frame update
    void Start()
    {
        //pain1.onGrab.AddListener(InspectedPain);
        //pain2.onGrab.AddListener(InspectedPain);
        //pain3.onGrab.AddListener(InspectedPain);

        pain1.onHighlight.AddListener(SetCurrentPain);
        pain2.onHighlight.AddListener(SetCurrentPain);
        pain3.onHighlight.AddListener(SetCurrentPain);

        pain1.onUnhighlight.AddListener(SetCurrentPainNull);
        pain2.onUnhighlight.AddListener(SetCurrentPainNull);
        pain3.onUnhighlight.AddListener(SetCurrentPainNull);
    }

    void SetCurrentPain(Hand hand, Grabbable grabbable)
    {
        currentPain = grabbable;
    }
    void SetCurrentPainNull(Hand hand, Grabbable grabbable)
    {
        currentPain = null;
    }
    public void InspectedPain() 
    {
        if (currentPain == null)
            return;

        if (currentPain == pain1) 
        {
            inspectedPain1 = true;
            pain1.gameObject.SetActive(false);
        }


        if (currentPain == pain2)
        {
            inspectedPain2 = true;
            pain2.gameObject.SetActive(false);
        }

        if (currentPain == pain3) 
        {
            inspectedPain3 = true;
            pain3.gameObject.SetActive(false);
        }
           

        if (inspectedPain1 && inspectedPain2 && inspectedPain3)
        {
            panelEnd.SetActive(true);
        }
        //switch (index)
        //{
        //    case 1:
        //        inspectedPain1 = true;
        //        break;
        //    case 2:
        //        inspectedPain2 = true;
        //        break;
        //    case 3:
        //        inspectedPain3 = true;
        //        break;
        //    default:
        //        break;
        //}


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
