using UnityEditor;
using UnityEngine;

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
            builder.StartGridBuilding();
        }
        
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Finish Grid Building", GUILayout.Height(37)))
        {
            builder.FinishGridBuilding();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Save Level", GUILayout.Height(37)))
        {
            builder.SaveLevel();
        }
        
        GUILayout.EndHorizontal();
    }
}