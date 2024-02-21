using System;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class KnifeTool : PlayerTool
{

    [SerializeField] private Material _tempCrossSectionMat;
    
    [SerializeField] private float _sliceForce;
    
    private bool _isDragging;

    private Ray _startDragRay;


    private void Start()
    {
        Select();
    }


    private void Update()
    {
        if(IsSelected == false)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _startDragRay = Utility.MainCamera.ViewportPointToRay(Vector3.one * 0.5F);
        } else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(_isDragging)
                TrySlice();
        }
    }

    private void TrySlice()
    {
        _isDragging = false;
        
        var currentRay = Utility.MainCamera.ViewportPointToRay(Vector3.one * 0.5F);

        var finalRay = new Ray()
        {
            origin = (currentRay.origin + _startDragRay.origin) / 2F,
            direction = Vector3.Slerp(currentRay.direction, _startDragRay.direction, 0.5F)
        };

        var isValid = Physics.SphereCast(finalRay, 0.05F, out var hitInfo);
        
        if(isValid == false)
            return;

        if(hitInfo.rigidbody == null)
            return;
        
        var slicePlaneNormal = Vector3.Cross(currentRay.direction, _startDragRay.direction).normalized;
        
        SliceObject(hitInfo.rigidbody.gameObject, hitInfo.point, slicePlaneNormal);
    }

    private void SliceObject(GameObject target, Vector3 point, Vector3 normal)
    {
        var slices = target.SliceInstantiate(point, normal, _tempCrossSectionMat);
    
        if(slices == null)
            return;

        var forceDirection = normal;
        
        foreach (var slice in slices)
        {
            slice.transform.position = target.transform.position;
            slice.AddComponent<MeshCollider>().convex = true;
            slice.AddComponent<Rigidbody>().AddForce((forceDirection + Vector3.up + Random.onUnitSphere).normalized * _sliceForce, ForceMode.Impulse);
            forceDirection = -forceDirection;
        }
        
        target.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(_isDragging == false)
            return;
        
        var currentRay = Utility.MainCamera.ViewportPointToRay(Vector3.one * 0.5F);

        var rayOrigin = (currentRay.origin + _startDragRay.origin) / 2F;
        
        var rayDirection = Vector3.Slerp(currentRay.direction, _startDragRay.direction, 0.5F);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * 5F);

        var result = Physics.SphereCast(new Ray(rayOrigin, rayDirection), 0.1F, out var raycast);
        
        if(result == false)
            return;

        Gizmos.DrawWireSphere(raycast.point, 0.1F);
    }

    public override void Select()
    {
        base.Select();
        
        _isDragging = false;
    }
 
    
}