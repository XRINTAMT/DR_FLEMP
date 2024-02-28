using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIndicatorColor : MonoBehaviour
{
    [SerializeField] Image indicator;
    bool activate;
    ShowcaseController showcaseController;
    // Start is called before the first frame update
    void Start()
    {
        showcaseController = FindObjectOfType<ShowcaseController>();
        indicator.color = Color.red;
    }

    public void Enter() 
    {
        indicator.color = Color.green;
    }
    public void Press()
    {
        activate = true;
        showcaseController.DisableAllCanvas();
        indicator.enabled = false;
    }
    public void Exit() 
    {
        indicator.enabled = true;
        if (activate)
            indicator.color = Color.grey;

        if (!activate)
            indicator.color = Color.red;

    }
    
}
