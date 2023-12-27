using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class CooldownTimer : MonoBehaviour
{
    private Image cooldownImage; // Inspector에서 할당
    public float cooldownDuration = 5f; // 쿨다운 시간 (초)
    private StageManager stageManager;
    private float cooldownTimer;
    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        cooldownImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (stageManager == null || stageManager.currentPlayer == null || stageManager.currentPlayer.skillState == null || cooldownImage == null)
        {
            return;
        }
        cooldownImage.fillAmount = stageManager.currentPlayer.skillState.currentSkillTimer / stageManager.currentPlayer.skillState.skillCoolTime;//20번쨰 줄
        
    }

}
