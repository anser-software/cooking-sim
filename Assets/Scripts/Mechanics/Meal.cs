using System.Collections.Generic;

public class Meal
{
    public List<FoodGameObject> FoodObjects;

    public Meal(List<FoodGameObject> foodObjects)
    {
        FoodObjects = foodObjects;
    }

    public string GetDescription()
    {
        var description = string.Empty;
        
        foreach (var foodObject in FoodObjects)
        {
            description += foodObject.GetFoodName() + "\n";
            description += foodObject.GetStateDescription() + "\n";
        }
        
        return description;
    }
}