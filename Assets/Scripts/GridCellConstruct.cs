using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridCellConstruct : GridCell
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    public enum CellType
    {
        simpleCell,
        darkNode,
        lightNode
    }
    
    public int requiredAmount;
    public int currentAmount;
    public CellType ThisCellType;
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
#if UNITY_EDITOR
            Selection.activeObject = gameObject;
#endif
        }
        else if (Input.GetMouseButton(1))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        _textMeshProUGUI.text = $"{ThisCellType.ToString()}";
    }
}