using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Solid Food Item", fileName = "SolidFoodItem", order = 0)]
public class SolidFoodItem : FoodItem
{

    public float CookingRate = 1F;

    public float ThresholdCookingTemperature = 50F;
    
}