using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridField
{
    public class UIElementSelectorInEditor : UIElementSelector
    {
        [SerializeField] private GameObject _targetGameObject;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
#if UNITY_EDITOR
                Selection.activeObject = _targetGameObject;
#endif
            }
        }
    }
}