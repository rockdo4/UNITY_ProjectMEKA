using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GateController;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Vector3[] points;
    public float yPos;
    public float drawSpeed;
    public Vector3 startPoint;

    void Start()
    {
        Debug.Log("DrawLine Start");
        lineRenderer = GetComponent<LineRenderer>();
        // 모든 포인트 순회하면서 ypos 조작
        //for(int i = 0; i < points.Length; i++)
        //{
        //    // yPos modefy
        //    var point = points[i].position;
        //    point.y = yPos;
        //    points[i].position = point;

        //    // lineRenderer pointCount, setPosition
        //    lineRenderer.positionCount = i + 1;
        //    lineRenderer.SetPosition(i, points[i].position);
        //}
    }

    public void SetPoints(Transform[] wayPoints, Transform house)
    {
        Debug.Log("SetPoints");
        var count = wayPoints.Length + 1;
        points = new Vector3[count];
        //points[0] = startPoint;
        //var startPos = startPoint;
        //startPos.y = yPos;
        //points[0] = startPos;
        
        //points[points.Length - 1] = house;

        for (int i = 0; i < count; i++)
        {
            // yPos modefy
            if(i == 0)
            {
                //points[i] = startPoint;
                var startPos = startPoint;
                startPos.y = yPos;
                points[i] = startPos;
            }
            else
            {
                //points[i] = wayPoints[i - 1].position;
                var point = wayPoints[i - 1].position;
                point.y = yPos;
                points[i] = point;
            }

            // lineRenderer pointCount, setPosition
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    //IEnumerator DrawLineOverTime()
    //{
    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        lineRenderer.positionCount = i + 1;
    //        lineRenderer.SetPosition(i, points[i].position);
    //        yield return new WaitForSeconds(1f / drawSpeed);
    //    }
    //}

    //IEnumerator DrawLineSmoothly()
    //{
    //    Vector3 startPosition = points[0].position; //points[1].transform.parent.transform.InverseTransformPoint(points[0].parent.TransformPoint(Vector3.zero));

    //    for (int i = 1; i < points.Length; i++)
    //    {
    //        Vector3 endPosition = points[i].position;
    //        float t = 0f;
    //        while (t < 1f)
    //        {
    //            t += Time.deltaTime * drawSpeed;
    //            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
    //            lineRenderer.positionCount = i + 1;
    //            lineRenderer.SetPosition(i, newPosition);
    //            yield return null;
    //        }
    //        startPosition = endPosition;
    //    }
    //}

    public void ErasePoints()
    {
        lineRenderer.positionCount = 0;
        points = new Vector3[0];
    }
}
