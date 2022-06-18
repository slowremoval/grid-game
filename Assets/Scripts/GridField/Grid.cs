using System.Collections.Generic;
using System.Linq;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class Grid : MonoBehaviour
    {
        [HideInInspector] public List<CellNode> Nodes;
        [HideInInspector] public List<GridCell> Cells;

        public GridCell[,] _allGridElements;
        public List<GridCell> _activeGridElements = new List<GridCell>();


        private List<CellNode> currentNodes;

        #region NodeNeighbours

        public void RecalculateNeighbourAxises(GridCell gridCell)
        {
            DeactivateVerticalAndHorizontalConnections(gridCell);
            FindCurrentNodes(gridCell);


            foreach (CellNode node in currentNodes)
            {
                CheckNeighbours(node);
            }
        }

        private void CheckNeighbours(CellNode node)
        {
            int neighboursCount = 0;
            neighboursCount += CheckHorizontal(node);
            neighboursCount += CheckVertical(node);

            node.CurrentAmount = neighboursCount;

            node.UpdateVisualization();
        }

        private void DeactivateVerticalAndHorizontalConnections(GridCell gridCell)
        {
            foreach (GridCell cell in _activeGridElements)
            {
                if ((int)cell.Coordinates.x != (int)gridCell.Coordinates.x)
                {
                    continue;
                }
                cell.CellConnectionProvider.DeactivateHorizontalConnections();
            }
            foreach (GridCell cell in _activeGridElements)
            {
                if ((int)cell.Coordinates.y != (int)gridCell.Coordinates.y)
                {
                    continue;
                }
                cell.CellConnectionProvider.DeactivateVerticalConnections();
            }
        }

        private void FindCurrentNodes(GridCell gridCell)
        {
            currentNodes = new List<CellNode>();

            foreach (CellNode node in Nodes)
            {
                if ((int)node.Coordinates.x == (int)gridCell.Coordinates.x ||
                    (int)node.Coordinates.y == (int)gridCell.Coordinates.y)
                {
                    currentNodes.Add(node);
                }
            }
        }


        private int CheckVertical(CellNode cellNode)
        {
            int count = 0;

            count += CheckUpperSide(cellNode);

            count += CheckUnderSide(cellNode);

            return count;
        }

        private int CheckHorizontal(CellNode cellNode)
        {
            int rightNeighbours = CheckRightSide(cellNode);

            int leftNeighbours = CheckLeftSide(cellNode);

            int count = leftNeighbours + rightNeighbours;

            return count;
        }

        private int CheckUnderSide(CellNode cellNode)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell _previousCell = cellNode;

            foreach (var simpleCell in Cells)
            {
                if ((int)simpleCell.Coordinates.y != (int)cellNode.Coordinates.y)
                {
                    continue;
                }

                if (simpleCell.Coordinates.x <= cellNode.Coordinates.x)
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
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)cellNode.Coordinates.x,
                    step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;
                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, _previousCell, CellSide.down,
                        CellSide.top, true);
                }

                _previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private int CheckUpperSide(CellNode cellNode)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell _previousCell = cellNode;

            for (int index = Cells.Count - 1; index >= 0; index--)
            {
                GridCell simpleCell = Cells[index];

                if ((int)simpleCell.Coordinates.y != (int)cellNode.Coordinates.y)
                {
                    continue;
                }

                if (simpleCell.Coordinates.x >= cellNode.Coordinates.x)
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
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)cellNode.Coordinates.x,
                    -step);

                if (temp == 0)
                {
                    isCountActive = false;
                }


                if (isCountActive)
                {
                    count += temp;

                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, _previousCell, CellSide.top,
                        CellSide.down, true);
                }

                _previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private void ConnectionBetweenCellsSetActive(CellType currentNodeType, GridCell simpleCell,
            GridCell _previousCell,
            CellSide first, CellSide second, bool setActive)
        {
            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, first, currentNodeType);
            _previousCell.CellConnectionProvider.ConnectionSetActive(setActive, second, currentNodeType);
        }

        private int CheckLeftSide(CellNode cellNode)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell _previousCell = cellNode;

            for (int index = Cells.Count - 1; index >= 0; index--)
            {
                var simpleCell = Cells[index];

                if ((int)simpleCell.Coordinates.x != (int)cellNode.Coordinates.x)
                {
                    continue;
                }

                if (simpleCell.Coordinates.y >= cellNode.Coordinates.y)
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
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)cellNode.Coordinates.y,
                    -step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;

                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, _previousCell, CellSide.right,
                        CellSide.left, true);
                }

                step++;
                _previousCell = simpleCell;
            }

            return count;
        }

        private int CheckRightSide(CellNode cellNode)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell _previousCell = cellNode;

            foreach (var simpleCell in Cells)
            {
                if ((int)simpleCell.Coordinates.x != (int)cellNode.Coordinates.x)
                {
                    continue;
                }

                if (simpleCell.Coordinates.y <= cellNode.Coordinates.y)
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
                    cellNode.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)cellNode.Coordinates.y,
                    step);

                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;
                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, _previousCell, CellSide.left,
                        CellSide.right, true);
                }

                step++;

                _previousCell = simpleCell;
            }

            return count;
        }


        private int CheckAxisCell(CellType cellType, CellType currentNodeType, int cellCoordinate, int nodeCoordinate,
            int step = 1)
        {
            if (cellType != currentNodeType && cellType != CellType.universal) return 0;

            if (cellCoordinate == nodeCoordinate) return 0;

            int count = 0;

            if (cellCoordinate - step == nodeCoordinate)
            {
                count++;
            }

            return count;
        }

        #endregion
    }
}