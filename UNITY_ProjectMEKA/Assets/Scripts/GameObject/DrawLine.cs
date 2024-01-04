using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GateController;

public class DrawLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform[] points;
    public float yPos;
    public float drawSpeed;
    [HideInInspector]
    public List<WaveInfo> waveInfos;

    void Start()
    {
        Debug.Log("DrawLine Start");
        lineRenderer = GetComponent<LineRenderer>();
        // 모든 포인트 순회하면서 ypos 조작
        // 동시에 라인렌더러에 포인트카운트, setposition
        //StartCoroutine(DrawLineOverTime());
    }

    IEnumerator DrawLineOverTime()
    {
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, points[i].position);
            yield return new WaitForSeconds(1f / drawSpeed);
        }
    }

    IEnumerator DrawLineSmoothly()
    {
        Vector3 startPosition = points[0].position; //points[1].transform.parent.transform.InverseTransformPoint(points[0].parent.TransformPoint(Vector3.zero));

        for (int i = 1; i < points.Length; i++)
        {
            Vector3 endPosition = points[i].position;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * drawSpeed;
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, newPosition);
                yield return null;
            }
            startPosition = endPosition;
        }
    }

    public void ErasePoints()
    {
        lineRenderer.positionCount = 0;
    }
}
