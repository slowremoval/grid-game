using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class SaveData
    {
        public Vector2[] Coordinates;
        public int[] RequiredAmounts;
        public CellType[] CellTypes;
        public Vector2 GridSize;
        public CellSide[] UnactiveSide;

        public void SetSaveData(GridCell[,] grid)
        {
            GridSize = new Vector2(grid.GetLength(0), grid.GetLength(1));
            Coordinates = new Vector2[grid.Length];
            CellTypes = new CellType[grid.Length];
            RequiredAmounts = new int[grid.Length];
            UnactiveSide = new CellSide[grid.Length];
            
            int count = 0;

            foreach (GridCell item in grid)
            {
                if (!(item is GridCellConstruct newItem))
                {
                    return;
                }

                Coordinates[count] = newItem.Coordinates;
                CellTypes[count] = newItem.ThisCellType;
                RequiredAmounts[count] = newItem.requiredAmount;
                UnactiveSide[count] = newItem.UnactiveSide;

                count++;
            }
        }
    }
}