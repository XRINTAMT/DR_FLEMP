using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorController : MonoBehaviour
{
    [SerializeField] GameObject sensor1;
    [SerializeField] GameObject sensor2;
    [SerializeField] GameObject sensor3;
    public UnityEvent putSensor;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void TakeSensor()
    {
        sensor1.SetActive(false);
        sensor2.SetActive(true);
    }

    public void PutSensor()
    {
        sensor2.SetActive(false);
        sensor3.SetActive(true);
        putSensor?.Invoke();
    }
}
// Update is called once per frame

