using UnityEditor;
using UnityEngine;

namespace GridField
{
    [CustomEditor(typeof(GridConstructor))]
    public class GridConstructorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridConstructor builder = (GridConstructor)target;

            GUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Start Grid Building", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return;
                }
                
                builder.StartGridBuilding();
            }
        
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Start Save Redacting", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return;
                }
                
                builder.StartSaveRedacting();
            }
        
        
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
        
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save Level", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return;
                }
                
                builder.SaveLevel();
            } 
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Start Level", GUILayout.Height(37)))
            {
                if (Application.isPlaying == false)
                {
                    return;
                }
                
                builder.StartLevel();
            }
        
            GUILayout.EndHorizontal();
        }
    }
}