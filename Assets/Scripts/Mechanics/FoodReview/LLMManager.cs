using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
//using LLama;
//using LLama.Native;
//using LLama.Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class LLMManager : MonoBehaviour
{

    public string ZeroPrompt => _zeroPrompt;
    
    public Action<string> OnTokenGenerated;

    public string ModelPath = "models/mistral-7b-instruct-v0.1.Q4_K_M.gguf"; // change it to your own model path
    //public TMP_Text Output;
    //public TMP_InputField Input;
    //public Button Submit;

    private string _submittedText = "";

    private string _zeroPrompt = "A story told by the Gamemaster of a role-playing campaign. Gamemaster is good at writing, always follows Player’s instructions and comes up with very engaging and entertaining scenarios. \n\nFirst, the Player introduces the setting of the campaign and their character. Then the Gamemaster establishes the character, location and current situation. Finally the Gamemaster offers the Player 3 actions for their character to choose from. \n\nPlayer:";

    //private ChatSession _session;
    
    public void Prompt(string prompt)
    {
        _submittedText = prompt + "\r\nGamemaster:";
    }

    public void ActionPrompt(string state)
    {
        _submittedText = state + "\r\nGamemaster: The Player can choose one of the following 3 actions:\r\n";
    }
    /*
    async UniTaskVoid Start()
    {
        //var bobPrompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.

        var bobPrompt = _zeroPrompt;
        
        // Load a model
        var parameters = new ModelParams(Application.streamingAssetsPath + "/" + ModelPath)
        {
            ContextSize = 4096,
            Seed = 1024,
            GpuLayerCount = 35
        };
        
        // Switch to the thread pool for long-running operations
        await UniTask.SwitchToThreadPool();
        using var model = LLamaWeights.LoadFromFile(parameters);
        await UniTask.SwitchToMainThread();
        
        // Initialize a chat session
        using var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        var session = new ChatSession(ex);

        // run the inference in a loop to chat with LLM
        while (bobPrompt != "stop")
        {
            await foreach (var token in ChatConcurrent(
                session.ChatAsync(
                    bobPrompt, 
                    new InferenceParams() 
                    { 
                        Temperature = 0.6f, 
                        AntiPrompts = new List<string> { "Player:" } 
                    }
                )
            ))
            {
                await UniTask.Yield();
                
                OnTokenGenerated?.Invoke(token);
            }
            
            await UniTask.WaitUntil(() => _submittedText != "");
            
            bobPrompt = _submittedText;
            _submittedText = "";
        }
    }

    /// <summary>
    /// Wraps AsyncEnumerable with transition to the thread pool. 
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns>IAsyncEnumerable computed on a thread pool</returns>
    private async IAsyncEnumerable<string> ChatConcurrent(IAsyncEnumerable<string> tokens)
    {
        await UniTask.SwitchToThreadPool();
        await foreach (var token in tokens)
        {
            yield return token;
        }
    }*/
}