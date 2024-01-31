using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class ARactivate : MonoBehaviour
{
    public GameObject arCamera;
    public GameObject[] objHide;
    public GameObject[] objAppear;
    AutoHandPlayer autoHandPlayer;
    bool state;
    // Start is called before the first frame update
    void Start()
    {
        autoHandPlayer = FindObjectOfType<AutoHandPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AutoHandPlayer>())
        {
            state = true;
            ViewAR();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AutoHandPlayer>())
        {
            state = false;
            ViewAR();
        }
    }

    public void PressButtonAR() 
    {
        state = !state;
        ViewAR();
    }
    public void ViewAR()
    {
        if (state)
        {
            //arCamera.transform.parent = autoHandPlayer.trackingContainer;
            arCamera.SetActive(state);
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = !state;

        }
        if (!state)
        {
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = !state;
            arCamera.SetActive(state);
            //arCamera.transform.parent = transform;
        }
        //arCamera.transform.localPosition = Vector3.zero;
        //arCamera.transform.localEulerAngles = Vector3.zero;
        for (int i = 0; i < objHide.Length; i++)
            objHide[i].SetActive(!state);
        for (int i = 0; i < objAppear.Length; i++)
            objAppear[i].SetActive(state);
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
}
