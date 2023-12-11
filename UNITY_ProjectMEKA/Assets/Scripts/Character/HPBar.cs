using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Image image;
    private Canvas canvas;
    private PlayerState player;
    private EnemyState enemy;
    private float originalWidth;

    void Start()
    {
        image = GetComponent<Image>();
        //meme = GetComponentInParent<CharacterState>();
        player = GetComponentInParent<PlayerState>();
        if(player == null )
        {
            enemy = GetComponentInParent<EnemyState>();
        }

        canvas = GetComponentInParent<Canvas>();
        originalWidth = image.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);

        float hpFraction;

        if(player != null ) 
        {
            hpFraction = player.Hp / player.maxHp;
        }
        else
        {
            hpFraction = enemy.Hp / enemy.maxHp;
        }

        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);
    }
}

