using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class ARactivate : MonoBehaviour
{
    public GameObject arCamera;
    public GameObject[] obj;
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
            arCamera.transform.parent = autoHandPlayer.trackingContainer;
            arCamera.transform.localPosition = Vector3.zero;
            arCamera.transform.localEulerAngles = Vector3.zero;
            arCamera.SetActive(true);
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = false;
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AutoHandPlayer>())
        {
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = true;
            arCamera.SetActive(false);
            arCamera.transform.parent = transform;
            arCamera.transform.localPosition = Vector3.zero;
            arCamera.transform.localEulerAngles = Vector3.zero;

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(true);
            }
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
            arCamera.transform.parent = autoHandPlayer.trackingContainer;
            arCamera.SetActive(state);
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = !state;

        }
        if (!state)
        {
            autoHandPlayer.headCamera.GetComponent<Camera>().enabled = !state;
            arCamera.SetActive(state);
            arCamera.transform.parent = transform;
        }
        arCamera.transform.localPosition = Vector3.zero;
        arCamera.transform.localEulerAngles = Vector3.zero;
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(!state);
        }
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
}
