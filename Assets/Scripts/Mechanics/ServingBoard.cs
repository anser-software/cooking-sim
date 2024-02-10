using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServingBoard : MonoBehaviour
{

    public Meal GetMealOnBoard()
    {
        return new Meal(GetFoodOnBoard());
    }

    private List<FoodGameObject> GetFoodOnBoard()
    {
        var bounds = GetCheckBoxBounds();

        var colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        return colliders
            .Select(coll => coll.GetComponent<FoodGameObject>())
            .Where(foodGO => foodGO != null)
            .ToList();
    }

    private Bounds GetCheckBoxBounds()
    {
        return new Bounds(transform.position + Vector3.up * 0.5F, GetComponent<BoxCollider>().bounds.size.WithY(1F));
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var bounds = GetCheckBoxBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}