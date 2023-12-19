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
        var x = Mathf.RoundToInt(coords.x);
        var y = Mathf.RoundToInt(coords.y);
        var z = Mathf.RoundToInt(coords.z);

        return new Vector3Int(x, y, z);
    }
}
