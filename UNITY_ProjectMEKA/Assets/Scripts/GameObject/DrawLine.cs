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
    private float tempAlphaValue;
    private float fadeSpeed = 0.01f;
    private bool lineOn;

    void Start()
    {
        Debug.Log("DrawLine Start");
        lineRenderer = GetComponent<LineRenderer>();
        yPos = 0.7f;
        tempAlphaValue = 0f;
        var color = new Color(255, 255, 255, tempAlphaValue);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // Ω√¿€ 0~255
        if(lineOn && tempAlphaValue <= 255f)
        {
            tempAlphaValue += Time.timeScale * fadeSpeed;
            var color = new Color(255, 255, 255, tempAlphaValue);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
        // ≥° 255~0
        else if(!lineOn && tempAlphaValue >= 0f)
        {
            tempAlphaValue -= Time.timeScale * fadeSpeed;
            var color = new Color(255, 255, 255, tempAlphaValue);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }

    public void SetPoints(Transform[] wayPoints)
    {
        lineOn = true;
        gameObject.SetActive(true);
        Debug.Log("SetPoints");
        var count = wayPoints.Length + 1;
        points = new Vector3[count];

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

            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    public void ErasePoints()
    {
        lineOn = false;

        //lineRenderer.positionCount = 0;
        points = new Vector3[0];
    }
}
