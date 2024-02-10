using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodReviewDebug : MonoBehaviour
{
    
    [SerializeField] private ServingBoard _servingBoard;
    
    [SerializeField] private StreamAPIManager _streamAPIManager;

    [SerializeField] private TextMeshProUGUI _responseText;
    
    [SerializeField, TextArea] private string _systemPrompt;

    [SerializeField, TextArea] private string _debugMealDescription;

    private string _response = string.Empty;
    
    private void Start()
    {
        _streamAPIManager.OnResponseGenerated += OnResponseGenerated;
    }

    private void OnResponseGenerated(string obj)
    {
        _response += obj;
        _responseText.SetText(_response);
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            Review();
        if (Keyboard.current.tKey.wasPressedThisFrame)
            DebugPromptLLM();
    }

    private void Review()
    {
        var meal = _servingBoard.GetMealOnBoard();

        var description = meal.GetDescription();
    }

    private void DebugPromptLLM()
    {
        _response = string.Empty;
        _streamAPIManager.Prompt(_systemPrompt, _debugMealDescription);
    }
    
}
