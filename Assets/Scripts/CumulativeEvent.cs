using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CumulativeEvent : MonoBehaviour
{
    [SerializeField] int CallsNeeded;
    [SerializeField] UnityEvent OnAccumulated;

    public void Call()
    {
        CallsNeeded -= 1;
        if(CallsNeeded == 0)
        {
            OnAccumulated.Invoke();
        }
    }
}
