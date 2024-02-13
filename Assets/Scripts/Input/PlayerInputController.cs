using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerDown();
        }
    }

    private void PointerDown()
    {
        var rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit);
        
        if (rayHit)
        {
            var trigger = hit.collider.GetComponent<PointerDownTrigger>();
            
            if (trigger == null)
                return;
            
            trigger.OnPointerDown();
        }
    }
    
}
