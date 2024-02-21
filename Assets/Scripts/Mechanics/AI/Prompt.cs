using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Prompt", menuName = "Cooking/LLM Prompt", order = 0)]
public class Prompt : ScriptableObject
{

    [TextArea(4, 24)]
    public string PromptText;
    
    public LayeredPromptSegment[] PromptClasses;

    public string GetTextRaw() => PromptText;
    
    public string GetText(params PromptBlock[] args)
    {
        var totalBlanks = PromptClasses.Sum(c => c.ChildClasses.Length);
        
        var outputs = new string[totalBlanks];
        //var inputs = new string[PromptClasses.Length];

        var currentClassIndex = 0;
        
        var classes = new List<LayeredPromptSegment>(PromptClasses);
        
        foreach (var block in args)
        {
            var parentClass = classes.FirstOrDefault(c => c.ParentClass == block.Class);
            
            if(Equals(parentClass, default(LayeredPromptSegment)))
                continue;
            
            classes.Remove(parentClass);

            foreach (var childClass in parentClass.ChildClasses)
            {
                var hasThisClass = block.TryGetPromptText(childClass, out var text);
                
                if (hasThisClass == false)
                    continue;
                
                outputs[currentClassIndex] = text + "\n";
                currentClassIndex++;
                    
                if(currentClassIndex >= totalBlanks)
                    return string.Format(PromptText, (string[])outputs);
            }
        }
        
        return string.Format(PromptText, (string[])outputs);
    }
    
}