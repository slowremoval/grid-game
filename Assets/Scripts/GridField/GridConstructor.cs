using System.Collections.Generic;
using System.IO;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class GridConstructor : Grid
    {
        [Header("Grid Size")]
        [Range(0, 17)][SerializeField] public int _colsCount;
        [Range(0, 11)][SerializeField] public int _rowsCount;

        [Space] [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private GameObject _cellConstructPrefab;
        [SerializeField] private GameObject _cellNodePrefab;
        [SerializeField] private GameObject _cellEmptyPrefab;
        [SerializeField]private float _spacing = 20;

        [Space] [SerializeField] private string _levelName;

        
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
                    GridCellConstruct currentCell = _cells[i, j].GetComponent<GridCellConstruct>();
                    currentCell.SetCellType(loadedLvl.CellTypes[count]);
                    currentCell.SetRequiredAmount(loadedLvl.RequiredAmounts[count]);
                    count++;
                }
            }
        }

        public void StartLevel()
        {
            DesubscribeToSimpleCellsChanging();
            
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
            currentLevel.SetSaveData(_cells);

            string json = JsonUtility.ToJson(currentLevel);

            string path = Application.dataPath + string.Format($"/Resources/Levels/Level_{_levelName}.json");

            File.WriteAllText(path, json);
        }

        #endregion

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

        private void SetGridProperties(int rowsCount, int colsCount)
        {
            _cells = new GridCell[rowsCount, colsCount];
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
                    InstantiateCell(cellPrefab, i, j);
                }
            }

            SetGridCenter();
        }

        private GameObject InstantiateCell(GameObject cellPrefab, int i, int j)
        {
            GameObject instantiatedCell = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            AddCellToArray(instantiatedCell, i, j);
            RectTransform instantiatedCellRect = instantiatedCell.transform as RectTransform;
            instantiatedCellRect.anchoredPosition =
                new Vector2(
                    j * (_cellSize + _spacing),
                    i * (_cellSize + _spacing));
            _cells[i, j] = instantiatedCell.GetComponent<GridCell>();
            _cells[i, j].Coordinates = new Vector2(i, j);
            _cells[i, j].name = $"Cell [{i}, {j}]";
            
            return instantiatedCell;
        }

        private void SetGridCenter()
        {
            RectTransform rectTransform = gameObject.transform as RectTransform;
            float posX;
            float posY;
            
            float cellSize = _cellPrefab.GetComponent<RectTransform>().sizeDelta.x + _spacing;
            if (_cells.GetLength(0) % 2 != 0)
                posX = -(_cells.GetLength(0) - 1) * cellSize / 2;
            else
            {
                posX = -(_cells.GetLength(0) - 1) * cellSize / 2;
            }

            if (_cells.GetLength(1) % 2 != 0)
                posY = -(_cells.GetLength(1) - 1) * cellSize / 2;
            else
            {
                posY = -(_cells.GetLength(1) - 1) * cellSize / 2;
            }

            rectTransform.anchoredPosition = new Vector2(posY, posX);
        }
        
        private void InstantiateLoadedGrid(SaveData lvl)
        {
            ClearGrid();
            SetGridProperties((int)lvl.GridSize.x, (int)lvl.GridSize.y);

            SimpleCells = new List<SimpleCell>();
            Nodes = new List<CellNode>();
            
            int count = 0;
            
            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _colsCount; j++)
                {
                    switch (lvl.CellTypes[count])
                    {
                        case CellType.simple:
                            ConfigureSimpleCell(lvl, i, j, count);
                            break;
                        case CellType.dark:
                            ConfigureNode(lvl, i, j, count, CellType.dark);
                            break;
                        case CellType.light:
                            ConfigureNode(lvl, i, j, count, CellType.light);
                            break;
                        case CellType.empty:
                            InstantiateCell(_cellEmptyPrefab, i, j);
                            break;
                    }
                    
                    count++;

                    _cells[i, j].name = $"Cell [{i}, {j}]";
                    _cells[i, j].Coordinates = new Vector2(i, j);
                }
            }
            SetGridCenter();
            DeactivateEmptyCells();
        }

        private void ConfigureSimpleCell(SaveData lvl, int i, int j, int count)
        {
            GameObject currentCell = InstantiateCell(_cellPrefab, i, j);
            SimpleCell simpleCell = currentCell.GetComponent<SimpleCell>();
            simpleCell.UnactiveSide = lvl.UnactiveSide[count];
            SimpleCells.Add(simpleCell);
        }

        private void ConfigureNode(SaveData lvl, int i, int j, int count, CellType type)
        {
            GameObject currentCell = InstantiateCell(_cellNodePrefab, i, j);
            CellNode node = currentCell.GetComponent<CellNode>();
            node.NodeType = type;
            node.requiredAmount = lvl.RequiredAmounts[count];
            node.UnactiveSide = lvl.UnactiveSide[count];
            node.GridData = this;
            Nodes.Add(node);
        }

        private void DeactivateEmptyCells()
        {
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