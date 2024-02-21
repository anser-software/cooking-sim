using System;
using UnityEngine;

[Serializable]
public struct PromptSegment
{
    public PromptClass Class;
    
    [TextArea(4, 24)]
    public string PromptText;
}

[Serializable]
public struct LayeredPromptSegment
{

    public PromptClass ParentClass;
    
    public PromptClass[] ChildClasses;

}