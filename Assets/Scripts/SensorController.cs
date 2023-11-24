using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorController : MonoBehaviour
{
    [SerializeField] GameObject sensor1;
    [SerializeField] GameObject sensor2;
    [SerializeField] GameObject sensor3;
    [SerializeField] GameObject thermometer1;
    [SerializeField] GameObject thermometer2;
    //[SerializeField] GameObject thermometer3;
    [SerializeField] GameObject bloodPressure1;
    [SerializeField] GameObject bloodPressure2;
    [SerializeField] GameObject bloodPressure3;
    public UnityEvent putSensor;
    public UnityEvent putThermometer;
    public UnityEvent putBloodPressure;
    public bool sensorAnimation;
    public bool thermometerAnimation;
    public bool bloodPressureAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void TakeSensor()
    {
        if (sensorAnimation)
        {
            sensor1.SetActive(false);
            sensor2.SetActive(true);
        }
        if (thermometerAnimation)
        {
            thermometer1.SetActive(false);
            thermometer2.SetActive(true);
        }
        if (bloodPressureAnimation)
        {
            bloodPressure1.SetActive(false);
            bloodPressure2.SetActive(true);
        }

    }

    public void PutSensor()
    {
        if (sensorAnimation)
        {
            sensor2.SetActive(false);
            sensor3.SetActive(true);
            putSensor?.Invoke();
            sensorAnimation = false;
        }
        if (thermometerAnimation)
        {
            thermometer2.SetActive(false);
            //thermometer3.SetActive(true);
            putThermometer?.Invoke();
            thermometerAnimation = false;
        }
        if (bloodPressureAnimation)
        {
            bloodPressure2.SetActive(false);
            bloodPressure3.SetActive(true);
            putBloodPressure?.Invoke();
            bloodPressureAnimation = false;
        }

    }

}
// Update is called once per frame

