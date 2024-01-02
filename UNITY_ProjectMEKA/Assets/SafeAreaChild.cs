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

        // Screen.safeArea를 부모 RectTransform의 크기에 맞게 조정
        Vector2 parentSize = parentRectTransform.rect.size;
        anchorMin = safeArea.position;
        anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= parentSize.x;
        anchorMin.y /= parentSize.y;
        anchorMax.x /= parentSize.x;
        anchorMax.y /= parentSize.y;
        anchorMax.x *= 0.5f;
        // 하단 바 형태 UI 요소를 위한 추가적인 조정
        float barHeight = 0.1f;
        anchorMin.y = Mathf.Max(anchorMin.y, 0); // 하단 "Safe Area" 경계를 고려
        anchorMax.y = Mathf.Min(anchorMax.y, barHeight); // 바의 최대 높이를 고려

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
