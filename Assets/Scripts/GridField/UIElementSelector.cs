using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridField
{
    public abstract class UIElementSelector : MonoBehaviour, IPointerDownHandler
    {
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
#if UNITY_EDITOR
                Selection.activeObject = gameObject;
#endif
            }
        }
    }
}