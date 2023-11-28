using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Aoe,
        PiercingShot,
    }
    public enum Passive
    {
        None,//없음
        Unstoppable,//저지 불가
        Explosion,//자폭
        BusterCall,//지원 전술
        SpeedUp,//이속 증가
        Counterattack,//역습
        Spite,//악의
        Outlander,//아웃랜더
        Tenacity,//망자의 집념
        Revenge,//보복
        Mechanic,//정비공

    }

    //공격 가능범위
    public int[,] AttackRange;
    

    [SerializeField, Header("적 패시브 설정")]
    public List<Passive> passive;

    [SerializeField, Header("적 타입 설정")]
    public Defines.EnemyType enemyType;
    
    [SerializeField, Header("속성 설정")]
    public Defines.Property property;

    [SerializeField, Header("캐릭터 클래스 설정")]
    public Defines.Occupation occupation;

    [SerializeField, Header("체력 설정")]
    public float maxHp;

    [SerializeField, Header("방어력 설정")]
    public float amror;

    // 11.22, 김민지, 적 이동시 필요
    [SerializeField, Header("이동속력 설정,적 이동시 필요")]
    public float speed;

    [SerializeField, Header("스킬 쿨타입 설정")]
    public float cooldown;

    [SerializeField, Header("공격 사정거리 설정")]
    public float range;

    [SerializeField, Header("공격력 설정")]
    public float damage;

    [SerializeField, Header("공격 딜레이 설정")]
    public float attackDelay;

    [SerializeField, Header("경험치량 설정 몬스터 전용")]
    public int exp;

    [SerializeField, Header("원거리 타입일시 장착할 발사체")]
    public string BulletName;

    [SerializeField, Header("발사체 타입")]
    public Type BulletType;

    [SerializeField, Header("발사할때 이팩트")]
    public string flashName;

    [SerializeField, Header("맞았을때 이팩트")]
    public string hitName;

    [SerializeField, Header("행")]
    public int hang;
    [SerializeField, Header("열")]
    public int yal;
    [SerializeField, Header("공격범위설정")]
    public int[] rangeAttack;
    
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

        // 1차원 배열의 데이터를 2차원 배열로 변환
        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

        //return AttackRange; // 변환된 2차원 배열 반환
    }

    //[HideInInspector]
    public int level;//레벨

    [HideInInspector]
    public int grade;//등급 레벨의 10의자리에 해당하며 6까지만 존재가능

    [HideInInspector]
    public int experience;//이 경험치 이상이여야 레벨에 ++

    [HideInInspector]
    public int currentExperience;

    [HideInInspector]
    public float Hp;

    [HideInInspector]
    public int ID;

    [HideInInspector]
    public bool isBlock = true;

    private void Awake()
    {
        Hp = maxHp;
    }
   
}
