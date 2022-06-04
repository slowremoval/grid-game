using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class Grid: MonoBehaviour
    {
        public List<CellNode> Nodes;
        public List<SimpleCell> SimpleCells;
        
        public GridCell[,] _cells;
        
        public int _colsCount { get; set; }
        public int _rowsCount { get; set; }
        
        protected void SetGridProperties(int rowsCount, int colsCount)
        {
            _cells = new GridCell[rowsCount, colsCount];
        }
    }
}