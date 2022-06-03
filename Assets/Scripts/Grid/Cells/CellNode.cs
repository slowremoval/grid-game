using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellNode : GridCell
{
    public enum NodeType
    {
        dark,
        light
    }
    
    
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private GameObject _visualization;

    public int requiredAmount;
    public int currentAmount;
    
    public NodeType ThisNodeType;
    private Image _nodeVisualization;

    private void Start()
    {
        _nodeVisualization = _visualization.GetComponent<Image>();
        
        if (_textMeshProUGUI == null)
        {
            return;
        }
        _textMeshProUGUI.text = $"{currentAmount}/{requiredAmount}";

        switch (ThisNodeType)
        {
            case NodeType.dark:
                _nodeVisualization.color = Color.gray;
                break;
            case NodeType.light:
                _nodeVisualization.color = Color.yellow;
                break;
        }
    }
}