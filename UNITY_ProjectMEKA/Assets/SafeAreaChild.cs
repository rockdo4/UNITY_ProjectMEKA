using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaChild : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Rect safeArea;
    private Vector2 anchorMin;
    private Vector2 anchorMax;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();

        safeArea = Screen.safeArea;

        // Screen.safeArea�� �θ� RectTransform�� ũ�⿡ �°� ����
        Vector2 parentSize = parentRectTransform.rect.size;
        anchorMin = safeArea.position;
        anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= parentSize.x;
        anchorMin.y /= parentSize.y;
        anchorMax.x /= parentSize.x;
        anchorMax.y /= parentSize.y;
        anchorMax.x *= 0.5f;
        // �ϴ� �� ���� UI ��Ҹ� ���� �߰����� ����
        float barHeight = 0.1f;
        anchorMin.y = Mathf.Max(anchorMin.y, 0); // �ϴ� "Safe Area" ��踦 ���
        anchorMax.y = Mathf.Min(anchorMax.y, barHeight); // ���� �ִ� ���̸� ���

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
