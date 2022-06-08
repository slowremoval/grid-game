using GridField.Cells;
using UnityEngine;
using UnityEngine.UI;

public class CellSolid : GridCell
{
    [SerializeField] private GameObject _visualization;
    
    private Image _cellImage;

    public CellType _solidCellType { get; private set; }

    public void SetCellType(CellType cellType) => _solidCellType = cellType;


    private void Awake()
    {
        _cellImage = _visualization.GetComponent<Image>();
    }

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        switch (_solidCellType)
        {
            case CellType.dark:
                _cellImage.color = Color.grey;
                break;
            case CellType.light:
                _cellImage.color = Color.yellow;
                break;
            default:
                _cellImage.color = Color.magenta;
                break;
        }
    }
}
