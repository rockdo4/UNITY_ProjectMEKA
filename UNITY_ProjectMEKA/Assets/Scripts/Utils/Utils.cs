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
        //레이를 쏴서 맞은 게 커런트 플레이어면, true 반환
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
        var x = Mathf.FloorToInt(coords.x);
        var y = Mathf.FloorToInt(coords.y);
        var z = Mathf.FloorToInt(coords.z);

        return new Vector3Int(x, y, z);
    }

    public static int[,] RotateArray(int[,] arr, int rotationCount)
    {
        // 회전 횟수를 4로 나눈 나머지를 구하여 불필요한 회전을 최소화합니다.
        rotationCount %= 4;

        // 배열의 행과 열의 크기
        int rowCount = arr.GetLength(0);
        int colCount = arr.GetLength(1);

        for (int r = 0; r < rotationCount; r++)
        {
            // 시계 방향으로 90도 회전한 배열을 저장할 새로운 배열을 생성합니다.
            int[,] rotatedArr = new int[colCount, rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    rotatedArr[j, rowCount - 1 - i] = arr[i, j];
                }
            }

            // 다음 회전을 위해 arr를 rotatedArr로 업데이트하고, 행과 열의 크기를 교환합니다.
            arr = rotatedArr;
            int temp = rowCount;
            rowCount = colCount;
            colCount = temp;
        }

        // 최종적으로 회전된 배열을 반환합니다.
        return arr;
    }
}
