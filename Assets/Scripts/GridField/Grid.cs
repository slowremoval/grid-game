using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class Grid : MonoBehaviour
    {
        [HideInInspector] public List<CellNode> Nodes;
        [HideInInspector] public List<GridCell> Cells;

        public GridCell[,] _allGridElements;
    }
}