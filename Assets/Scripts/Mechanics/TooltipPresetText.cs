using UnityEngine;

[CreateAssetMenu(fileName = "TooltipPresetText", menuName = "Cooking/Text Tooltip Preset", order = 0)]
public class TooltipPresetText : TooltipPreset
{
    
    public override TooltipBlockUI CreateTooltipBlockUI(TooltipBlock block)
    {
        var tooltipBlockUI = Instantiate(TooltipBlockUI);

        if(Icon)
            tooltipBlockUI.SetIcon(Icon);
        
        var data = ShowName && block.Name != string.Empty ? $"{block.Name} {block.Data}" : block.Data;
        
        tooltipBlockUI.SetData(data);
        
        return tooltipBlockUI;
    }
    
}