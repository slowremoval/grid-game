using UnityEngine;

namespace GridField.Cells
{
    public abstract class GridCell : UIElementSelector
    {
        public Vector2 Coordinates;
        public CellSide UnactiveSide;
    }
}