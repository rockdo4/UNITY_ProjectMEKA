using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Defines;

public static class Utils
{
    public static bool IsUILayer()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                //Debug.Log("Hit " + result.gameObject.name, result.gameObject);
                var layerName = LayerMask.LayerToName(result.gameObject.layer);

                switch(layerName)
                {
                    case "UI":
                    case "ArrangeTile":
                        return true;
                    default:
                        return false;
                }
            }
        }
        return false;
    }

    public static bool IsCurrentPlayer(GameObject player)
    {
        //���̸� ���� ���� �� Ŀ��Ʈ �÷��̾��, true ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject == player)
            {
                return true;
            }
        }
        return false;
    }

    public static List<string> GetSceneNames(StageClass stageClass)
    {
        List<string> sceneNames = new List<string>();
        
        switch (stageClass)
        {
            case StageClass.Story:
                foreach (var field in typeof(StorySceneNames).GetFields())
                {
                    if (field.IsLiteral && !field.IsInitOnly)
                    {
                        sceneNames.Add((string)field.GetValue(null));
                    }
                }
                break;
            case StageClass.Assignment:
                foreach (var field in typeof(AssignmentSceneNames).GetFields())
                {
                    if (field.IsLiteral && !field.IsInitOnly)
                    {
                        sceneNames.Add((string)field.GetValue(null));
                    }
                }
                break;
            case StageClass.Challenge:
                foreach (var field in typeof(ChallengeSceneNames).GetFields())
                {
                    if (field.IsLiteral && !field.IsInitOnly)
                    {
                        sceneNames.Add((string)field.GetValue(null));
                    }
                }
                break;
        }
        return sceneNames;
    }

    public static Vector3Int Vector3ToVector3Int(Vector3 coords)
    {
        // floorToInt -> tile positions unaccurate
        var x = Mathf.RoundToInt(coords.x);
        var z = Mathf.RoundToInt(coords.z);
        var y = Mathf.RoundToInt(coords.y);

        return new Vector3Int(x, y, z);
    }

    public static int[,] RotateArray(int[,] arr, int rotationCount)
    {
        rotationCount %= 4;

        int rowCount = arr.GetLength(0);
        int colCount = arr.GetLength(1);

        for (int r = 0; r < rotationCount; r++)
        {
            int[,] rotatedArr = new int[colCount, rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    rotatedArr[j, rowCount - 1 - i] = arr[i, j];
                }
            }

            arr = rotatedArr;
            int temp = rowCount;
            rowCount = colCount;
            colCount = temp;
        }

        return arr;
    }
}
