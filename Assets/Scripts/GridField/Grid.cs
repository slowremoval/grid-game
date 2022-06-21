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
        private List<CountingCell> CountingCells;

        #region NodeNeighbours

        public void RecalculateNeighbourAxises(GridCell gridCell)
        {
            DeactivateVerticalAndHorizontalConnections(gridCell);
            FindCurrentCountingCells();
            
            FindCurrentNodes(gridCell);

            foreach (GridCell activeGridElement in _activeGridElements)
            {
                if (activeGridElement is CountingCell countingCell)
                {
                    CheckNeighbours(countingCell);
                }
            }
            
            foreach (CellNode node in Nodes)
            {
                CheckNeighbours(node);
            }
        }

        private void FindCurrentCountingCells()
        {
            CountingCells = new List<CountingCell>();
            foreach (var gridCell in _activeGridElements)
            {
                if (gridCell is CountingCell countingCell)
                {
                    CountingCells.Add(countingCell);
                }
            }
            
            

            foreach (var countingCell in CountingCells)
            {
                countingCell.isChecked = false;
            }

            for (var index = 0; index < CountingCells.Count; index++)
            {
                var countingCell = CountingCells[index];
                
                if (countingCell.isChecked)
                {
                    continue;
                }

                CheckNeighbours(countingCell);
                countingCell.isChecked = true;
            }
        }

        protected void CheckNeighbours(CellNode node)
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


        private int CheckVertical(GridCell cellNode)
        {
            int count = 0;

            count += CheckUpperSide(cellNode);

            count += CheckUnderSide(cellNode);

            return count;
        }

        private int CheckHorizontal(GridCell cellNode)
        {
            int rightNeighbours = CheckRightSide(cellNode);

            int leftNeighbours = CheckLeftSide(cellNode);

            int count = leftNeighbours + rightNeighbours;

            return count;
        }

        private int CheckUnderSide(GridCell cellNode)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cellNode;

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

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.top))
                {
                    isCountActive = false;
                }

                bool ignoreCounting = cellNode is CountingCell;
                
                bool temp = CheckAxisCell(
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)cellNode.Coordinates.x,
                    step, ignoreCounting);

                if (temp == false)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count = IncrementCounter(count, simpleCell,ref isCountActive);
                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, previousCell, CellSide.down,
                        CellSide.top, true);
                }

                previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private int IncrementCounter(int count, GridCell cell, ref bool isCountActive)
        {
            if (cell is CountingCell countingCell)
            {
                count = countingCell.Capacity;
                isCountActive = false;
            }
            else
            {
                count += 1; 
            }
            
            return count;
        }

        private void CheckNeighbours(CountingCell countingCell)
        {
            int neighboursCount = 0;
            
            countingCell.Capacity = 0;
            neighboursCount += CheckHorizontal(countingCell);
            neighboursCount += CheckVertical(countingCell);

            countingCell.Capacity = neighboursCount;

            countingCell.UpdateVisualization();
        }

        private int CheckUpperSide(GridCell cellNode)
        {
            int step = 1;
            int count = 0;
            bool isCountActive = true;
            GridCell previousCell = cellNode;

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

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.down))
                {
                    isCountActive = false;
                }

                bool ignoreCounting = cellNode is CountingCell;
                bool temp = CheckAxisCell(
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.x,
                    (int)cellNode.Coordinates.x,
                    -step, ignoreCounting);

                if (temp == false)
                {
                    isCountActive = false;
                }


                if (isCountActive)
                {
                    count = IncrementCounter(count, simpleCell,ref isCountActive);

                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, previousCell, CellSide.top,
                        CellSide.down, true);
                }

                previousCell = simpleCell;
                step++;
            }

            return count;
        }

        private void ConnectionBetweenCellsSetActive(CellType currentNodeType, GridCell simpleCell,
            GridCell previousCell,
            CellSide first, CellSide second, bool setActive)
        {
            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, first, currentNodeType);
            previousCell.CellConnectionProvider.ConnectionSetActive(setActive, second, currentNodeType);
        }

        private int CheckLeftSide(GridCell cellNode)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cellNode;

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

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                bool ignoreCounting = cellNode is CountingCell;
                bool temp = CheckAxisCell(
                    simpleCell.CellType, cellNode.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)cellNode.Coordinates.y,
                    -step, ignoreCounting);

                if (temp == false)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count = IncrementCounter(count, simpleCell,ref isCountActive);

                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, previousCell, CellSide.right,
                        CellSide.left, true);
                }

                step++;
                previousCell = simpleCell;
            }

            return count;
        }

        private int CheckRightSide(GridCell cellNode)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell previousCell = cellNode;

            foreach (GridCell simpleCell in Cells)
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

                if (step > 1 && previousCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }

                bool ignoreCounting = cellNode is CountingCell;
                bool temp = CheckAxisCell(
                    simpleCell.CellType,
                    cellNode.CellType,
                    (int)simpleCell.Coordinates.y,
                    (int)cellNode.Coordinates.y,
                    step, ignoreCounting);

                if (temp == false)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count = IncrementCounter(count, simpleCell,ref isCountActive);
                    
                    ConnectionBetweenCellsSetActive(cellNode.CellType, simpleCell, previousCell, CellSide.left,
                        CellSide.right, true);
                }

                step++;

                previousCell = simpleCell;
            }

            return count;
        }


        private bool CheckAxisCell(CellType cellType, CellType currentNodeType, int cellCoordinate, int nodeCoordinate,
            int step = 1, bool ignoreCounting = false)
        {
            if (ignoreCounting && cellType == CellType.counting)
            {
                return false;
            }
            
            if (cellType != currentNodeType && cellType != CellType.universal) return false;

            if (cellCoordinate == nodeCoordinate) return false;
            
            return cellCoordinate - step == nodeCoordinate;
        }

        #endregion
    }
}