using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingSkillType : SkillBase
{
    
    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;

    [SerializeField, Header("데미지 타이밍 프레임 시작 몇초후")]
    public float delay;
    [SerializeField, Header("공격 애니매이션이 따로 있는가")]
    public bool isAttackAnimation;

    public void ConvertTo2DArray()
    {
        // 1차원 배열의 길이가 행과 열의 곱과 일치하는지 확인
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        // 새 2차원 배열 생성
        AttackRange = new int[hang, yal];

        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

    }
    public override void UseSkill()
    {
        switch (skillType)
        {
            case Defines.SkillType.SnipingSingle:
                break;
            case Defines.SkillType.SnipingArea:

                break;

        }
        
    }
    public void IsSkillAttackArea()
    {

    }

}
