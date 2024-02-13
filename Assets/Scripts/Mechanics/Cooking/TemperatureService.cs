using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemperatureService : MonoBehaviour
{

    public static TemperatureService Instance { get; private set; }

    public float TCurveMidpoint, TCurveDispersion;
    
    public float RoomTemperature;

    public float HeatFlowRate;
    
    [SerializeField] private TemperatureSource _temperatureSourcePrefab;
    
    private List<TemperatureSource> _temperatureSources = new List<TemperatureSource>();
    
    private void Awake()
    {
        Instance = this;
    }

    public float GetDeltaTemperatureAt(Vector3 position, float currentBodyTemperature, float deltaTime)
    {
        if (_temperatureSources.Count == 0)
            return 0F;

        float TemperatureCurve(float distance, float midpoint, float dispersion)
        {
            var exponent = 1F / (dispersion + 0.001F) * (distance - midpoint);
            
            return 1F / (1F + Mathf.Exp(exponent));
        }
        
        var avgPower = _temperatureSources.Sum(t => t.Power / ((position - t.transform.position).sqrMagnitude + 1F)) / _temperatureSources.Count;
        
        var avgTemp = _temperatureSources.Sum(t =>
            t.Temperature * 
            TemperatureCurve((position - t.transform.position).sqrMagnitude, TCurveMidpoint, TCurveDispersion)) /
                      _temperatureSources.Count;
        
        var temperature = (avgTemp + RoomTemperature);

        var differenceInTemperature = temperature - currentBodyTemperature;
        
        var deltaTemperature = differenceInTemperature * Mathf.Clamp(avgPower, 0.5F, 100F) * HeatFlowRate * deltaTime;
        
        return deltaTemperature;
    }
    
    public float GetTemperatureAt(Vector3 position)
    {
        if (_temperatureSources.Count == 0)
            return RoomTemperature;
        
        float GetDistSqr(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        var avgTemp = _temperatureSources.Sum(t => t.Temperature / (GetDistSqr(position, t.transform.position) + 1F)) / _temperatureSources.Count;
        
        var temperature = (avgTemp + RoomTemperature);
        
        return temperature;
    }
    
    public TemperatureSource AddTemperatureSource(Vector3 position, float temperature, float power)
    {
        var source = Instantiate(_temperatureSourcePrefab, position, Quaternion.identity);
        source.Initialize(temperature, power);
        _temperatureSources.Add(source);
        return source;
    }
    
    public void RemoveTemperatureSource(TemperatureSource source)
    {
        if (!_temperatureSources.Contains(source))
            return;
        
        _temperatureSources.Remove(source);
    }
    
}