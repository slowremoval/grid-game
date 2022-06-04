using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class CellNode : GridCell
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private GameObject _visualization;

        [HideInInspector] public int requiredAmount;
        //[HideInInspector] public int currentAmount;

        [HideInInspector] public Grid GridData;
        
        private CellType _nodeType;
        public CellType NodeType
        {
            get => _nodeType;
            set
            {
                if (value == CellType.empty || value == CellType.simple)
                {
                    return;
                }
                _nodeType = value;
            }
        }

        private Image _nodeVisualization;
        
        
        public void CheckNeighbours()
        { 
            int neighboursCount = 0;
            neighboursCount += CheckHorizontal(); 
            neighboursCount += CheckVertical();
            
            ShowNodeRequirements(neighboursCount);
        }

        private int CheckVertical()
        {
            int count = 0;
            
            count = CheckUpperSide(count);
            
            count = CheckUnderSide(count);
        
            return count;
        }

        private int CheckUnderSide(int count)
        {
            int step = 1;

            foreach (var simpleCell in GridData.SimpleCells)
            {
                if (simpleCell.Coordinates.y != Coordinates.y)
                {
                    continue;
                }

                if (simpleCell.Coordinates.x <= Coordinates.x)
                {
                    continue;
                }
                
                int temp = CheckSide(
                    simpleCell.CellColor,
                    (int)simpleCell.Coordinates.x,
                    (int)Coordinates.x,
                    step);

                if (temp == 0)
                {
                    break;
                }

                count += temp;
                step++;
            }

            return count;
        }

        private int CheckUpperSide(int count)
        {
            int step = 1;

            for (int index = GridData.SimpleCells.Count - 1; index >= 0; index--)
            {
                var simpleCell = GridData.SimpleCells[index];
                
                if (simpleCell.Coordinates.y != Coordinates.y)
                {
                    continue;
                }

                if (simpleCell.Coordinates.x >= Coordinates.x)
                {
                    continue;
                }

                int temp = CheckSide(
                    simpleCell.CellColor,
                    (int)simpleCell.Coordinates.x,
                    (int)Coordinates.x,
                    -step);

                if (temp == 0)
                {
                    break;
                }

                count += temp;
                step++;
            }

            return count;
        }

        private int CheckHorizontal()
        {
            int count = 0;
            
            count = CheckRightSide(count);
            
            count = CheckLeftSide(count);
        
            return count;
        }

        private int CheckLeftSide(int count)
        {
            int step = 1;

            for (int index = GridData.SimpleCells.Count - 1; index >= 0; index--)
            {
                var simpleCell = GridData.SimpleCells[index];
                
                if (simpleCell.Coordinates.x != Coordinates.x)
                {
                    continue;
                }

                if (simpleCell.Coordinates.y >= Coordinates.y)
                {
                    continue;
                }

                Debug.Log(
                    $"step : {step}, cellCoordinate  : {simpleCell.Coordinates.y}, nodeCoordinate : {(int)Coordinates.y}");

                int temp = CheckSide(
                    simpleCell.CellColor,
                    (int)simpleCell.Coordinates.y,
                    (int)Coordinates.y,
                    -step);

                if (temp == 0)
                {
                    break;
                }

                count += temp;
                step++;
            }

            return count;
        }

        private int CheckRightSide(int count)
        {
            int step = 1;

            foreach (var simpleCell in GridData.SimpleCells)
            {
                if (simpleCell.Coordinates.x != Coordinates.x)
                {
                    continue;
                }

                if (simpleCell.Coordinates.y <= Coordinates.y)
                {
                    continue;
                }
                
                Debug.Log($"step : {step}, cellCoordinate  : {simpleCell.Coordinates.y}, nodeCoordinate : {(int)Coordinates.y}");
                
                int temp = CheckSide(
                    simpleCell.CellColor,
                    (int)simpleCell.Coordinates.y,
                    (int)Coordinates.y,
                    step);

                if (temp == 0)
                {
                    break;
                }

                count += temp;
                step++;
            }

            return count;
        }

        private int CheckSide(CellType cellType, int cellCoordinate, int nodeCoordinate , int step)
        {
            if (cellType != _nodeType) return 0;
            
            if (cellCoordinate == nodeCoordinate) return 0;
            
            int count = 0;
            
            if (cellCoordinate - step == nodeCoordinate)
            {
                count++;
            }
            
            return count;
        }


        private void Start()
        {
            if (_textMeshProUGUI == null)
            {
                return;
            }

            _nodeVisualization = _visualization.GetComponent<Image>();
            UpdateVisualization();
        }

        private void UpdateVisualization()
        {
            ShowNodeRequirements(0);

            SwitchNodeType();
        }

        private void ShowNodeRequirements(int currentAmount)
        {
            _textMeshProUGUI.text = $"{currentAmount}/{requiredAmount}";
        }

        private void SwitchNodeType()
        {
            switch (NodeType)
            {
                case CellType.dark:
                    _nodeVisualization.color = Color.gray;
                    break;
                case CellType.light:
                    _nodeVisualization.color = Color.yellow;
                    break;
                default:
                    _nodeVisualization.color = Color.magenta;
                    break;
            }
        }
    }
}