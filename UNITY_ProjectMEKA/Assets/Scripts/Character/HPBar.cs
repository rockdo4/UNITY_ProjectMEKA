using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Image image;
    private Canvas canvas;
    private CharacterState meme;
    private float originalWidth;

    void Start()
    {
        image = GetComponent<Image>();
        meme = GetComponentInParent<CharacterState>();
        canvas = GetComponentInParent<Canvas>();
        originalWidth = image.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);

        float hpFraction = meme.Hp / meme.maxHp;
        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);
    }
}

