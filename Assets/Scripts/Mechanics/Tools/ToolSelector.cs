using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolSelector : MonoBehaviour
{

    [SerializeField] private PlayerTool[] _playerTools;
    
    private void Start()
    {
        SelectTool(0);
    }
    
    private void SelectTool(int index)
    {
        for (var i = 0; i < _playerTools.Length; i++)
        {
            if(i == index)
                _playerTools[i].Select();
            else
                _playerTools[i].Deselect();
        }
    }

    private void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame) 
            SelectTool(0);
        else if(Keyboard.current.digit2Key.wasPressedThisFrame)
            SelectTool(1);
        else if(Keyboard.current.digit3Key.wasPressedThisFrame)
            SelectTool(2);
        else if(Keyboard.current.digit4Key.wasPressedThisFrame)
            SelectTool(3);
    }
}
