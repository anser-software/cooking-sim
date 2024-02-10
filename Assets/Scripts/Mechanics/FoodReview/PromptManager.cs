using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptManager : MonoBehaviour
{

    private string ActionRequestPrompt => $"{BasePrompt} \r\n {_latestState}";
    
    private string InitialPrompt => $"{BasePrompt}\r\nGamemaster, please describe an initial state in which the character finds themselves.";
    
    private string _baseSystemPrompt =
        "You play the role of a Gamemaster. We will engage in a story told by the Gamemaster of a role-playing campaign. Gamemaster is good at writing, always follows Player’s instructions and comes up with very engaging and entertaining scenarios.";

    private string BasePrompt => $"The setting of the story - {_settingDescription}.\r\nThe character - {_characterDescription}.";

    
    [SerializeField] private StreamAPIManager _streamAPIManager;

    [SerializeField] private Button _promptButton;
    [SerializeField] private TMP_InputField _settingField, _characterField;
    [SerializeField] private TMP_Text _outputText;

    [SerializeField] private Button[] _optionButtons;
    
    private string _settingDescription, _characterDescription;

    private string _latestState;

    private bool _gameStarted;
    
    private string[] _lastActions = new string[3];
    
    private void Awake()
    {
        //_streamAPIManager.OnTokenGenerated += OnTokenGenerated;
        
        _streamAPIManager.OnResponseGenerated += OnResponseGenerated;
        _streamAPIManager.OnResponseCompleted += UpdateUIFromResponse;

        _promptButton.onClick.AddListener(OnPromptButtonClicked);

        for (var i = 0; i < _optionButtons.Length; i++)
        {
            var i1 = i;
            _optionButtons[i].onClick.AddListener(() => SelectOption(i1 + 1));
        }

        _settingField.text =
            "The Cabal has been established in Nevada territory. Undead and werewolves roam the range. They are challenged by a group of gunslingers and martial artists. Silver bullets and ancient Chinese mysticism against fangs and claws.";
        _characterField.text = "Gunslinger named John";
    }

    private void UpdateUIFromResponse(ModelResponse response)
    {
        //_buttonsParent.SetActive(true);
    }
    
    private void SelectOption(int option)
    {
        foreach (var button in _optionButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        //_streamAPIManager.Prompt($"I am choosing option {option}: {_lastActions[option - 1]}");
    }

    private void OnPromptButtonClicked()
    {
        _outputText.text += "\n";
        
        _settingDescription = _settingField.text;
        _characterDescription = _characterField.text;

        _streamAPIManager.ConfigureSettingAndCharacter(_baseSystemPrompt, _settingDescription, _characterDescription);

        _gameStarted = true;
        
        _settingField.gameObject.SetActive(false);
        _characterField.gameObject.SetActive(false);
        _promptButton.gameObject.SetActive(false);

        _settingField.text = _characterField.text = _latestState = "";
    }

    private void OnResponseGenerated(string response)
    {
        var parameter = 0;
        if(parameter > 3) 
            return;

        if (parameter == 0)
        {
            _latestState = response;
            _outputText.text = response;
            return;
        }
        
        _optionButtons[parameter - 1].gameObject.SetActive(true);
        _optionButtons[parameter - 1].GetComponentInChildren<TextMeshProUGUI>().text = _lastActions[parameter - 1] = response;
    }
    
    private void OnTokenGenerated(string token)
    {
        _latestState += token;
        _outputText.text += token;
    }


}
