using System;
using System.Linq;
using GridField.Cells.Cell_Modifiers;
using UnityEngine;

namespace GridField.Cells
{
    public abstract class GridCell : UIElementSelector
    {
        [HideInInspector] public Vector2 Coordinates;
        [HideInInspector] public CellSide[] UnactiveSides = new CellSide[4];
        [HideInInspector] public Grid GridData;
        [HideInInspector] public int Capacity;
        public CellType CellType;


        public event Action<GridCell> OnVisualizationChanged;

        public CellConnectionProvider CellConnectionProvider;

        public void SendVisualizationChanged() => OnVisualizationChanged?.Invoke(this);
        public void InitializeGrid() => UnactiveSides = new CellSide[4];

        public void SetSidesProperties(Vector4 vector4, GridCell cell)
        {
            cell.UnactiveSides = new CellSide[4];

            int[] cellSides = Vector4ToArray(vector4);

            for (int i = 0; i < cell.UnactiveSides.Length; i++)
            {
                cell.UnactiveSides[i] = (CellSide)cellSides[i];
            }
        }

        public bool[] HasNeighbours()
        {
            bool[] neighbours = new bool[4];
            RotatingCellModifier rotating = default;

            if ((int)Coordinates.x < GridData._allGridElements.GetLength(0) - 1 &&
                !GridData._allGridElements[(int)Coordinates.x + 1, (int)Coordinates.y]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._allGridElements[(int)Coordinates.x + 1, (int)Coordinates.y]
                    .TryGetComponent<GridCell>(out var right);
                GridData._allGridElements[(int)Coordinates.x + 1, (int)Coordinates.y]
                    .TryGetComponent<RotatingCellModifier>(out rotating);

                if (rotating == default)
                {
                    if (!right.UnactiveSides.Contains(CellSide.down) && !UnactiveSides.Contains(CellSide.top))
                    {
                        neighbours[(int)CellSide.top - 1] = true;
                    }
                }
                else
                {
                    if (!UnactiveSides.Contains(CellSide.top))
                    {
                        neighbours[(int)CellSide.top - 1] = true;
                    }
                }
            }

            if ((int)Coordinates.x != 0 && !GridData._allGridElements[(int)Coordinates.x - 1, (int)Coordinates.y]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._allGridElements[(int)Coordinates.x - 1, (int)Coordinates.y]
                    .TryGetComponent<GridCell>(out var right);
                GridData._allGridElements[(int)Coordinates.x - 1, (int)Coordinates.y]
                    .TryGetComponent<RotatingCellModifier>(out rotating);

                if (rotating == default)
                {
                    if (!right.UnactiveSides.Contains(CellSide.top) && !UnactiveSides.Contains(CellSide.down))
                    {
                        neighbours[(int)CellSide.down - 1] = true;
                    }
                }
                else
                {
                    if (!UnactiveSides.Contains(CellSide.down))
                    {
                        neighbours[(int)CellSide.down - 1] = true;
                    }
                }
            }

            if ((int)Coordinates.y != 0 && !GridData._allGridElements[(int)Coordinates.x, (int)Coordinates.y - 1]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._allGridElements[(int)Coordinates.x, (int)Coordinates.y - 1]
                    .TryGetComponent<GridCell>(out var right);
                GridData._allGridElements[(int)Coordinates.x, (int)Coordinates.y - 1]
                    .TryGetComponent<RotatingCellModifier>(out rotating);

                if (rotating == default)
                {
                    if (!right.UnactiveSides.Contains(CellSide.right) && !UnactiveSides.Contains(CellSide.left))
                    {
                        neighbours[(int)CellSide.left - 1] = true;
                    }
                }
                else
                {
                    if (!UnactiveSides.Contains(CellSide.left))
                    {
                        neighbours[(int)CellSide.left - 1] = true;
                    }
                }
            }

            if ((int)Coordinates.y < GridData._allGridElements.GetLength(1) - 1 && !GridData
                    ._allGridElements[(int)Coordinates.x, (int)Coordinates.y + 1]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._allGridElements[(int)Coordinates.x, (int)Coordinates.y + 1]
                    .TryGetComponent<GridCell>(out var right);
                GridData._allGridElements[(int)Coordinates.x, (int)Coordinates.y + 1]
                    .TryGetComponent<RotatingCellModifier>(out rotating);

                if (rotating == default)
                {
                    if (!right.UnactiveSides.Contains(CellSide.left) && !UnactiveSides.Contains(CellSide.right))
                    {
                        neighbours[(int)CellSide.right - 1] = true;
                    }
                }
                else
                {
                    if (!UnactiveSides.Contains(CellSide.right))
                    {
                        neighbours[(int)CellSide.right - 1] = true;
                    }
                }
            }

            return neighbours;
        }

        private int[] Vector4ToArray(Vector4 vector4)
        {
            int[] vector4ToArray = new int [4];

            vector4ToArray[0] = (int)vector4.x;
            vector4ToArray[1] = (int)vector4.y;
            vector4ToArray[2] = (int)vector4.z;
            vector4ToArray[3] = (int)vector4.w;

            return vector4ToArray;
        }
    }
}