using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridField.Cells
{
    public abstract class GridCell : MonoBehaviour, IPointerDownHandler
    {
        public Vector2 Coordinates;

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