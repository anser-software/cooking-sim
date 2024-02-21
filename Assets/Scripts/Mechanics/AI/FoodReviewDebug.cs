using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FoodReviewDebug : MonoBehaviour
{
    
    [SerializeField] private ServingBoard _servingBoard;
    
    [SerializeField] private StreamAPIManager _streamAPIManager;

    [SerializeField] private TextMeshProUGUI _responseText;
    
    [SerializeField, TextArea] private string _systemPrompt;

    [SerializeField, TextArea] private string _debugMealDescription;

    [SerializeField] private PromptBlock[] _gimmicks;
    
    [SerializeField] private PromptBlock[] _examples;
    
    [SerializeField] private AggregatePromptBlock[] _personalityDimensions;

    [SerializeField] private Prompt _foodSystemPrompt, _foodOrderPrompt, _foodReviewPrompt;
    
    private string _response = string.Empty;

    private string _foodSystemPromptText, _foodOrderPromptText, _foodReviewPromptText;
    
    private PromptBlock _currentGimmick, _currentExamples;

    private PromptBlock[] _currentPersonalities;

    private string _orderText;
    
    private void Start()
    {
        _streamAPIManager.OnResponseGenerated += UpdateTextUI;

        ResetEverything();
    }

    private void ResetEverything()
    {
        SetCharacter();

        InitializePrompts();
    }
    
    private void InitializePrompts()
    {
        SetSystemPrompt();

        //var blocksWithExamples = new [] { _currentGimmick, _currentPersonality, _currentExamples };
        
        var gimmickOrderRules = _currentGimmick.GetPromptText(_foodOrderPrompt.PromptClasses[0].ChildClasses[0]);
        var personalityOrderRules = _currentPersonalities.Select(p => p.GetPromptText(_foodOrderPrompt.PromptClasses[1].ChildClasses[0])).Aggregate((a, b) => a + "\n" + b);
        var examples = _examples.Select(p => p.GetPromptText(_foodOrderPrompt.PromptClasses[2].ChildClasses[0])).Aggregate((a, b) => a + "\n" + b);

        _foodOrderPromptText = string.Format(_foodOrderPrompt.GetTextRaw(), gimmickOrderRules, personalityOrderRules, examples);
        //_foodOrderPromptText = _foodOrderPrompt.GetText(blocksWithExamples);
        
        Debug.Log($"Order Prompt: {_foodOrderPromptText}");

        _foodReviewPromptText = _foodReviewPrompt.GetTextRaw();
    }

    private void SetCharacter()
    {
        _currentGimmick = _gimmicks[Random.Range(0, _gimmicks.Length)];

        var traitCount = Random.Range(1, _personalityDimensions.Length);

        var indices = new List<int>();
        
        while (indices.Count < traitCount)
        {
            var rand = Random.Range(0, _personalityDimensions.Length);
            
            if (!indices.Contains(rand))
                indices.Add(rand);
        }
        
        _currentPersonalities = new PromptBlock[traitCount];
        
        for (var i = 0; i < traitCount; i++)
        {
            var index = indices[i];
            _currentPersonalities[i] = _personalityDimensions[index].PromptBlocks[Random.Range(0, _personalityDimensions[index].PromptBlocks.Length)];
        }
        
        //var currentPersonalityDimension = _personalityDimensions[Random.Range(0, _personalityDimensions.Length)];
        //_currentPersonality = currentPersonalityDimension.PromptBlocks[Random.Range(0, currentPersonalityDimension.PromptBlocks.Length)];

        var example = _examples[Random.Range(0, _examples.Length)];
        _currentExamples = example;
    }
    
    private void SetSystemPrompt()
    {
        //var blocks = new [] { _currentGimmick, _currentPersonality };
        //_foodSystemPromptText = _foodSystemPrompt.GetText(blocks);
        var gimmickDescription = _currentGimmick.GetPromptText(_foodSystemPrompt.PromptClasses[0].ChildClasses[0]);
        var gimmickGeneralRules = _currentGimmick.GetPromptText(_foodSystemPrompt.PromptClasses[0].ChildClasses[1]);
        
        var personalityDescription = _currentPersonalities.Select(p => p.GetPromptText(_foodSystemPrompt.PromptClasses[1].ChildClasses[0])).Aggregate((a, b) => a + "\n" + b);
        var personalityTraits = _currentPersonalities.Select(p => p.GetPromptText(_foodSystemPrompt.PromptClasses[1].ChildClasses[1])).Aggregate((a, b) => a + "\n" + b);

        var systemPrompt = _foodSystemPrompt.GetTextRaw();
        
        _foodSystemPromptText = string.Format(systemPrompt, gimmickDescription, gimmickGeneralRules, personalityDescription, personalityTraits);
        
        Debug.Log($"System Prompt: {_foodSystemPromptText}");
    }
    
    private void UpdateTextUI(string obj)
    {
        _response += obj;
        _responseText.SetText(_response);
    }

    private void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
            Order();
        if (Keyboard.current.rKey.wasPressedThisFrame)
            Review();
    }

    private void Order()
    {
        _response = string.Empty;
        _streamAPIManager.OnResponseCompleted += OnOrderGenerated;

        _streamAPIManager.Prompt(_foodSystemPromptText, _foodOrderPromptText);
    }

    private void OnOrderGenerated(string order)
    {
        _orderText = order;
        _streamAPIManager.OnResponseCompleted -= OnOrderGenerated;
    }
    
    private void Review()
    {
        var meal = _servingBoard.GetMealOnBoard();

        if (meal.IsEmpty())
            return;
        
        _response = string.Empty;
        
        var description = meal.GetDescription();
        
        description = description.Insert(0, "Meal Description: \n");

        var foodReviewPrompt = string.Format(_foodReviewPromptText, _orderText);
        
        Debug.Log($"Food Review Prompt: {foodReviewPrompt}");
        
        _streamAPIManager.Prompt(_foodSystemPromptText + "\n" + foodReviewPrompt, description);
        
        ResetEverything();
    }

    
}
