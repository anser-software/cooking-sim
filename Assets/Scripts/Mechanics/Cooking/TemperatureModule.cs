using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TemperatureModule : MonoBehaviour, ITooltipProvider
{

    [SerializeField] private TooltipPreset _tooltipPreset;
    
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

    
    public TooltipPreset TooltipPreset => _tooltipPreset;

    public IEnumerable<TooltipBlock> GetTooltipData()
    {
        return new []{ new TooltipBlock("Temperature", Mathf.Round(_temperature).ToString(CultureInfo.InvariantCulture) + "°") };
    }
}
