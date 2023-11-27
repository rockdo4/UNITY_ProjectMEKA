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

    private void Awake()
    {
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
            var pos = hit.transform.position;
            transform.position = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hit.transform != null && directions.Contains(hit.transform.gameObject))
        {
            Debug.Log("��ġ����");
            // ĳ���� ���� �ű��
        }
        else
        {
            Debug.Log("��ġ�Ұ�");
        }
    }

}
