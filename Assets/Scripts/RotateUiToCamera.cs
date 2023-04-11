using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUiToCamera : MonoBehaviour
{

    [SerializeField] private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        transform.LookAt(targetPosition);
    }
}
