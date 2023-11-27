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
    private BoxCollider boxCollider;
    private float half;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;

        var parent = transform.parent;
        for (int i = 0; i < (int)Direction.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("��ư�ٿ�");
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("�巡����");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << LayerMask.NameToLayer("Arrange");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //var pos = hit.transform.position;
            //transform.position = pos;

            var pos = hit.point;
            transform.position = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(hit);
        if (hit.transform != null && directions.Contains(hit.transform.parent.gameObject))
        {
            Debug.Log($"��ġ���� : {hit.transform.gameObject.name}");
            var pos = hit.transform.parent.position;
            var go = hit.transform.parent.gameObject;
            // pos���� �ڵ鷯�� half��ŭ ����
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
            // ĳ���� ���� �ű��
        }
        else
        {
            Debug.Log($"��ġ�Ұ� : {hit}");
        }
    }

}
