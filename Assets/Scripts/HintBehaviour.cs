using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class HintBehaviour : MonoBehaviour
{
    public static bool startFirstTime;
    private void Awake()
    {
        if (!startFirstTime)
        {
            HintControllers [] hintControllers = FindObjectsOfType<HintControllers>();
            for (int i = 0; i < hintControllers.Length; i++)
            {
                hintControllers[i].EnableHint();
            }
            startFirstTime = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startFirstTime)
        {
            GetComponent<PlacePoint>().enabled = false;
        }
    }


}
