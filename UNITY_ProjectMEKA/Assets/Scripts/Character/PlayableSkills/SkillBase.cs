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

   

    public abstract void UseSkill();
}
