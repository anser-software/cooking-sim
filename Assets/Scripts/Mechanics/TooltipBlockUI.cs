using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBlockUI : MonoBehaviour
{
    
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _data;
    
    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
    
    public void SetData(string data)
    {
        _data.text = data;
    }

}