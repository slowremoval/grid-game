using UnityEditor;
using UnityEngine;

namespace GridField.Cells
{
    [CustomEditor(typeof(GridCellConstruct))]
    public class GridCellConstructEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridCellConstruct builder = (GridCellConstruct)target;

            GUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Set Light Node", GUILayout.Height(33)))
            {
                builder.SetLightNode();
            }

            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Set Dark Node", GUILayout.Height(33)))
            {
                builder.SetDarkNode();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Set Simple Sell", GUILayout.Height(33)))
            {
                builder.SetSimpleCell();
            }

            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Set Empty Cell", GUILayout.Height(33)))
            {
                builder.SetEmptyCell();
            }

            GUILayout.EndHorizontal();

            NumberButtons(builder, 21);
        }

        private void NumberButtons(GridCellConstruct part, int number)
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