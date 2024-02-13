using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureModule : MonoBehaviour
{

    public float Temperature => _temperature;

    private float _temperature;
    
    private void Start()
    {
        _temperature = TemperatureService.Instance.RoomTemperature;
    }

    private void Update()
    {
        var deltaTemperature = TemperatureService.Instance.GetDeltaTemperatureAt(transform.position, _temperature, Time.deltaTime);
        
        _temperature += deltaTemperature;
        
        //Debug.Log($"Current Temperature: {_temperature}");
    }
}
