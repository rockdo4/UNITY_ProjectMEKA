using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrangeJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Count
    }

    private List<GameObject> directions = new List<GameObject>();
    private RaycastHit hit;
    private GameObject prevHit;
    private Transform player;
    private BoxCollider boxCollider;
    private float half;
    public float yOffset = 1f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;
        Debug.Log(player.gameObject.name);

        var parent = transform.parent;
        for (int i = 0; i < (int)Direction.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("버튼다운");
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그중");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << LayerMask.NameToLayer("Arrange");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if(hit.transform.gameObject != prevHit)
            {
                prevHit = hit.transform.gameObject;
            }

            var pos = hit.point;
            transform.position = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hit.transform.gameObject != null)
        {
            if(directions.Contains(hit.transform.parent.gameObject))
            {
                Debug.Log($"배치가능 : {hit.transform.gameObject.name}");
                var pos = hit.transform.parent.position;
                var go = hit.transform.parent.gameObject;
                // pos에서 핸들러의 half만큼 빼기
                if (go == directions[(int)Direction.Up])
                {
                    pos.z += -half;
                    player.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else if (go == directions[(int)Direction.Right])
                {
                    pos.x += -half;
                    player.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
                else if (go == directions[(int)Direction.Down])
                {
                    pos.z += half;
                    player.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else
                {
                    pos.x += half;
                    player.rotation = Quaternion.Euler(0f, -90f, 0f);
                }

                transform.position = pos;
            }
        }
        else
        {
            Debug.Log($"배치불가 : {prevHit}");
            var pos = prevHit.transform.parent.position;
            var go = hit.transform.parent.gameObject;
            // pos에서 핸들러의 half만큼 빼기
            if (go == directions[(int)Direction.Up])
            {
                pos.z += -half;
            }
            else if (go == directions[(int)Direction.Right])
            {
                pos.x += -half;
            }
            else if (go == directions[(int)Direction.Down])
            {
                pos.z += half;
            }
            else
            {
                pos.x += half;
            }

            transform.position = pos;
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
