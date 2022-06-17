using System.Collections.Generic;
using GridField.Cells;
using System.Linq;
using UnityEngine;

namespace GridField
{
    public class Grid : MonoBehaviour
    {
        [HideInInspector] public List<CellNode> Nodes;
        [HideInInspector] public List<GridCell> Cells;

        public GridCell[,] _allGridElements;
        public List<GridCell> _allGridElementsList;


        protected void InitializeGrid()
        {
            foreach (GridCell cell in Cells)
            {
                if (cell is SimpleCell simpleCell)
                {
                    simpleCell.OnColorChanged += CheckCellNeighbours;
                }
            }
        }

        private void CheckCellNeighbours(GridCell cell)
        {
            CheckRightSide(cell);
        }


        private int CheckRightSide(GridCell changedCell)
        {
            int count = 0;
            int step = 1;

            bool isCountActive = true;

            GridCell _previousCell = changedCell;
            CellNode currentNode = default;
            

            bool hasNode = false;

            for (int i = (int)changedCell.Coordinates.y; i < _allGridElements.GetLength(1); i++)
            {
                GridCell currentCell = _allGridElements[(int)changedCell.Coordinates.x, i];
                
                if (currentCell is CellNode node)
                {
                    hasNode = true;
                    currentNode = node;
                }
            }

            if (hasNode == false)
            {
                return 0;
            }
            
            for (int i = (int)changedCell.Coordinates.y; i < _allGridElements.GetLength(1); i++)
            {
                GridCell currentCell = _allGridElements[(int)changedCell.Coordinates.x, i];
                GridCell nextCell = _allGridElements[(int)changedCell.Coordinates.x, i + 1];
                if (currentCell is CellNode node)
                {
                    node.CurrentAmount = count;
                    node.ShowNodeRequirements(count);
                    return count;
                }
                
                if (step > 1 && currentCell.UnactiveSides.Contains(CellSide.left))
                {
                    isCountActive = false;
                }

                if (_previousCell.UnactiveSides.Contains(CellSide.right))
                {
                    isCountActive = false;
                }
                
                int temp = CheckAxisCell(
                    nextCell.CellType, 
                    changedCell.CellType,
                    (int)nextCell.Coordinates.y,
                    (int)changedCell.Coordinates.y,
                    step);
                
                if (temp == 0)
                {
                    isCountActive = false;
                }

                if (isCountActive)
                {
                    count += temp;
                    ConnectionBetweenCellsSetActive(currentCell, _previousCell, CellSide.left, 
                        changedCell.CellType,CellSide.right, true);
                }
                else
                {
                    if (Mathf.Abs(currentCell.Coordinates.y - _previousCell.Coordinates.y) > 1)
                    {
                        break;
                    }

                    ConnectionBetweenCellsSetActive(currentCell, _previousCell, CellSide.left, 
                        changedCell.CellType,CellSide.right,false);
                }

                step++;

                _previousCell = currentCell;
            }
            
            return count;
        }

        
        
        
        
        
        
        private void ConnectionBetweenCellsSetActive(GridCell simpleCell, GridCell previousCell,
            CellSide first, CellType currentCellType, CellSide second, bool setActive)
        {
            simpleCell.CellConnectionProvider.ConnectionSetActive(setActive, first, currentCellType);
            previousCell.CellConnectionProvider.ConnectionSetActive(setActive, second, currentCellType);
        }
        
        private int CheckAxisCell(CellType otherCellType, CellType currentCellType, int cellCoordinate, int currentCellCoordinate, int step = 1)
        {
            if (otherCellType != currentCellType && otherCellType != CellType.universal) return 0;

            if (cellCoordinate == currentCellCoordinate) return 0;

            int count = 0;

            if (cellCoordinate - step == currentCellCoordinate)
            {
                count++;
            }

            return count;
        }
    }
}