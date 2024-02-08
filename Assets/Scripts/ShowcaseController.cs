using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowcaseController : MonoBehaviour
{
    public CanvasGroup[] canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DisableAllCanvas() 
    {
        for (int i = 0; i < canvasGroup.Length; i++)
        {
            canvasGroup[i].alpha = 0;
            canvasGroup[i].GetComponent<GraphicRaycaster>().enabled = false;
        }
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
