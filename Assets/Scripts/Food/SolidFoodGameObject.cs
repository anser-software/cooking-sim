using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SolidFoodGameObject : FoodGameObject
{

    [SerializeField] private SolidFoodItem _foodItem;

    [SerializeField] private TemperatureModule _temperatureModule;
    
    private float _cookingProgress;
    
    private Rigidbody _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override string GetFoodName() => _foodItem.FoodName;
    
    public override string GetStateDescription()
    {
        var stateDescription = string.Empty;

        stateDescription += $"[Weight]: {_rb.mass * 1000F} grams \n";
        stateDescription += $"[Cooking Progress]: {Mathf.RoundToInt(_cookingProgress * 100F)}%\n";
        
        return stateDescription;
    }

    private void Update()
    {
        UpdateCooking();
    }

    private void UpdateCooking()
    {
        var temperature = _temperatureModule.Temperature;

        if (temperature < _foodItem.ThresholdCookingTemperature)
            return;

        var deltaCooking = (temperature - _foodItem.ThresholdCookingTemperature) * _foodItem.CookingRate * Time.deltaTime;
        
        _cookingProgress += deltaCooking;
        
        Debug.Log(GetFoodName());
        Debug.Log($"Cooking Progress: {_cookingProgress * 100F}%");
    }
    
}