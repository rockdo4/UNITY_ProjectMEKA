using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField, Header("스킬 코스트")]//p
    public float skillCost;

    [SerializeField, Header("스킬 쿨타임")]//p
    public float skillCoolTime;

    //어떤 스킬인지 표시: 자동, 즉발, 스나이핑 단일, 스나이핑 광역 
    [SerializeField, Header("스킬 타입")]
    public Defines.SkillType skillType;

    [HideInInspector]
    public bool isSkillUsing;

    [SerializeField, Header("기본 공격 범위를 무시하는가")]
    public bool isRangeOut;

    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;
    [HideInInspector]
    public List<Tile> attackableTiles;
    [HideInInspector]
    public int[,] attackRangeRot;
    public void ConvertTo2DArray()
    {
        // 1차원 배열의 길이가 행과 열의 곱과 일치하는지 확인
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        // 새 2차원 배열 생성
        AttackRange = new int[yal, hang];

        for (int i = 0; i < yal; i++)
        {
            for (int j = 0; j < hang; j++)
            {
                AttackRange[i, j] = rangeAttack[(hang - j - 1) * yal + i];
            }
        }
    }


    // 12.20, 김민지, 추가
    public List<GameObject> targetList = new List<GameObject>();

    public abstract void UseSkill();
}
