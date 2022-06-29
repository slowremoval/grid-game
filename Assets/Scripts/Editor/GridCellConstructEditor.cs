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

            GridCellConstruct constructor = (GridCellConstruct)target;

            SetCellType(constructor);

            SideSetActive(constructor);

            CreateNumberButtons(constructor, 21);
        }

        private void SetCellType(GridCellConstruct constructor)
        {
            GUILayout.BeginHorizontal();

            SetLightNodeButton(constructor);

            SetDarkNodeButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetLightNodeRotatingCell(constructor);
            SetDarkNodeRotatingCell(constructor);
            
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetSimpleCellButton(constructor);

            SetEmptyCellButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetStableLightButton(constructor);
            SetStableDarkButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetStableLightRotatingCell(constructor);
            SetStableDarkRotatingCell(constructor);
            
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            SetRotatingCellButton(constructor);
            SetUniversalCellButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            //SetCountingCellButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            SetUniversalRotatingCell(constructor);

            GUILayout.EndHorizontal();
        }

        private void SetLightNodeRotatingCell(GridCellConstruct constructor)
        {GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Ligh tNode Rotating", GUILayout.Height(33)))
            {
                constructor.SetLightNodeRotatingCell();
            }
        }

        private void SetDarkNodeRotatingCell(GridCellConstruct constructor)
        {GUI.backgroundColor = Color.grey;
            if (GUILayout.Button("Dark Node Rotating", GUILayout.Height(33)))
            {
                constructor.SetDarkNodeRotatingCell();
            }
        }

        private void SetStableLightRotatingCell(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Stable Light Rotating", GUILayout.Height(33)))
            {
                constructor.SetStableLightRotatingCell();
            }
        }

        private void SetStableDarkRotatingCell(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Stable Dark Rotating", GUILayout.Height(33)))
            {
                constructor.SetStableDarkRotatingCell();
            }
        }

        private void SetUniversalRotatingCell(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Universal Rotating", GUILayout.Height(33)))
            {
                constructor.SetUniversalRotatingCell();
            }
        }

        private void SetCountingCellButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.magenta;
            if (GUILayout.Button("Counting Cell", GUILayout.Height(33)))
            {
                constructor.SetCountingCell();
            }
        }

        private void SideSetActive(GridCellConstruct constructor)
        {
            GUILayout.BeginHorizontal();

            DeactivateLeftSideButton(constructor);
            DeactivateRightSideButton(constructor);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            DeactivateUpperSideButton(constructor);
            DeactivateUnderSideButton(constructor);

            GUILayout.EndHorizontal();
        }

        private void DeactivateLeftSideButton(GridCellConstruct constructor)
        {
            _leftSideActive = !constructor.UnactiveSides.Contains(CellSide.left);
            GUI.backgroundColor = _leftSideActive ? Color.green : Color.grey;
            if (GUILayout.Button("Left Side", GUILayout.Height(33)))
            {
                constructor.ChangeLeftSideState();
            }
        }

        private void DeactivateRightSideButton(GridCellConstruct constructor)
        {
            _rightSideActive = !constructor.UnactiveSides.Contains(CellSide.right);
            GUI.backgroundColor = _rightSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Right Side", GUILayout.Height(33)))
            {
                constructor.ChangeRightSideState();
            }
        }

        private void DeactivateUpperSideButton(GridCellConstruct constructor)
        {
            _upperSideActive = !constructor.UnactiveSides.Contains(CellSide.top);
            GUI.backgroundColor = _upperSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Upper Side", GUILayout.Height(33)))
            {
                constructor.ChangeUpperSideState();
            }
        }

        private void DeactivateUnderSideButton(GridCellConstruct constructor)
        {
            _underSideActive = !constructor.UnactiveSides.Contains(CellSide.down);

            GUI.backgroundColor = _underSideActive ? Color.green : Color.grey;

            if (GUILayout.Button("Under Side", GUILayout.Height(33)))
            {
                constructor.ChangeUnderSideState();
            }
        }

        private void SetUniversalCellButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Universal Cell", GUILayout.Height(33)))
            {
                constructor.SetUniversalCell();
            }
        }

        private void SetStableDarkButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Stable Dark", GUILayout.Height(33)))
            {
                constructor.SetStableDarkCell();
            }
        }

        private void SetStableLightButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Stable Light", GUILayout.Height(33)))
            {
                constructor.SetStableLightCell();
            }
        }

        private static void SetRotatingCellButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.magenta;
            if (GUILayout.Button("Rotating Cell", GUILayout.Height(33)))
            {
                constructor.SetRotatingCell();
            }
        }

        private static void SetEmptyCellButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Empty Cell", GUILayout.Height(33)))
            {
                constructor.SetEmptyCell();
            }
        }

        private static void SetSimpleCellButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Simple Cell", GUILayout.Height(33)))
            {
                constructor.SetSimpleCell();
            }
        }

        private static void SetDarkNodeButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Dark Node", GUILayout.Height(33)))
            {
                constructor.SetDarkNode();
            }
        }

        private static void SetLightNodeButton(GridCellConstruct constructor)
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Light Node", GUILayout.Height(33)))
            {
                constructor.SetLightNode();
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