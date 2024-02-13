using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{

    [Tooltip("TEMPORARY")] 
    [SerializeField] private float _temperature, _powerPerBurner;
    
    [SerializeField] private Transform[] _burners;

    private int _burnerIndex;

    //private bool _enabledBurners = true;
    
    private TemperatureSource[] _temperatureSources;

    private void Awake()
    {
        _temperatureSources = new TemperatureSource[_burners.Length];
    }

    public void ActivateBurner()
    {
        _temperatureSources[_burnerIndex] = TemperatureService.Instance.AddTemperatureSource(_burners[_burnerIndex].position, _temperature, _powerPerBurner);
        
        _burnerIndex++;

        if (_burnerIndex < _burners.Length) 
            return;
        
        _burnerIndex = 0;
        
        foreach (var source in _temperatureSources)
        {
            TemperatureService.Instance.RemoveTemperatureSource(source);
        }
        //_enabledBurners = !_enabledBurners;
    }
    
}
