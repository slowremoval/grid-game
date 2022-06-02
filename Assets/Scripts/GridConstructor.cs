using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridConstructor : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    
    [Space] [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _cellConstructPrefab;
    [SerializeField] private GameObject _cellNodePrefab;
    [Space] private Vector2 _gridSize;
    private Vector2 _cellSize;
    private Vector2 _spacing;

    private GridCell[,] _cells;

    private RectOffset _padding;

    private int _colsCount;

    private int _rowsCount;
    [SerializeField]
    private string _saveLevelNumber;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _gridLayoutGroup.enabled = false;
    }

    #region Constructor Navigation

    public void StartGridBuilding()
    {
        ClearGrid();

        _gridLayoutGroup.enabled = true;

        GenerateGrid(_cellConstructPrefab, _rowsCount, _colsCount);
    }

    public void FinishGridBuilding()
    {
        //todo
    }

    public void LoadLevel()
    {
        
    }
    
    public void SaveLevel()
    {
        Debug.Log($"Trying to save...");

        SaveData currentLevel = new SaveData();
        currentLevel.SetSaveData(_cells);
        
        string json = JsonUtility.ToJson(currentLevel);

        string path = Application.dataPath + string.Format($"/Resources/Levels/Level_{_saveLevelNumber}.json");

        File.WriteAllText(path, json);
        
        Debug.Log($"Saved!");
    }

    #endregion

    private void Start()
    {
        GetGridProperties(out int colsCount, out int rowsCount);

        _colsCount = colsCount;
        _rowsCount = rowsCount;

        SetGridProperties(_rowsCount, _colsCount);

        GenerateGrid(_cellPrefab, rowsCount, colsCount);
    }

    private void SetGridProperties(int rowsCount, int colsCount)
    {
        _cells = new GridCell[rowsCount, colsCount];
    }

    private void GetGridProperties(out int colsCount, out int rowsCount)
    {
        _gridSize = _rectTransform.sizeDelta;

        _cellSize = _gridLayoutGroup.cellSize;
        _spacing = _gridLayoutGroup.spacing;
        _padding = _gridLayoutGroup.padding;

        float cellHeight = _cellSize.y + _spacing.y;
        float gridHeight = _gridSize.y - _padding.vertical;
        float gridWidth = _gridSize.x - _padding.horizontal;

        rowsCount = (int)(Mathf.Round(gridHeight / cellHeight));
        colsCount = (int)(Mathf.Round(gridWidth / cellHeight));

        //Debug.Log($"grid size is {colsCount} x {rowsCount}");
    }

    private void GenerateGrid(GameObject cellPrefab, int rowsCount, int colsCount)
    {
        SetGridProperties(_rowsCount, _colsCount);
        
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                _cells[i, j] = Instantiate(cellPrefab, transform).GetComponent<GridCell>();
                _cells[i, j].name = $"Cell [{i}, {j}]";
                _cells[i, j].Coordinates = new Vector2(i, j);
            }
        }
    }

    private void ClearGrid()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell.gameObject);
        }
    }
}

public class SaveData
{
    public Vector2[] Coordinates;
    public bool[] IsActive;
    public int[] RequiredAmounts;
    public GridCellConstruct.CellType[] CellTypes;

    public void SetSaveData(GridCell[,] grid)
    {
        Coordinates = new Vector2[grid.Length];
        CellTypes = new GridCellConstruct.CellType[grid.Length];
        IsActive = new bool[grid.Length];
        RequiredAmounts = new int[grid.Length];

        int count = 0;
        foreach (GridCell item in grid)
        {
            var newItem = item as GridCellConstruct;
            
            Coordinates[count] = newItem.Coordinates;
            CellTypes[count] = newItem.ThisCellType;
            IsActive[count] = newItem.gameObject.active;
            RequiredAmounts[count] = newItem.requiredAmount;
            count++;
        }
    }
}