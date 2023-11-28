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
    [HideInInspector]
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
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = hitPoint;

            Bounds bounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));
            transform.localPosition = bounds.ClosestPoint(transform.localPosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hit.transform != null)
        {
            if(directions.Contains(hit.transform.parent.gameObject))
            {
                Debug.Log($"배치가능 : {hit}");
                RotatePlayer(hit.transform.parent);
            }
        }
        else
        {
            Debug.Log($"배치불가 : {prevHit}");
            RotatePlayer(prevHit.transform.parent);
        }
    }

    public void RotatePlayer(Transform hitGo)
    {
        var pos = hitGo.position;
        var go = hitGo.gameObject;
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

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
