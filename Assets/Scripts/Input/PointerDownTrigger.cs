using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerDownTrigger : MonoBehaviour
{

    [SerializeField] private UnityEvent onPointerDown;
    
    public void OnPointerDown()
    {
        Debug.Log("OnPointerDown");
        onPointerDown?.Invoke();
    }

}
