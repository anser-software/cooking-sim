using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipDisplayUI : MonoBehaviour
{
    
    [SerializeField] private GameIOEvents _gameIOEvents;

    [SerializeField] private Image _background;

    [SerializeField] private RectTransform _tooltipContainer;
    
    private bool _isShowingTooltip;
    
    private void OnEnable()
    {
        _gameIOEvents.OnObjectRightClick += TryShowTooltip;
    }

    private void TryShowTooltip(GameObject target)
    {
        if(_isShowingTooltip)
            return;
        
        var tooltipProviders = target.GetComponentsInChildren<ITooltipProvider>();
        
        if(tooltipProviders == null || tooltipProviders.Length == 0)
            return;

        
        foreach (var tooltipProvider in tooltipProviders)
        {
            foreach (var tooltipBlock in tooltipProvider.GetTooltipData())
            {
                var blockUI = tooltipProvider.TooltipPreset.CreateTooltipBlockUI(tooltipBlock);
                blockUI.transform.SetParent(_tooltipContainer, false);
            }
        }
        
        _background.enabled = true;
        
        _isShowingTooltip = true;
    }
    
    private void Update()
    {
        if(Mouse.current.rightButton.wasReleasedThisFrame)
            ResetTooltip();
    }

    private void ResetTooltip()
    {
        if (_isShowingTooltip == false)
            return;
        
        if(_tooltipContainer.childCount == 0)
            return;

        foreach (var childBlock in _tooltipContainer.GetComponentsInChildren<TooltipBlockUI>())
        {
            Destroy(childBlock.gameObject);
        }
        
        _background.enabled = false;
        
        _isShowingTooltip = false;
    }
    
    private void OnDisable()
    {
        _gameIOEvents.OnObjectRightClick -= TryShowTooltip;
    }
    
}

public interface ITooltipProvider
{
    
    public TooltipPreset TooltipPreset { get; }

    public IEnumerable<TooltipBlock> GetTooltipData();

}

public struct TooltipBlock
{
    public string Name, Data;
    
    public TooltipBlock(string name, string data) {
        Name = name;
        Data = data;
    }
}