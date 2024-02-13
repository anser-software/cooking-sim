using UnityEngine;

public class TemperatureSource : MonoBehaviour
{

    public float Temperature { get; private set; }

    public float Power { get; private set; }

    public void Initialize(float temperature, float power)
    {
        Temperature = temperature;
        Power = power;
    }
    
}