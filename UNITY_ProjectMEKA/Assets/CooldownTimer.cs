using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;
using static UnityEngine.Rendering.DebugUI;

public class CooldownTimer : MonoBehaviour
{
    private Image cooldownImage; // Inspector에서 할당
    private StageManager stageManager;
    private float originalWidth;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        cooldownImage = GetComponent<Image>();
        originalWidth = cooldownImage.rectTransform.sizeDelta.x;

    }
    private void Update()
    {
        if (stageManager == null || stageManager.currentPlayer == null ||
       stageManager.currentPlayer.skillState == null || cooldownImage == null)
        {
            return;
        }

        float hpFraction = stageManager.currentPlayer.skillState.currentSkillTimer /
                           stageManager.currentPlayer.skillState.skillCoolTime;

        cooldownImage.rectTransform.sizeDelta = new Vector2(originalWidth * hpFraction,
                                                            cooldownImage.rectTransform.sizeDelta.y);

        if (hpFraction >= 1)
        {
            cooldownImage.rectTransform.sizeDelta = new Vector2(originalWidth,
                                                                cooldownImage.rectTransform.sizeDelta.y);
        }
    }

}
