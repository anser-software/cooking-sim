using System.Collections.Generic;

public class Meal
{
    public List<FoodGameObject> FoodObjects;

    public Meal(List<FoodGameObject> foodObjects)
    {
        FoodObjects = foodObjects;
    }

    public bool IsEmpty()
    {
        return FoodObjects.Count == 0;
    }

    public string GetDescription()
    {
        var description = string.Empty;

        var countPerFoodType = new Dictionary<string, int>();
        
        for (var i = 0; i < FoodObjects.Count; i++)
        {
            var foodObject = FoodObjects[i];
            var name = foodObject.GetFoodName() + "\n";
            var desc = foodObject.GetStateDescription() + "\n";
            
            description += "-----------------\n";
            description += name;
            description += desc;
            
            countPerFoodType[name] = countPerFoodType.GetValueOrDefault(name, 0) + 1;
        }

        return description;
    }
}