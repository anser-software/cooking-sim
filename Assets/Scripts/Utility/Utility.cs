using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class Utility
{

    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            return _mainCamera;
        }
    }
    
    private static Camera _mainCamera;

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }
    
    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }
    
    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
    
    public static RaycastHit ScreenPointSphereCast(Vector2 screenPos, float radius, float maxDistance = Mathf.Infinity, int layerMask = -1)
    {
        var ray = MainCamera.ScreenPointToRay(screenPos);

        Physics.SphereCast(ray, radius, out var hit, maxDistance, layerMask);
        
        return hit;
    }
    
}
