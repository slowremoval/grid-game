using GridField.Cells;
using UnityEngine;

namespace GridField
{
    public class SaveData
    {
        public Vector2[] Coordinates;
        public int[] RequiredAmounts;
        public CellType[] CellTypes;

        public void SetSaveData(GridCell[,] grid)
        {
            Coordinates = new Vector2[grid.Length];
            CellTypes = new CellType[grid.Length];
            RequiredAmounts = new int[grid.Length];

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

                count++;
            }
        }
    }
}