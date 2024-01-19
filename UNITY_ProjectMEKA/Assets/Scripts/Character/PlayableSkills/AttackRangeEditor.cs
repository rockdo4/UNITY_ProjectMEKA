using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AttackRangeEditor : EditorWindow
{

    private int rows = 0;
    private int columns = 0;
    private int[,] array;
    private Vector2 scrollPos;

    [MenuItem("Window / AttackRangeSeting")]
    public static void Window()
    {
        EditorWindow.GetWindow<AttackRangeEditor>("AttackRage");
    }
    void OnGUI()
    {
        GUILayout.Label("Array Size", EditorStyles.boldLabel);

        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);

        if (GUILayout.Button("Create Array"))
        {
            array = new int[rows, columns];
        }

        if (array != null)
        {
            DrawArrayEditor();
        }

        if (GUILayout.Button("Save"))
        {
            // 여기에 배열 데이터 저장 로직을 추가합니다.
            Debug.Log("Array Saved");
        }
    }

    void DrawArrayEditor()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < rows; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < columns; j++)
            {
                array[i, j] = EditorGUILayout.IntField(array[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }


}
