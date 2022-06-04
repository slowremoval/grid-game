using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridConstructor : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    [Space] [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _cellConstructPrefab;
    [SerializeField] private GameObject _cellNodePrefab;
    [SerializeField] private GameObject _cellEmptyPrefab;
    [Space] private Vector2 _gridSize;
    private Vector2 _cellSize;
    private Vector2 _spacing;

    private GridCell[,] _cells;

    private RectOffset _padding;

    private int _colsCount;

    private int _rowsCount;
    [Space] [SerializeField] private string _levelName;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
#if UNITY_EDITOR
            Selection.activeObject = gameObject;
#endif
        }
    }

    #region Constructor Navigation

    public void StartGridBuilding()
    {
        ClearGrid();

        _gridLayoutGroup.enabled = true;

        GenerateGrid(_cellConstructPrefab, _rowsCount, _colsCount);

        StartCoroutine(DisableGridLayoutRoutine());
    }

    public void StartSaveRedacting()
    {
        StartGridBuilding();
        SaveData loadedLvl = LoadLevel(_levelName);

        if (loadedLvl is null)
        {
            return;
        }

        ContinueLevelRedacting(loadedLvl);
    }

    private void ContinueLevelRedacting(SaveData loadedLvl)
    {
        int count = 0;

        for (int i = 0; i < _rowsCount; i++)
        {
            for (int j = 0; j < _colsCount; j++)
            {
                GridCellConstruct currentCell = _cells[i, j] as GridCellConstruct;

                var temp = _cells[i, j].GetComponent<GridCellConstruct>();


                temp.SetCellType(loadedLvl.CellTypes[count]);
                count++;
            }
        }
    }

    public void StartLevel()
    {
        StartGridBuilding();
        SaveData loadedLvl = LoadLevel(_levelName);

        if (loadedLvl is null)
        {
            return;
        }

        InstantiateLoadedGrid(loadedLvl);
    }

    public SaveData LoadLevel(string levelNumber)
    {
        string loadPath = Application.dataPath + string.Format($"/Resources/Levels/Level_{levelNumber}.json");

        if (!File.Exists(loadPath))
        {
            Debug.Log($"{loadPath} doesn`t exist!");
            return null;
        }

        string saveContent = File.ReadAllText(loadPath);

        SaveData lvl = JsonUtility.FromJson<SaveData>(saveContent);

        return lvl;
    }

    public void SaveLevel()
    {
        SaveData currentLevel = new SaveData();
        currentLevel.SetSaveData(_cells);

        string json = JsonUtility.ToJson(currentLevel);

        string path = Application.dataPath + string.Format($"/Resources/Levels/Level_{_levelName}.json");

        File.WriteAllText(path, json);
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

    private IEnumerator DisableGridLayoutRoutine()
    {
        yield return new WaitForEndOfFrame();
        _gridLayoutGroup.enabled = false;
    }

    private void SetGridProperties(int rowsCount, int colsCount) => _cells = new GridCell[rowsCount, colsCount];

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
    }

    private void GenerateGrid(GameObject cellPrefab, int rowsCount, int colsCount)
    {
        SetGridProperties(_rowsCount, _colsCount);

        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < colsCount; j++)
            {
                GameObject instantiatedCell = Instantiate(cellPrefab, transform);

                AddCellToArray(instantiatedCell, i, j);
                _cells[i, j].name = $"Cell [{i}, {j}]";
                _cells[i, j].Coordinates = new Vector2(i, j);
            }
        }
    }

    private void InstantiateLoadedGrid(SaveData lvl)
    {
        ClearGrid();

        SetGridProperties(_rowsCount, _colsCount);

        int count = 0;

        _gridLayoutGroup.enabled = true;

        for (int i = 0; i < _rowsCount; i++)
        {
            for (int j = 0; j < _colsCount; j++)
            {
                GameObject gridCell;
                CellNode node;

                switch (lvl.CellTypes[count])
                {
                    case GridCellConstruct.CellType.simple:
                        gridCell = Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity, transform);
                        AddCellToArray(gridCell, i, j);
                        break;
                    case GridCellConstruct.CellType.dark:
                        gridCell = Instantiate(_cellNodePrefab, Vector3.zero, Quaternion.identity, transform);
                        AddCellToArray(gridCell, i, j);
                        node = gridCell.GetComponent<CellNode>();
                        node.ThisNodeType = CellNode.NodeType.dark;
                        node.requiredAmount = lvl.RequiredAmounts[count];
                        break;
                    case GridCellConstruct.CellType.light:
                        gridCell = Instantiate(_cellNodePrefab, Vector3.zero, Quaternion.identity, transform);
                        AddCellToArray(gridCell, i, j);
                        node = gridCell.GetComponent<CellNode>();
                        node.ThisNodeType = CellNode.NodeType.light;
                        node.requiredAmount = lvl.RequiredAmounts[count];
                        break;
                    case GridCellConstruct.CellType.empty:
                        gridCell = Instantiate(_cellEmptyPrefab, Vector3.zero, Quaternion.identity, transform);
                        AddCellToArray(gridCell, i, j);
                        break;
                }

                count++;

                _cells[i, j].name = $"Cell [{i}, {j}]";
                _cells[i, j].Coordinates = new Vector2(i, j);
            }
        }

        StartCoroutine(DisableGridLayoutRoutine());
        StartCoroutine(DeactivateEmptyCells());
    }

    private IEnumerator DeactivateEmptyCells()
    {
        yield return new WaitForEndOfFrame();
        foreach (GridCell cell in _cells)
        {
            if (cell.TryGetComponent<EmptyCell>(out _))
            {
                cell.gameObject.SetActive(false);
            }
        }
    }

    private void AddCellToArray(GameObject gridCell, int i, int j) => _cells[i, j] = gridCell.GetComponent<GridCell>();

    private void ClearGrid()
    {
        if (_cells is null || _cells.Length == 0)
        {
            return;
        }

        foreach (var cell in _cells)
        {
            Destroy(cell.gameObject);
        }
    }
}

public class SaveData
{
    public Vector2[] Coordinates;
    public int[] RequiredAmounts;
    public GridCellConstruct.CellType[] CellTypes;

    public void SetSaveData(GridCell[,] grid)
    {
        Coordinates = new Vector2[grid.Length];
        CellTypes = new GridCellConstruct.CellType[grid.Length];
        RequiredAmounts = new int[grid.Length];

        int count = 0;

        foreach (GridCell item in grid)
        {
            if (!(item is GridCellConstruct newItem))
            {
                return;
            }

            Coordinates[count] = newItem.Coordinates;
            CellTypes[count] = newItem.ThisCellType;
            RequiredAmounts[count] = newItem.requiredAmount;

            count++;
        }
    }
}