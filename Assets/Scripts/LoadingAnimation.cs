using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private float rotationPeriod = 1f;
    [SerializeField] private float degreesPerTurn = 45f;
    private float turnPeriod;

    private float timeElapsed;

    private void Start()
    {
        turnPeriod = Mathf.Abs(rotationPeriod / (360 / degreesPerTurn));
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= turnPeriod)
        {
            transform.Rotate(Vector3.back, degreesPerTurn); // Rotate around the y-axis by the specified degrees
            timeElapsed = 0f;
        }
    }
}
