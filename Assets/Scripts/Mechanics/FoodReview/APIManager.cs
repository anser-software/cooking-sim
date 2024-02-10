using System;
using System.Collections;
using System.Collections.Generic;
using com.studios.taprobana;
using Newtonsoft.Json;
using UnityEngine;

public class APIManager : MonoBehaviour
{
     private ChatCompletionsApi chatCompletionsApi;
    private readonly string apiKey = "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";

    private string _baseSystemPrompt =
        "You play the role of a Gamemaster. We will engage in a story told by the Gamemaster of a role-playing campaign. Gamemaster is good at writing, always follows Player’s instructions and comes up with very engaging and entertaining scenarios.";
    
    private string _currentStatus = "Beginning of the story";
    
    private void Start()
    {
        chatCompletionsApi = new ChatCompletionsApi(apiKey);
        chatCompletionsApi.ConversationHistoryMemory = 0;
        //chatCompletionsApi.SetSystemMessage("You play the role of a Gamemaster. We will engage in a story told by the Gamemaster of a role-playing campaign. Gamemaster is good at writing, always follows Player’s instructions and comes up with very engaging and entertaining scenarios.");
    }

    public void ConfigureSettingAndCharacter(string setting, string character)
    {
        var message = $"{_baseSystemPrompt}\r\nSetting - {setting}\r\nCharacter - {character}";
        chatCompletionsApi.SetSystemMessage(message);
        GetResponse();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        { 
            //GetResponse();
        }
    }

    private Function GetFunctionData()
    {
        var description = "Given the CURRENT_STATUS, continue the story in an interesting way, the continuation should be returned in parameter new_status." +
                          " Offer the player a choice of 3 actions, each of them should be given in parameter action_1, action_2, action_3 respectively.";
        //var description = "Generate 1 question about the provided CHARACTER, generate 2 answers (answer_1 and answer_2), " +
        //                  "one answer should be correct and other should be incorrect, the correct answer out of them should be given in parameter correct";
        var funcName = "status_update";

        var parameters = new Parameter();
        parameters.AddProperty("new_status", DataTypes.STRING, "new status");
        parameters.AddProperty("action_1", DataTypes.STRING, "action 1");
        parameters.AddProperty("action_2", DataTypes.STRING, "action 2");
        parameters.AddProperty("action_3", DataTypes.STRING, "action 3");

        var function = new Function(funcName, description, parameters);
        return function;
    }

    public async void GetResponse()
    {
        try
        {
            var input = "CURRENT_STATUS: " + _currentStatus;
            var request = new ChatCompletionsRequest();
            Message message = new(Roles.USER, input);
            request.AddMessage(message);
            request.AddFunction(GetFunctionData());

            var res = await chatCompletionsApi.CreateChatCompletionsRequest(request);

            var output = res.GetFunctionCallResponse().Arguments;
            
            var outputConverted = JsonConvert.DeserializeObject<ModelResponse>(output);
            
            Debug.Log(outputConverted.new_status);
            
            Debug.Log(res.GetFunctionCallResponse().Arguments);

            // input:- CHARACTER: Mario
            // Response
            //{
            //    "question": "In which year was the first Mario game released?",
            //    "answer_1": "1981",
            //    "answer_2": "1985",
            //    "correct": "1985"
            //}
        }
        catch (OpenAiRequestException exception)
        {
            Debug.LogError(exception);
        }
    }
}

public class ModelResponse
{
    public string new_status, action_1, action_2, action_3;
}
