using System;
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
    [SerializeField] GameObject painInsect1;
    [SerializeField] GameObject painInsect2;
    [SerializeField] GameObject painInsect3;
    [SerializeField] Slider sliderPain1;
    [SerializeField] Slider sliderPain2;
    [SerializeField] Slider sliderPain3;
    [SerializeField] Grabbable currentPain = null;
    [SerializeField] bool inspectedPain1;
    [SerializeField] bool inspectedPain2;
    [SerializeField] bool inspectedPain3;
  
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

        sliderPain1.onValueChanged.AddListener(ChangeValue);
        sliderPain2.onValueChanged.AddListener(ChangeValue);
        sliderPain3.onValueChanged.AddListener(ChangeValue);
    }

    void SetCurrentPain(Hand hand, Grabbable grabbable)
    {
        currentPain = grabbable;
    }
    public void SetCurrentPain(Grabbable grabbable)
    {
        currentPain = grabbable;
    }

 
    void ChangeValue(float value) 
    {
        if (currentPain == null)
            return;
        currentPain.GetComponent<PainIndicator>().painCount=((int)value);
    }
    void SetCurrentPainNull(Hand hand, Grabbable grabbable)
    {
        currentPain = null;
    }
    public void InsectPain1() 
    {
        inspectedPain1 = true;
        if (inspectedPain1 && inspectedPain2 && inspectedPain3)
        {
            panelEnd.SetActive(true);
        }

    }
    public void InsectPain2()
    {
        inspectedPain2 = true;
        if (inspectedPain1 && inspectedPain2 && inspectedPain3)
        {
            panelEnd.SetActive(true);
        }
    }
    public void InsectPain3()
    {
        inspectedPain3 = true;
        if (inspectedPain1 && inspectedPain2 && inspectedPain3)
        {
            panelEnd.SetActive(true);
        }
    }
    public void InspectedPain() 
    {
        if (currentPain == null)
            return;

        if (currentPain == pain1) 
        {
            inspectedPain1 = true;
            painInsect1.SetActive(true);
        }

        if (currentPain == pain2)
        {
            inspectedPain2 = true;
            painInsect2.SetActive(true);
        }

        if (currentPain == pain3) 
        {
            inspectedPain3 = true;
            painInsect3.SetActive(true);
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
