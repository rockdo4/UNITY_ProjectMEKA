using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image shield;

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

        if(player != null)
        {
            if(player.maxShield == 0 )
            {
				shield.gameObject.SetActive(false);
			}
        }
    }

    void Update()
    {
        //canvas.transform.LookAt(Camera.main.transform);

        float hpFraction;
        float shieldFraction;

        if(player != null ) 
        {
            hpFraction = player.Hp / player.maxHp;
            shieldFraction = player.shield / player.maxShield;
        }
        else
        {
            hpFraction = enemy.Hp / enemy.maxHp;
            shieldFraction = enemy.shield / enemy.maxShield;
        }

        image.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction, image.rectTransform.sizeDelta.y);
        shield.rectTransform.sizeDelta = new Vector2(originalWidth * shieldFraction, shield.rectTransform.sizeDelta.y);
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

