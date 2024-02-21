using UnityEngine;

[CreateAssetMenu(fileName = "PromptAggregate", menuName = "Cooking/LLM Aggregate Prompt Block", order = 0)]
public class AggregatePromptBlock : ScriptableObject
{
    public PromptClass Class;

    public PromptBlock[] PromptBlocks;
}