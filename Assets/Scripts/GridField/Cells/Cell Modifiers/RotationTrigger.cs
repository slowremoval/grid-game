using System;
using UnityEngine;
using UnityEngine.EventSystems;

internal class RotationTrigger : MonoBehaviour, IPointerDownHandler
{
    public event Action OnRotationTriggerActivated;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnRotationTriggerActivated?.Invoke();
    }
}