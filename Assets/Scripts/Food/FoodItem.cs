using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Cooking/Food Item", order = 0)]
public abstract class FoodItem : ScriptableObject
{

    public string FoodName;
    
}