using GridField;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridConstructor))]
    public class GridConstructorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridConstructor builder = (GridConstructor)target;

            GUILayout.BeginHorizontal();

            if (StartGrinBuildingButton(builder)) return;

            if (StartSaveRedactingButton(builder)) return;


            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
        
            if (SaveLevelButton(builder)) return;

            if (StartLevelButton(builder)) return;

            GUILayout.EndHorizontal();
        }

        private static bool StartLevelButton(GridConstructor builder)
        {
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Start Level", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return true;
                }

                builder.StartLevel();
            }

            return false;
        }

        private static bool SaveLevelButton(GridConstructor builder)
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save Level", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return true;
                }

                builder.SaveLevel();
            }

            return false;
        }

        private static bool StartSaveRedactingButton(GridConstructor builder)
        {
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Start Save Redacting", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return true;
                }

                builder.StartSaveRedacting();
            }

            return false;
        }

        private static bool StartGrinBuildingButton(GridConstructor builder)
        {
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Start Grid Building", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return true;
                }

                builder.StartGridBuilding();
            }

            return false;
        }
    }
}