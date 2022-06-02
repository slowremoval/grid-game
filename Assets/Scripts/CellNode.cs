using TMPro;
using UnityEngine;

public class CellNode : GridCell
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    
    public enum NodeType
    {
        black,
        white
    }

    public int requiredAmount;
    public int currentAmount;
    
    public NodeType ThisNodeType;

    private void OnValidate()
    {
        if (_textMeshProUGUI == null)
        {
            return;
        }
        _textMeshProUGUI.text = $"{currentAmount}/{requiredAmount}";
    }
}