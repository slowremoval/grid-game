using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GridCell : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public Vector2 Coordinates;
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
#if UNITY_EDITOR
            Selection.activeObject = gameObject;
#endif
        }
    }
}