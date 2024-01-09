using UnityEngine;
using UnityEngine.Events;

public class LongButtonPress : MonoBehaviour
{
    public UnityEvent OnHeldForLongEnough;
    public float Timeout = 2.0f;

    private bool buttonPressed;
    private float pressTime;

    private void Update()
    {
        if (buttonPressed)
        {
            float heldDuration = Time.time - pressTime;
            if (heldDuration >= Timeout)
            {
                OnHeldForLongEnough.Invoke();
            }
        }
    }

    public void Pressed()
    {
        buttonPressed = true;
        pressTime = Time.time;
    }

    public void Released()
    {
        buttonPressed = false;
    }
}