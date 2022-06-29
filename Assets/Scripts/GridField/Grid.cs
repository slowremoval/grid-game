using System;
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


        private List<CellNode> _currentNodes;

        #region NodeNeighbours

        protected void RecalculateNeighbourAxises(GridCell gridCell)
        {
            DeactivateVerticalAndHorizontalConnections(gridCell);

            FindCurrentNodes(gridCell);

            RecalculateNodesNeighbours();
        }

        private void RecalculateNodesNeighbours()
        {
            foreach (CellNode node in _currentNodes)
            {
                CheckNeighbours(node, Cells);
            }
        }

        protected void CheckNeighbours<T>(CellNode node, List<T> listToCheck) where T : GridCell
        {
            int[] neighboursCount = CheckCellNeighbours(node, listToCheck);

            node.SetNeighboursAround(neighboursCount);
            
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
            _currentNodes = new List<CellNode>();

            foreach (CellNode node in Nodes)
            {
                if ((int)node.Coordinates.x == (int)gridCell.Coordinates.x ||
                    (int)node.Coordinates.y == (int)gridCell.Coordinates.y)
                {
                    _currentNodes.Add(node);
                }
            }
        }

        private int CheckUpperSide<T>(GridCell cell, List<T> listToCheck) where T : GridCell
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cell;

            foreach (T currentCell in listToCheck)
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

                if (previousCell.UnactiveSides.Contains(CellSide.top))
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

        private void UpdateCurrentCellConnection(GridCell cellToCheck, GridCell currentCell, int cellCoordinates,
            int currentCellCoordinates, int step, GridCell previousCell,
            ref int count, ref bool isCountActive)
        {
            if (currentCell is CellNode)
            {
                isCountActive = false;
                return;
            }

            if (isCountActive)
            {
                isCountActive = CheckAxisCell(
                    currentCell.CellType, cellToCheck.CellType,
                    currentCellCoordinates,
                    cellCoordinates,
                    step);
            }
            
            if (isCountActive)
            {
                count = IncrementCounter(count, currentCell);
                ConnectionBetweenCellsSetActive(cellToCheck.CellType, currentCell, previousCell, true);
            }
        }

        private int IncrementCounter(int count, GridCell cell)
        {
            count += cell.Capacity;
            return count;
        }

        private int[] CheckCellNeighbours<T>(GridCell cell, List<T> listToCheck)
            where T : GridCell
        {
            int[] sidesValues = new int[4];

            sidesValues[0] = CheckUpperSide(cell, listToCheck);
            sidesValues[1] = CheckRightSide(cell, listToCheck);
            sidesValues[2] = CheckUnderSide(cell, listToCheck);
            sidesValues[3] = CheckLeftSide(cell, listToCheck);

            return sidesValues;
        }

        private int CheckUnderSide<T>(GridCell cell, List<T> listToCheck) where T : GridCell
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cell;

            for (int index = listToCheck.Count - 1; index >= 0; index--)
            {
                GridCell currentCell = listToCheck[index];

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

                if (previousCell.UnactiveSides.Contains(CellSide.down))
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
            int cellSideMemberCount = Enum.GetNames(typeof(CellSide)).Length;

            CellSide previousSide = GetPreviousCellPosition(simpleCell, previousCell);

            CellSide currentSide =
                (int)(previousSide + 2) < cellSideMemberCount ? (previousSide + 2) : (previousSide - 2);

            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, currentSide, currentNodeType);
            previousCell.CellConnectionProvider.ConnectionSetActive(setActive, previousSide, currentNodeType);
        }

        private CellSide GetPreviousCellPosition(GridCell simpleCell, GridCell previousCell)
        {
            CellSide previousSide;
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

            return previousSide;
        }

        private int CheckLeftSide<T>(GridCell cell, List<T> listToCheck) where T : GridCell
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cell;

            for (int index = listToCheck.Count - 1; index >= 0; index--)
            {
                GridCell currentCell = listToCheck[index];

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

                if (previousCell.UnactiveSides.Contains(CellSide.left))
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

        private int CheckRightSide<T>(GridCell cell, List<T> listToCheck) where T : GridCell
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cell;

            foreach (T currentCell in listToCheck)
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

                if (previousCell.UnactiveSides.Contains(CellSide.right))
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


        private bool CheckAxisCell(CellType cellType, CellType cellToCheckType, int currentCellCoordinate,
            int cellCoordinate, int step = 1)
        {
            if (cellToCheckType == CellType.empty || cellType == CellType.empty) return false;
            
            if (cellToCheckType == CellType.simple) return false;

            if (cellType != cellToCheckType && cellType != CellType.universal) return false;

            if (currentCellCoordinate == cellCoordinate) return false;

            return currentCellCoordinate - step == cellCoordinate;
        }

        #endregion
    }
}