using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PromptBlock", menuName = "Cooking/LLM Prompt Block", order = 0)]
public class PromptBlock : ScriptableObject
{

    public PromptClass Class;
    
    public PromptSegment[] PromptSegments;

    public PromptClass GetPromptClass()
    {
        return Class;
    }
    
    public string GetPromptText(string className)
    {
        return PromptSegments.First(x => x.Class.Name == className).PromptText;
    }
    
    public string GetPromptText(PromptClass promptClass)
    {
        return PromptSegments.First(x => x.Class == promptClass).PromptText;
    }
    
    public bool TryGetPromptText(PromptClass promptClass, out string text)
    {
        text = string.Empty;
        
        var targetClassSubset = PromptSegments.Where(x => x.Class == promptClass).ToList();

        switch (targetClassSubset.Count)
        {
            case 0:
                return false;
            case 1:
                text = targetClassSubset[0].PromptText;
                break;
            default:
                text =  PromptSegments.Select(s => s.PromptText).Aggregate((a, b) => a + "\n" + b);
                break;
        }

        return true;
    }
}