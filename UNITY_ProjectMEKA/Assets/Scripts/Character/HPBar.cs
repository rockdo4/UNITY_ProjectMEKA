using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Image image;
    private CharacterState meme;
    private float originalWidth;

    void Start()
    {
        image = GetComponent<Image>();
        meme = GetComponentInParent<CharacterState>();
        originalWidth = image.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        float hpFraction = meme.Hp / meme.maxHp;
        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);
    }
}

