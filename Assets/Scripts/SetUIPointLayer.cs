using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIPointLayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIPointRight;
    public GameObject UIPointLeft;
    void Start()
    {
        Invoke("SetUIPoint", 1);
    }
    void SetUIPoint()
    {
        UIPointRight.layer = 26;
        UIPointLeft.layer = 26;
        foreach (Renderer rend in UIPointRight.GetComponentsInChildren<Renderer>(true))
        {
            rend.gameObject.layer = 26;
        }

        foreach (Renderer rend in UIPointLeft.GetComponentsInChildren<Renderer>(true))
        {
            rend.gameObject.layer = 26;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
