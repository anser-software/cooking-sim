using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemperatureService : MonoBehaviour
{
    
    public static TemperatureService Instance;
    
    
    public float RoomTemperature;
    
    private List<TemperatureSource> _temperatureSources = new List<TemperatureSource>();
    
    private void Awake()
    {
        Instance = this;
    }
    
    public float GetTemperatureAt(Vector3 position)
    {
        if (_temperatureSources.Count == 0)
            return RoomTemperature;
        
        float GetDistSqr(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        var maxTemp = _temperatureSources
            .OrderByDescending(s => s.Temperature / GetDistSqr(position, s.Position))
            .First().Temperature;
        
        return Mathf.Max(RoomTemperature, maxTemp);
    }
    
    public TemperatureSource AddTemperatureSource(Vector3 position, float temperature)
    {
        var source = new TemperatureSource(position, temperature);
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

public class TemperatureSource
{
    public Vector3 Position;
    public float Temperature;

    public TemperatureSource(Vector3 position, float temperature)
    {
        Position = position;
        Temperature = temperature;
    }
}
