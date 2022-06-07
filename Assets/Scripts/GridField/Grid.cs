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

       
    }
}