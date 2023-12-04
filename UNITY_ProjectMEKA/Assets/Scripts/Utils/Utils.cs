using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
}
