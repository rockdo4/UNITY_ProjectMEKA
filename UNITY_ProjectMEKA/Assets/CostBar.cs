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
        canvas.transform.LookAt(Camera.main.transform);

        float hpFraction;

        hpFraction = player.state.cost / player.state.maxCost;
        
        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);

    }
}
