using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonGO : MonoBehaviour
{
    
    [SerializeField] private UnityEvent onActivate;
    
    public void Activate()
    {
        onActivate?.Invoke();
    }
    
}
