using System.Collections;
using System.Collections.Generic;
using System.IO;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class GridConstructor : Grid
    {
        [Header("Grid Size")] [Range(0, 17)] [SerializeField]
        public int _colsCount;

        [Range(0, 11)] [SerializeField] public int _rowsCount;

        [Space] [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private GameObject _cellConstructPrefab;
        [SerializeField] private GameObject _cellNodePrefab;
        [SerializeField] private GameObject _cellEmptyPrefab;
        [SerializeField] private GameObject _cellRotatingPrefab;
        [SerializeField] private GameObject _cellStablePrefab;
        [SerializeField] private GameObject _cellUniversalPrefab;
        [SerializeField] private GameObject _cellCountingPrefab;

        [Space] [SerializeField] private string _levelName;

        private float _spacing = 42;

        private float _cellSize;

        #region Constructor Navigation

        public void StartGridBuilding()
        {
            DesubscribeToSimpleCellsChanging();
            ClearGrid();

            GenerateGrid(_cellConstructPrefab);
        }

        public void StartSaveRedacting()
        {
            SaveData loadedLvl = LoadLevel(_levelName);

            if (loadedLvl is null)
            {
                return;
            }

            ContinueLevelRedacting(loadedLvl);
            SetGridCenter();
        }

        public void StartLevel()
        {
            DesubscribeToSimpleCellsChanging();
            _activeGridElements = new List<GridCell>();
            SaveData loadedLvl = LoadLevel(_levelName);

            if (loadedLvl is null)
            {
                return;
            }

            InstantiateLoadedGrid(loadedLvl);
            SubscribeToSimpleCellsChanging();
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
            currentLevel.SetSaveData(_allGridElements);

            string json = JsonUtility.ToJson(currentLevel);

            string path = Application.dataPath + string.Format($"/Resources/Levels/Level_{_levelName}.json");

            File.WriteAllText(path, json);
        }

        #endregion

        private void ContinueLevelRedacting(SaveData loadedLvl)
        {
            DesubscribeToSimpleCellsChanging();
            ClearGrid();
            SetGridProperties((int)loadedLvl.GridSize.x, (int)loadedLvl.GridSize.y);

            GenerateGrid(_cellConstructPrefab);
            int count = 0;

            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    GridCellConstruct currentCell = _allGridElements[i, j].GetComponent<GridCellConstruct>();
                    currentCell.SetCellType(loadedLvl.CellTypes[count]);
                    currentCell.SetRequiredAmount(loadedLvl.CelCapacities[count]);
                    currentCell.SetSidesProperties(loadedLvl.UnactiveSidesVector[count], currentCell);
                    currentCell.ShowUnactiveSides();
                    count++;
                }
            }
        }

        private void DesubscribeToSimpleCellsChanging()
        {
            foreach (GridCell cell in Cells)
            {
                if (cell is SimpleCell simpleCell)
                {
                    simpleCell.OnColorChanged -= base.RecalculateNeighbourAxises;
                }
            }
        }

        private void SubscribeToSimpleCellsChanging()
        {
            foreach (GridCell cell in Cells)
            {
                if (cell is SimpleCell simpleCell)
                {
                    simpleCell.OnColorChanged += base.RecalculateNeighbourAxises;
                }
            }
        }

        private void SetGridProperties(int rowsCount, int colsCount)
        {
            _allGridElements = new GridCell[rowsCount, colsCount];
            _colsCount = colsCount;
            _rowsCount = rowsCount;
        }

        private void Awake()
        {
            _cellSize = _cellPrefab.GetComponent<RectTransform>().sizeDelta.x;
        }

        private void GenerateGrid(GameObject cellPrefab)
        {
            SetGridProperties(_rowsCount, _colsCount);

            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    var cell = InstantiateCell(cellPrefab, i, j);

                    if (cell.TryGetComponent<GridCell>(out var gridCell))
                    {
                        gridCell.InitializeGrid();
                    }
                }
            }

            SetGridCenter();
        }

        private GridCell InstantiateCell(GameObject cellPrefab, int i, int j)
        {
            GameObject instantiatedCellGameObject =
                Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

            if (instantiatedCellGameObject.gameObject.TryGetComponent(out GridCell instantiatedCell) == false)
            {
                return default;
            }

            AddCellToArray(instantiatedCell, i, j);

            RectTransform instantiatedCellRect = instantiatedCell.transform as RectTransform;

            instantiatedCellRect.anchoredPosition =
                new Vector2(
                    j * (_cellSize + _spacing),
                    i * (_cellSize + _spacing));
            _allGridElements[i, j] = instantiatedCell;
            _allGridElements[i, j].Coordinates = new Vector2(i, j);
            _allGridElements[i, j].name = $"Cell [{i}, {j}]";

            return instantiatedCell;
        }

        private void SetGridCenter()
        {
            RectTransform rectTransform = gameObject.transform as RectTransform;
            float posX;
            float posY;

            float cellSize = _cellPrefab.GetComponent<RectTransform>().sizeDelta.x + _spacing;
            if (_allGridElements.GetLength(0) % 2 != 0)
                posX = -(_allGridElements.GetLength(0) - 1) * cellSize / 2;
            else
            {
                posX = -(_allGridElements.GetLength(0) - 1) * cellSize / 2;
            }

            if (_allGridElements.GetLength(1) % 2 != 0)
                posY = -(_allGridElements.GetLength(1) - 1) * cellSize / 2;
            else
            {
                posY = -(_allGridElements.GetLength(1) - 1) * cellSize / 2;
            }

            rectTransform.anchoredPosition = new Vector2(posY, posX);
        }

        private void InstantiateLoadedGrid(SaveData lvl)
        {
            ClearGrid();
            SetGridProperties((int)lvl.GridSize.x, (int)lvl.GridSize.y);

            Cells = new List<GridCell>();
            Nodes = new List<CellNode>();

            int count = 0;

            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    switch (lvl.CellTypes[count])
                    {
                        case CellType.simple:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellPrefab, Cells);
                            break;
                        case CellType.dark:
                            ConfigureCell<CellNode>(lvl, i, j, count, _cellNodePrefab, Nodes, CellType.dark);
                            break;
                        case CellType.light:
                            ConfigureCell<CellNode>(lvl, i, j, count, _cellNodePrefab, Nodes, CellType.light);
                            break;
                        case CellType.empty:
                            InstantiateCell(_cellEmptyPrefab, i, j);
                            break;
                        case CellType.rotating:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellRotatingPrefab, Cells);
                            break;
                        case CellType.stableDark:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellStablePrefab, Cells,
                                CellType.stableDark);
                            break;
                        case CellType.stableLight:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellStablePrefab, Cells,
                                CellType.stableLight);
                            break;
                        case CellType.universal:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellUniversalPrefab, Cells,
                                CellType.universal);
                            break;
                        case CellType.counting:
                            ConfigureCell<GridCell>(lvl, i, j, count, _cellCountingPrefab, Cells,
                                CellType.counting);
                            break;
                    }

                    count++;

                    _allGridElements[i, j].name = $"Cell [{i}, {j}]";
                    _allGridElements[i, j].Coordinates = new Vector2(i, j);
                }
            }

            SetGridCenter();
            StartCoroutine(DeactivateEmptyCells());

            StartCoroutine(StartNodesCheck(1));
        }

        private IEnumerator StartNodesCheck(int framesToWait = 1)
        {
            for (int i = 0; i < framesToWait; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (var node in Nodes)
            {
                CheckNeighbours(node);
            }
        }


        private void ConfigureCell<T>(SaveData lvl, int i, int j, int count, GameObject cellPrefab, List<T> listToAdd,
            CellType type = CellType.simple) where T : GridCell
        {
            GridCell currentCell = InstantiateCell(cellPrefab, i, j);

            if (!(currentCell is T cellGeneric)) return;

            cellGeneric.CellType = type;
            cellGeneric.SetSidesProperties(lvl.UnactiveSidesVector[count], cellGeneric);
            cellGeneric.GridData = this;
            cellGeneric.Capacity = lvl.CelCapacities[count];

            listToAdd.Add(cellGeneric);
            _activeGridElements.Add(cellGeneric);
        }


        private IEnumerator DeactivateEmptyCells()
        {
            yield return new WaitForEndOfFrame();
            foreach (GridCell cell in _allGridElements)
            {
                if (cell.TryGetComponent<EmptyCell>(out _))
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }

        private void AddCellToArray(GridCell gridCell, int i, int j) =>
            _allGridElements[i, j] = gridCell;

        private void ClearGrid()
        {
            if (_allGridElements is null || _allGridElements.Length == 0)
            {
                return;
            }

            foreach (var cell in _allGridElements)
            {
                Destroy(cell.gameObject);
            }
        }
    }
}