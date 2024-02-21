using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    
    [SerializeField] private GameIOEvents _gameIOEvents;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftClick();
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RightClick();
        }
    }

    private void LeftClick()
    {
        var rayHit = CameraRaycast(out var hit);

        if (!rayHit) 
            return;
        
        _gameIOEvents.OnLeftClickOnObject(hit.transform.gameObject);
            
        var trigger = hit.collider.GetComponent<PointerDownTrigger>();
            
        if (trigger == null)
            return;
            
        trigger.OnPointerDown();
    }
    
    private void RightClick()
    {
        var rayHit = CameraRaycast(out var hit);
        
        if (rayHit)
            _gameIOEvents.OnRightClickOnObject(hit.transform.gameObject);
    }

    private bool CameraRaycast(out RaycastHit hitInfo)
    {
        return Physics.Raycast(Utility.MainCamera.transform.position, Utility.MainCamera.transform.forward,
            out hitInfo);
    }
    
}
