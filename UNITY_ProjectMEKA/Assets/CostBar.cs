using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostBar : MonoBehaviour
{
    private Image image;
    private PlayerController player;
    private Canvas canvas;
    private float originalWidth;
    void Start()
    {
        image = GetComponent<Image>();
        player = GetComponentInParent<PlayerController>();
        canvas = GetComponentInParent<Canvas>();
        originalWidth = image.rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        //canvas.transform.LookAt(Camera.main.transform);

        float hpFraction;

        hpFraction = player.state.cost / player.state.maxCost;
        
        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);
        Vector3 toCamera = (Camera.main.transform.position - canvas.transform.position).normalized;

        Vector3[] directions = {
        Vector3.forward, // ºÏ
        Vector3.back,    // ³²
        Vector3.right,   // µ¿
        Vector3.left     // ¼­
        };

        float maxDot = -Mathf.Infinity;
        Vector3 nearestDirection = Vector3.forward;

        foreach (Vector3 dir in directions)
        {
            float dot = Vector3.Dot(toCamera, dir);
            if (dot > maxDot)
            {
                maxDot = dot;
                nearestDirection = dir;
            }
        }

        canvas.transform.rotation = Quaternion.LookRotation(nearestDirection);
    }



}
