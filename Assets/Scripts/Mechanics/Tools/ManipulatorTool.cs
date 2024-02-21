using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManipulatorTool : PlayerTool
{

    [SerializeField] private float _snapStrength, _zoomStrength;
    
    private float _distance;
    
    private Rigidbody _rb;
    
    private void Update()
    {
        if(IsSelected == false)
            return;
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PickUp();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Drop();
        }
        
        ProcessDistance();
    }
    
    private void ProcessDistance()
    {
        if(_rb == null)
            return;

        var deltaZoom = 0F;
        
        if (Keyboard.current.qKey.isPressed)
            deltaZoom = -1F;
        else if (Keyboard.current.eKey.isPressed)
            deltaZoom = 1F;
        
        _distance = Mathf.Clamp(_distance + deltaZoom * Time.deltaTime * _zoomStrength, 0.5F, 1F);
    }

    private void LateUpdate()
    {
        Drag();
    }

    private void Drag()
    {
        if (_rb == null)
            return;
        
        var targetPoint = Utility.MainCamera.transform.position + Utility.MainCamera.transform.forward * _distance;
            
        var displacement = Vector3.ClampMagnitude(targetPoint - _rb.position, 0.5F);
            
        _rb.velocity = displacement * _snapStrength;

        //_rb.angularVelocity *= 0.1F;
    }

    private void PickUp()
    {
        var rayHit = Physics.Raycast(Utility.MainCamera.transform.position, Utility.MainCamera.transform.forward, out RaycastHit hit);
        
        if (rayHit)
        {
            _rb = hit.collider.GetComponent<Rigidbody>();
            
            if (_rb == null)
                return;
            
            _distance = Vector3.Distance(Utility.MainCamera.transform.position, _rb.position);
            
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Drop()
    {
        if (_rb == null)
            return;
        
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
        
        _rb = null;
    }

    public override void Deselect()
    {
        base.Deselect();
        
        Drop();
    }
}
