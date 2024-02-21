using UnityEngine;

public abstract class TooltipPreset : ScriptableObject
{
    
    public bool ShowName;

    public Sprite Icon;
    
    public TooltipBlockUI TooltipBlockUI;
    
    public abstract TooltipBlockUI CreateTooltipBlockUI(TooltipBlock block);
    
}