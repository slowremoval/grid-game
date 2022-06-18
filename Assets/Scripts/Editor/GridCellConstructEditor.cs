using System.Linq;
using GridField.Cells;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridCellConstruct))]
    public class GridCellConstructEditor : UnityEditor.Editor
    {
        private bool _leftSideActive = true;
        private bool _rightSideActive = true;
        private bool _underSideActive = true;
        private bool _upperSideActive = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridCellConstruct builder = (GridCellConstruct)target;

            SetCellType(builder);

            SideSetActive(builder);

            CreateNumberButtons(builder, 21);
        }

        private void SetCellType(GridCellConstruct builder)
        {
            GUILayout.BeginHorizontal();

            SetLightNodeButton(builder);

            SetDarkNodeButton(builder);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetSimpleCellButton(builder);

            SetEmptyCellButton(builder);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetStableLightButton(builder);

            SetStableDarkButton(builder);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            SetRotatingCellButton(builder);
            SetUniversalCellButton(builder);

            GUILayout.EndHorizontal();
        }

        private void SideSetActive(GridCellConstruct builder)
        {
            GUILayout.BeginHorizontal();

            DeactivateLeftSideButton(builder);
            DeactivateRightSideButton(builder);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            DeactivateUpperSideButton(builder);
            DeactivateUnderSideButton(builder);

            GUILayout.EndHorizontal();
        }

        private void DeactivateLeftSideButton(GridCellConstruct builder)
        {
            _leftSideActive = !builder.UnactiveSides.Contains(CellSide.left);
            GUI.backgroundColor = _leftSideActive ? Color.green : Color.grey;
            if (GUILayout.Button("Left Side", GUILayout.Height(33)))
            {
                builder.ChangeLeftSideState();
            }
        }

        private void DeactivateRightSideButton(GridCellConstruct builder)
        {
            _rightSideActive = !builder.UnactiveSides.Contains(CellSide.right);
            GUI.backgroundColor = _rightSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Right Side", GUILayout.Height(33)))
            {
                builder.ChangeRightSideState();
            }
        }

        private void DeactivateUpperSideButton(GridCellConstruct builder)
        {
            _upperSideActive = !builder.UnactiveSides.Contains(CellSide.top);
            GUI.backgroundColor = _upperSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Upper Side", GUILayout.Height(33)))
            {
                builder.ChangeUpperSideState();
            }
        }

        private void DeactivateUnderSideButton(GridCellConstruct builder)
        {
            _underSideActive = !builder.UnactiveSides.Contains(CellSide.down);

            GUI.backgroundColor = _underSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Under Side", GUILayout.Height(33)))
            {
                builder.ChangeUnderSideState();
            }
        }

        private void SetUniversalCellButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Set Universal Cell", GUILayout.Height(33)))
            {
                builder.SetUniversalCell();
            }
        }

        private void SetStableDarkButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Set Stable Dark", GUILayout.Height(33)))
            {
                builder.SetStableDarkCell();
            }
        }

        private void SetStableLightButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Set Stable Light", GUILayout.Height(33)))
            {
                builder.SetStableLightCell();
            }
        }

        private static void SetRotatingCellButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.magenta;
            if (GUILayout.Button("Set Rotating Cell", GUILayout.Height(33)))
            {
                builder.SetRotatingCell();
            }
        }

        private static void SetEmptyCellButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Set Empty Cell", GUILayout.Height(33)))
            {
                builder.SetEmptyCell();
            }
        }

        private static void SetSimpleCellButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Set Simple Cell", GUILayout.Height(33)))
            {
                builder.SetSimpleCell();
            }
        }

        private static void SetDarkNodeButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Set Dark Node", GUILayout.Height(33)))
            {
                builder.SetDarkNode();
            }
        }

        private static void SetLightNodeButton(GridCellConstruct builder)
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Set Light Node", GUILayout.Height(33)))
            {
                builder.SetLightNode();
            }
        }

        private void CreateNumberButtons(GridCellConstruct part, int number)
        {
            GUI.backgroundColor = Color.white;
            for (int j = 1; j < (number + 1);)
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < 3; i++)
                {
                    if (GUILayout.Button(j.ToString(), GUILayout.Height(42)))
                    {
                        part.SetRequiredAmount(j);

                        if (part.ThisCellType != CellType.light &&
                            part.ThisCellType != CellType.dark)
                        {
                            part.SetCellType(CellType.light);
                        }
                    }

                    j++;
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}