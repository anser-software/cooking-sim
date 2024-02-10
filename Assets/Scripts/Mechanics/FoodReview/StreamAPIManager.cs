using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

public class StreamAPIManager : MonoBehaviour
{
    
    public Action<string> OnResponseGenerated;

    public Action<ModelResponse> OnResponseCompleted;


    private string _currentStatus = "Beginning of the story";

    private bool _systemSetup;

    private List<Message> _messages = new List<Message>();

    private string _responseBuffer = string.Empty;
    
    private OpenAIClient _api;

    private List<Tool> _tools = new List<Tool>();

    public void ConfigureSettingAndCharacter(string systemPrompt, string setting, string character)
    {
        var message = $"{systemPrompt}\r\nSetting - {setting}\r\nCharacter - {character}";

        _messages.Add(new Message(Role.System, message));
        
        _systemSetup = true;

        _tools = new List<Tool>
        {
            GetFunctionData()
        };
        
        UniTask.Action(MakeRequest).Invoke();
    }
    
    public void Prompt(string systemPrompt, string prompt)
    {
        _messages.Clear();
        _messages.Add(new Message(Role.System, systemPrompt));
        _messages.Add(new Message(Role.User, prompt));

        UniTask.Action(PromptModel).Invoke();
    }

    async UniTaskVoid PromptModel()
    {
        var chatRequest = new ChatRequest(_messages);
        
        var response = await _api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
        {
            OnResponseGenerated(partialResponse.FirstChoice.Delta.ToString());
        });
        
        var choice = response.FirstChoice;
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
    }

    async UniTaskVoid MakeRequest()
    {
        foreach (var message in _messages)
        {
            Debug.Log($"{message.Role}: {message}");
        }

        
        _responseBuffer = string.Empty;
        
        var chatRequest = new ChatRequest(_messages, tools: _tools, toolChoice: _tools[0].Function.Name);

        var response = await _api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
        {
            var delta = partialResponse.FirstChoice.Delta;

            if (delta.ToolCalls == null || delta.ToolCalls.Count == 0)
                return;
            
            var messageChunk = delta.ToolCalls[0].Function.Arguments.ToString();
            
            ProcessResponseStream(messageChunk);
        });

        var functionResult = GetStructuredResponse(response);
        
        OnResponseCompleted?.Invoke(functionResult);
        
        _messages.Add(new Message(Role.System, "CURRENT_STATUS: " + functionResult.new_status));
    }
    
    private void ProcessResponseStream(string messageChunk)
    {
        _responseBuffer += messageChunk;
        
        Debug.Log(_responseBuffer);
        
        OnResponseGenerated?.Invoke(_responseBuffer);
    }
    
    
    async UniTaskVoid Start()
    {
        _api = new OpenAIClient("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");
        
        //await UniTask.WaitUntil(() => _systemSetup);
    }

    private ModelResponse GetStructuredResponse(ChatResponse input)
    {
        var usedTool = input.FirstChoice.Message.ToolCalls[0];
        
        var convertedResponse = JsonConvert.DeserializeObject<ModelResponse>(usedTool.Function.Arguments.ToString());

        return convertedResponse;
    }
    
    
    private Function GetFunctionData()
    {
        var description = "Given the CURRENT_STATUS, continue the story in an interesting way, the continuation should be returned in parameter new_status." +
                          " Offer the player a choice of 3 actions, each of them should be given in parameter action_1, action_2, action_3 respectively.";

        var funcName = "status_update";

        return new Function(
            funcName,
            description,
            new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["new_status"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "The new story status."
                    },
                    ["action_1"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Option 1."
                    },
                    ["action_2"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Option 2."
                    },
                    ["action_3"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Option 3."
                    }
                },
                ["required"] = new JArray { "new_status", "action_1", "action_2", "action_3" }
            });
    }
    
}

public class WeatherArgs
{
    public string location;
    public string unit;
}