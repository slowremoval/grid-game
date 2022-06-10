using System.Linq;
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

        [HideInInspector] public int CurrentAmount;


        private Image _nodeVisualization;

        public void CheckNeighbours()
        {
            int neighboursCount = 0;

            neighboursCount += CheckHorizontal();
            neighboursCount += CheckVertical();

            CurrentAmount = neighboursCount;

            ShowNodeRequirements(CurrentAmount);
        }

        private int CheckVertical()
        {
            int count = 0;

            count = CheckUpperSide(count);

            count = CheckUnderSide(count);

            return count;
        }

        private int CheckHorizontal()
        {
            int rightNeighbours = CheckRightSide();

            int leftNeighbours = CheckLeftSide();

            int count = leftNeighbours + rightNeighbours;

            return count;
        }

        private int CheckUnderSide(int count)
        {
            int step = 1;

            bool isCountActive = true;
            SimpleCell _previousCell = default;

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

                if (simpleCell.UnactiveSides.Contains(CellSide.down))
                {
                    Debug.Log($"top is false!");
                    isCountActive = false;
                }

                if (step > 1 && _previousCell.UnactiveSides.Contains(CellSide.top))
                {
                    isCountActive = false;
                }

                int temp = CheckAxisCell(
                    simpleCell.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)Coordinates.x,
                    step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.down, CellSide.top, true);
                }
                else
                {
                    if (step > 1 && Mathf.Abs(simpleCell.Coordinates.x - _previousCell.Coordinates.x) > 1)
                    {
                        break;
                    }

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.down, CellSide.top, false);
                }

                _previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private int CheckUpperSide(int count)
        {
            int step = 1;

            bool isCountActive = true;
            SimpleCell _previousCell = default;

            for (int index = GridData.SimpleCells.Count - 1; index >= 0; index--)
            {
                SimpleCell simpleCell = GridData.SimpleCells[index];
                
                if (simpleCell.Coordinates.y != Coordinates.y)
                {
                    continue;
                }

                if (simpleCell.Coordinates.x >= Coordinates.x)
                {
                    continue;
                }

                if (simpleCell.UnactiveSides.Contains(CellSide.top))
                {
                    isCountActive = false;
                }

                if (step > 1 && _previousCell.UnactiveSides.Contains(CellSide.down))
                {
                    isCountActive = false;
                }

                int temp = CheckAxisCell(
                    simpleCell.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)Coordinates.x,
                    -step);

                if (temp == 0)
                {
                    isCountActive = false;
                }


                if (isCountActive)
                {
                    count += temp;

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.top, CellSide.down, true);
                }
                else
                {
                    if (step > 1 && Mathf.Abs(simpleCell.Coordinates.x - _previousCell.Coordinates.x) > 1)
                    {
                        break;
                    }

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.top, CellSide.down, false);
                }

                _previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private static void ConnectionBetweenCellsSetActive(SimpleCell simpleCell, SimpleCell _previousCell,
            CellSide first, CellSide second, bool setActive)
        {
            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, first);
            _previousCell?.CellConnectionProvider.ConnectionSetActive(setActive, second);
        }

        private int CheckLeftSide()
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            SimpleCell _previousCell = default;

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

                if (simpleCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }

                if (step > 1 && _previousCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                int temp = CheckAxisCell(
                    simpleCell.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)Coordinates.y,
                    -step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.right, CellSide.left, true);
                }
                else
                {
                    if (step > 1 && Mathf.Abs(simpleCell.Coordinates.y - _previousCell.Coordinates.y) > 1)
                    {
                        break;
                    }

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.right, CellSide.left, false);
                }

                step++;
                _previousCell = simpleCell;
            }

            return count;
        }

        private int CheckRightSide()
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            SimpleCell _previousCell = default;

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

                if (simpleCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                if (step > 1 && _previousCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }


                int temp = CheckAxisCell(
                    simpleCell.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)Coordinates.y,
                    step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;
                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.left, CellSide.right, true);
                }
                else
                {
                    if (step > 1 && Mathf.Abs(simpleCell.Coordinates.y - _previousCell.Coordinates.y) > 1)
                    {
                        break;
                    }

                    ConnectionBetweenCellsSetActive(simpleCell, _previousCell, CellSide.left, CellSide.right, false);
                }

                step++;

                _previousCell = simpleCell;
            }

            return count;
        }


        private int CheckAxisCell(CellType cellType, int cellCoordinate, int nodeCoordinate, int step = 1)
        {
            if (cellType != CellType) return 0;

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
            switch (CellType)
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