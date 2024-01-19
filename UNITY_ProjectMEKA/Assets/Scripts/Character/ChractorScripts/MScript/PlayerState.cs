using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class PlayerState : CharacterState
{
    //public enum Type
    //{
    //    Bullet,
    //    Aoe,
    //    PiercingShot,
    //    ChainAttack,
    //    HitScan,
    //    Instantaneous,
    //}
    //public enum Passive
    //{
    //    None,//없음
    //    Unstoppable,//저지 불가
    //    Explosion,//자폭
    //    BusterCall,//지원 전술
    //    SpeedUp,//이속 증가
    //    Counterattack,//역습
    //    Spite,//악의
    //    Outlander,//아웃랜더
    //    Tenacity,//망자의 집념
    //    Revenge,//보복
    //    Mechanic,//정비공

    //}
    //public enum Skills
    //{
    //    None,
    //    Snapshot,
    //    StunningBlow,
    //    Test2,
    //    Test3,
    //}

    ////공격 가능범위
    //public int[,] AttackRange;

    //[SerializeField, Header("캐릭터 ID 설정")]//p
    //public int id;

    //[SerializeField, Header("캐릭터 이름 설정")]//p
    //public string name;

    [SerializeField, Header("캐릭터 배치 코스트")]//p
    public int arrangeCost;

    [SerializeField, Header("캐릭터 배치 쿨타임")]//p
    public float arrangeCoolTime;

    [SerializeField, Header("캐릭터 스킬 설정")]//p
    public Skills skill;

    [HideInInspector]
    public float cost;

    [SerializeField, Header("캐릭터 최대 시그마")]//p
    public float maxCost;

    [SerializeField, Header("캐릭터 스킬 코스트")]//p
    public float skillCost;

    [SerializeField, Header("스킬 쿨타임")]//p
    public float skillCoolTime;

    [SerializeField, Header("스킬 지속시간")]//p
    public float skillDuration;

    [SerializeField, Header("캐릭터 클래스 설정")]//p
    public Defines.Occupation occupation;

    [SerializeField, Header("치명타 확률 0 ~ 1")]//p
    public float critChance;

    [SerializeField, Header("치명타 피해 퍼센트 1 = 100")]//p
    public float fatalDamage = 1;

    [SerializeField, Header("공중유닛 우선타겟인가")]
    public bool isFirstTargetIsFly;

    [HideInInspector]
    public int experience;//이 경험치 이상이여야 레벨에 ++

    [HideInInspector]
    public int currentExperience;

}
