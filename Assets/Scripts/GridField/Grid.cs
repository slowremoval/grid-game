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
        public List<GridCell> _activeGridElements;


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

        protected void CheckNeighbours(CellNode node)
        {
            int neighboursCount = CheckCellNeighbours(node);

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

        private int CheckUpperSide(GridCell cell)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cell;

            foreach (GridCell currentCell in Cells)
            {
                if ((int)currentCell.Coordinates.y != (int)cell.Coordinates.y)
                {
                    continue;
                }

                if (currentCell.Coordinates.x <= cell.Coordinates.x)
                {
                    continue;
                }

                if (currentCell.UnactiveSides.Contains(CellSide.down))
                {
                    isCountActive = false;
                }

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.top))
                {
                    isCountActive = false;
                }

                UpdateCurrentCellConnection(cell, currentCell, (int)cell.Coordinates.x,
                    (int)currentCell.Coordinates.x, step,
                    previousCell, ref count, ref isCountActive);

                previousCell = currentCell;
                step++;
            }

            return count;
        }

        private void UpdateCurrentCellConnection(GridCell cell, GridCell currentCell, int cellCoordinates,
            int currentCellCoordinates, int step, GridCell previousCell,
            ref int count, ref bool isCountActive)
        {
            if (isCountActive)
            {
                isCountActive = CheckAxisCell(
                    currentCell.CellType, cell.CellType,
                    currentCellCoordinates,
                    cellCoordinates,
                    step);
            }

            if (isCountActive)
            {
                count = IncrementCounter(count, currentCell, ref isCountActive);

                ConnectionBetweenCellsSetActive(cell.CellType, currentCell, previousCell, true);
            }
        }

        private int IncrementCounter(int count, GridCell cell, ref bool isCountActive)
        {
            count += cell.Capacity;

            return count;
        }

        private int CheckCellNeighbours(GridCell cell)
        {
            int upperNeighbours = CheckUnderSide(cell);

            int underNeighbours = CheckUpperSide(cell);
            int rightNeighbours = CheckRightSide(cell);

            int leftNeighbours = CheckLeftSide(cell);

            int count = leftNeighbours + rightNeighbours + underNeighbours + upperNeighbours;
            return count;
        }

        private int CheckUnderSide(GridCell cell)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cell;

            for (int index = Cells.Count - 1; index >= 0; index--)
            {
                GridCell currentCell = Cells[index];

                if ((int)currentCell.Coordinates.y != (int)cell.Coordinates.y)
                {
                    continue;
                }

                if (currentCell.Coordinates.x >= cell.Coordinates.x)
                {
                    continue;
                }

                if (currentCell.UnactiveSides.Contains(CellSide.top))
                {
                    isCountActive = false;
                }

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.down))
                {
                    isCountActive = false;
                }

                UpdateCurrentCellConnection(cell, currentCell, (int)cell.Coordinates.x, (int)currentCell.Coordinates.x,
                    -step,
                    previousCell, ref count, ref isCountActive);

                previousCell = currentCell;
                step++;
            }

            return count;
        }

        private void ConnectionBetweenCellsSetActive(CellType currentNodeType, GridCell simpleCell,
            GridCell previousCell, bool setActive)
        {
            CellSide previousSide;
            CellSide currentSide;

            int cellSideMemberCount = CellSide.GetNames(typeof(CellSide)).Length;

            if (previousCell.Coordinates.y < simpleCell.Coordinates.y)
            {
                previousSide = CellSide.right;
            }
            else if (previousCell.Coordinates.y > simpleCell.Coordinates.y)
            {
                previousSide = CellSide.left;
            }
            else if (previousCell.Coordinates.x > simpleCell.Coordinates.x)
            {
                previousSide = CellSide.down;
            }
            else
            {
                previousSide = CellSide.top;
            }

            currentSide = (int)(previousSide + 2) < cellSideMemberCount ? (previousSide + 2) : (previousSide - 2);

            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, currentSide, currentNodeType);
            previousCell.CellConnectionProvider.ConnectionSetActive(setActive, previousSide, currentNodeType);
        }

        private int CheckLeftSide(GridCell cell)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cell;

            for (int index = Cells.Count - 1; index >= 0; index--)
            {
                GridCell currentCell = Cells[index];

                if ((int)currentCell.Coordinates.x != (int)cell.Coordinates.x)
                {
                    continue;
                }

                if (currentCell.Coordinates.y >= cell.Coordinates.y)
                {
                    continue;
                }

                if (currentCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                UpdateCurrentCellConnection(cell, currentCell, (int)cell.Coordinates.y, (int)currentCell.Coordinates.y,
                    -step,
                    previousCell, ref count, ref isCountActive);

                step++;
                previousCell = currentCell;
            }

            return count;
        }

        private int CheckRightSide(GridCell cell)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cell;

            foreach (GridCell currentCell in Cells)
            {
                if ((int)currentCell.Coordinates.x != (int)cell.Coordinates.x)
                {
                    continue;
                }

                if (currentCell.Coordinates.y <= cell.Coordinates.y)
                {
                    continue;
                }

                if (currentCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }

                UpdateCurrentCellConnection(cell, currentCell, (int)cell.Coordinates.y, (int)currentCell.Coordinates.y,
                    step,
                    previousCell, ref count, ref isCountActive);

                step++;

                previousCell = currentCell;
            }

            return count;
        }


        private bool CheckAxisCell(CellType cellType, CellType currentNodeType, int currentCellCoordinate,
            int cellCoordinate,
            int step = 1)
        {
            if (cellType != currentNodeType && cellType != CellType.universal) return false;

            if (currentCellCoordinate == cellCoordinate) return false;

            return currentCellCoordinate - step == cellCoordinate;
        }

        #endregion
    }
}