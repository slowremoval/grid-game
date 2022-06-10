using System.Linq;
using UnityEngine;

namespace GridField.Cells
{
    public abstract class GridCell : UIElementSelector
    {
        public Vector2 Coordinates;
        public CellSide[] UnactiveSides;
        public CellType CellType { get; set; }

        [HideInInspector] public Grid GridData;

        private void Start()
        {
            InitializeSides();
        }

        private void InitializeSides()
        {
            UnactiveSides = new CellSide[4];
        }

        public void SetSidesProperties(Vector4 vector4, GridCell cell)
        {
            cell.UnactiveSides = new CellSide[4];

            int[] cellSides = Vector4ToArray(vector4);

            for (int i = 0; i < cell.UnactiveSides.Length; i++)
            {
                cell.UnactiveSides[i] = (CellSide)cellSides[i];
            }
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

        public bool[] HasNeighbours(bool countNeighbourUnactive = true)
        {
            bool[] neighbours = new bool[4];
            RotatingCell rotating = default;
            
            if ((int)Coordinates.x < GridData._cells.GetLength(0) - 1 &&
                !GridData._cells[(int)Coordinates.x + 1, (int)Coordinates.y].TryGetComponent<EmptyCell>(out _))
            {
                GridData._cells[(int)Coordinates.x + 1, (int)Coordinates.y].TryGetComponent<GridCell>(out var right);
                GridData._cells[(int)Coordinates.x + 1, (int)Coordinates.y].TryGetComponent<RotatingCell>(out rotating);
                
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

            if ((int)Coordinates.x != 0 && !GridData._cells[(int)Coordinates.x - 1, (int)Coordinates.y]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._cells[(int)Coordinates.x - 1, (int)Coordinates.y].TryGetComponent<GridCell>(out var right);
                GridData._cells[(int)Coordinates.x - 1, (int)Coordinates.y].TryGetComponent<RotatingCell>(out rotating);

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

            if ((int)Coordinates.y != 0 && !GridData._cells[(int)Coordinates.x, (int)Coordinates.y - 1]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._cells[(int)Coordinates.x, (int)Coordinates.y - 1].TryGetComponent<GridCell>(out var right);
                GridData._cells[(int)Coordinates.x, (int)Coordinates.y - 1].TryGetComponent<RotatingCell>(out rotating);

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

            if ((int)Coordinates.y < GridData._cells.GetLength(1) - 1 && !GridData
                    ._cells[(int)Coordinates.x, (int)Coordinates.y + 1]
                    .TryGetComponent<EmptyCell>(out _))
            {
                GridData._cells[(int)Coordinates.x, (int)Coordinates.y + 1].TryGetComponent<GridCell>(out var right);
                GridData._cells[(int)Coordinates.x, (int)Coordinates.y + 1].TryGetComponent<RotatingCell>(out rotating);

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
    }
}