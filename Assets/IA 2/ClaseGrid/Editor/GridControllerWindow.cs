using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridControllerWindow : EditorWindow
{
    private SpatialGrid grid;
    private GUIStyle style;
    private Queries boxQ;

    [MenuItem("CustomTools/GridController")]
    public static void OpenWindow()
    {
        var w = GetWindow<GridControllerWindow>();
        w.Show();
    }

    private void OnEnable()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 20;
        grid = FindObjectOfType<SpatialGrid>();
        maxSize = new Vector2(500, 375);
        minSize = new Vector2(500, 375);
    }

    private void OnGUI()
    {
        var originalCol = GUI.color;
        EditorGUILayout.LabelField("CONTROL MEDIANTE GRILLA", style);
        EditorGUILayout.Space();
        if (!grid.activatedGrid)
            GUI.color = Color.red;

        if(GUILayout.Button("SIN GRILLA", GUILayout.Height(75)))
        {
            grid.AreGizmosShutDown = false;
            GameObject.Destroy(boxQ);
            boxQ = null;
            grid.activatedGrid = false;
            grid.showLogs = true;
            Repaint();
        }
        GUI.color = originalCol;
        EditorGUILayout.Space();

        if (grid.activatedGrid)
            GUI.color = Color.green;
        if (GUILayout.Button("CON GRILLA", GUILayout.Height(75)))
        {
            grid.AreGizmosShutDown = false;
            GameObject.Destroy(boxQ);
            boxQ = null;
            grid.activatedGrid = true;
            grid.showLogs = true;
            Repaint();
        }

        GUI.color = originalCol;
        for (int i = 0; i < 3; i++)
            EditorGUILayout.Space();

        EditorGUILayout.LabelField("QUERIES", style);
        EditorGUILayout.Space();

        if (GUILayout.Button("BOX QUERY", GUILayout.Height(50)))
        {
            if(boxQ == null)
            {
                var g = new GameObject();
                boxQ = g.AddComponent<Queries>();
                boxQ.targetGrid = grid;
            }
            boxQ.isBox = true;
            grid.AreGizmosShutDown = true; 
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("CIRCLE QUERY", GUILayout.Height(50)))
        {
            if (boxQ == null)
            {
                var g = new GameObject();
                boxQ = g.AddComponent<Queries>();
                boxQ.targetGrid = grid;
            }
            boxQ.isBox = false;
            grid.AreGizmosShutDown = true;
        }

    }
}
