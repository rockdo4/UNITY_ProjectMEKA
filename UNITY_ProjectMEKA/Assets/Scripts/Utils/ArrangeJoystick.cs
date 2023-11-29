using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerController;

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
    private Bounds backgroundBounds;
    private List<Bounds> bounds;
    private GameObject currentTile;
    private Transform player;
    private BoxCollider boxCollider;
    private float half;
    [HideInInspector]
    public float yOffset = 1f;
    [HideInInspector]
    public bool secondArranged;
    private GameObject firstArranger;

    private void OnEnable()
    {
        secondArranged = false;
        boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;
        backgroundBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

        var parent = transform.parent;
        for (int i = 0; i < (int)Direction.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }

        bounds = new List<Bounds>
        {
            new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f))
        };
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!secondArranged)
        {
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!secondArranged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;
                transform.localPosition = backgroundBounds.ClosestPoint(transform.localPosition);
            }

            for(int i = 0; i < (int)Direction.Count; ++i)
            {
                if (bounds[i].Contains(transform.localPosition))
                {
                    currentTile = directions[i];
                    RotatePlayer(currentTile.transform, false);
                    break;
                }
            }
        }

        // 드래그하고 있을 때, currentTile 방향으로 공격범위 보이게 세팅
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!secondArranged)
        {
            RotatePlayer(currentTile.transform, true);
            secondArranged = true;
            Time.timeScale = 1f;
            firstArranger.SetActive(false);
            transform.localPosition = Vector3.zero;
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void RotatePlayer(Transform currentTileParent, bool mouseUp)
    {
        var pos = currentTileParent.position;
        var go = currentTileParent.gameObject;
        // pos에서 핸들러의 half만큼 빼기
        if (go == directions[(int)Direction.Up])
        {
            //Debug.Log("rotate Player");
            if (mouseUp)
            {
                pos.z += -half;
            }
            else
            {
                //Debug.Log("up direction");
                player.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (go == directions[(int)Direction.Right])
        {
            if (mouseUp)
            {
                pos.x += -half;
            }
            else
            {
                player.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
        }
        else if (go == directions[(int)Direction.Down])
        {
            if (mouseUp)
            {
                pos.z += half;
            }
            else
            {
                player.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        else
        {
            if (mouseUp)
            {
                pos.x += half;
            }
            else
            {
                player.rotation = Quaternion.Euler(0f, -90f, 0f);
            }
        }

        if(mouseUp)
        {
            transform.position = pos;
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    public void SetFirstArranger(GameObject arranger)
    {
        firstArranger = arranger;
    }
}
