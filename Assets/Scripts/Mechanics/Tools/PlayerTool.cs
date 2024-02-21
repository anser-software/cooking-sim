using UnityEngine;

public abstract class PlayerTool : MonoBehaviour
{
    
    [SerializeField]
    protected GameObject _toolVisuals;

    protected bool IsSelected;
    
    public virtual void Select()
    {
        if(_toolVisuals)
            _toolVisuals.SetActive(true);
        
        IsSelected = true;
    }
    
    public virtual void Deselect()
    {
        if(_toolVisuals)
            _toolVisuals.SetActive(false);
        
        IsSelected = false;
    }
    
}