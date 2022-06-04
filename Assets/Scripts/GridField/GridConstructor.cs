using System.Collections;
using System.Collections.Generic;
using System.IO;
using GridField.Cells;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridField
{
    public class GridConstructor : Grid, IPointerDownHandler
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

        private RectOffset _padding;

        [Space] [SerializeField] private string _levelName;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
#if UNITY_EDITOR
                Selection.activeObject = gameObject;
#endif
            }
        }

        #region Constructor Navigation

        public void StartGridBuilding()
        {
            DesubscribeToSimpleCellsChanging();
            ClearGrid();

            _gridLayoutGroup.enabled = true;

            GenerateGrid(_cellConstructPrefab, _rowsCount, _colsCount);

            StartCoroutine(DisableGridLayoutRoutine());
        }

        public void StartSaveRedacting()
        {
            ClearGrid();
            DesubscribeToSimpleCellsChanging();
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
            DesubscribeToSimpleCellsChanging();
            int count = 0;

            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    GridCellConstruct currentCell = _cells[i, j].GetComponent<GridCellConstruct>();

                    currentCell.SetCellType(loadedLvl.CellTypes[count]);
                    count++;
                }
            }
        }

        public void StartLevel()
        {
            DesubscribeToSimpleCellsChanging();
            StartGridBuilding();
            SaveData loadedLvl = LoadLevel(_levelName);

            if (loadedLvl is null)
            {
                return;
            }

            InstantiateLoadedGrid(loadedLvl);
            SubscribeToSimpleCellsChanging();
        }

        private void SubscribeToSimpleCellsChanging()
        {
            foreach (CellNode node in Nodes)
            {
                foreach (SimpleCell cell in SimpleCells)
                {
                    cell.OnColorChanged += node.CheckNeighbours;
                }
            }
        }

        private void DesubscribeToSimpleCellsChanging()
        {
            foreach (CellNode node in Nodes)
            {
                foreach (SimpleCell cell in SimpleCells)
                {
                    cell.OnColorChanged -= node.CheckNeighbours;
                }
            }
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

            SimpleCells = new List<SimpleCell>();
            Nodes = new List<CellNode>();
            
            int count = 0;

            _gridLayoutGroup.enabled = true;

            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    GameObject gridCell;
                    CellNode node;
                    SimpleCell simpleCell;

                    switch (lvl.CellTypes[count])
                    {
                        case CellType.simple:
                            gridCell = Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity, transform);
                            AddCellToArray(gridCell, i, j);
                            simpleCell = gridCell.GetComponent<SimpleCell>();
                            SimpleCells.Add(simpleCell);
                            Debug.Log($"_simple cells : {SimpleCells.Count}");
                            break;
                        case CellType.dark:
                            gridCell = Instantiate(_cellNodePrefab, Vector3.zero, Quaternion.identity, transform);
                            AddCellToArray(gridCell, i, j);
                            node = gridCell.GetComponent<CellNode>();
                            node.NodeType = CellType.dark;
                            node.requiredAmount = lvl.RequiredAmounts[count];
                            node.GridData = this;
                            Nodes.Add(node);
                            break;
                        case CellType.light:
                            gridCell = Instantiate(_cellNodePrefab, Vector3.zero, Quaternion.identity, transform);
                            AddCellToArray(gridCell, i, j);
                            node = gridCell.GetComponent<CellNode>();
                            node.NodeType = CellType.light;
                            node.requiredAmount = lvl.RequiredAmounts[count];
                            node.GridData = this;
                            Nodes.Add(node);
                            break;
                        case CellType.empty:
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

        private void AddCellToArray(GameObject gridCell, int i, int j) =>
            _cells[i, j] = gridCell.GetComponent<GridCell>();

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
}