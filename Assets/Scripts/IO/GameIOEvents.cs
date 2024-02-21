using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameIO", menuName = "IO/Game IO Events", order = 0)]
public class GameIOEvents : ScriptableObject
{

    public event Action<GameObject> OnObjectLeftClick, OnObjectRightClick;

    public void OnLeftClickOnObject(GameObject gameObject) => OnObjectLeftClick?.Invoke(gameObject);

    public void OnRightClickOnObject(GameObject gameObject) => OnObjectRightClick?.Invoke(gameObject);
    
}
