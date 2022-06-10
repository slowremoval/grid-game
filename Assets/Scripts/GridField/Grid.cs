using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class Grid: MonoBehaviour
    {
        public List<CellNode> Nodes;
        public List<SimpleCell> SimpleCells;
        public List<RotatingCell> RotatingCells;
        
        public GridCell[,] _cells;

       
    }
}