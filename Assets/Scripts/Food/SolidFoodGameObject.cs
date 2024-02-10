using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SolidFoodGameObject : FoodGameObject
{
    
    [SerializeField] private SolidFoodItem _foodItem;

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

        stateDescription += $"Weight: {_rb.mass}\n";
        stateDescription += $"Cooking Progress: {_cookingProgress}";
        
        return stateDescription;
    }

    private void Update()
    {
        var currentTemperature = TemperatureService.Instance.GetTemperatureAt(transform.position);

        if (currentTemperature < _foodItem.ThresholdCookingTemperature)
            return;

        var deltaCooking = (currentTemperature - _foodItem.ThresholdCookingTemperature) * _foodItem.CookingRate *
                           Time.deltaTime;
        
        _cookingProgress += deltaCooking;
    }
}