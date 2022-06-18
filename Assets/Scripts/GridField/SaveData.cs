using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class SaveData
    {
        public Vector2[] Coordinates;
        public int[] CelCapacities;
        public CellType[] CellTypes;
        public Vector2 GridSize;
        public Vector4[] UnactiveSidesVector;

        public void SetSaveData(GridCell[,] grid)
        {
            if (grid == null || grid.Length == 0)
            {
                return;
            }
            
            GridSize = new Vector2(grid.GetLength(0), grid.GetLength(1));
            Coordinates = new Vector2[grid.Length];
            CellTypes = new CellType[grid.Length];
            CelCapacities = new int[grid.Length];
            UnactiveSidesVector = new Vector4[grid.Length];
            
            int count = 0;

            foreach (GridCell item in grid)
            {
                if (!(item is GridCellConstruct newItem))
                {
                    return;
                }

                Coordinates[count] = newItem.Coordinates;
                CellTypes[count] = newItem.ThisCellType;
                CelCapacities[count] = newItem.CellCapacity;
                
                UnactiveSidesVector[count] = new Vector4(
                    (int)newItem.UnactiveSides[0], 
                    (int)newItem.UnactiveSides[1],
                    (int)newItem.UnactiveSides[2], 
                    (int)newItem.UnactiveSides[3]);

                count++;
            }
        }
    }
}