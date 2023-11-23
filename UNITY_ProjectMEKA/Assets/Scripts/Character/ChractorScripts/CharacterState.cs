using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterState : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Aoe,
        PiercingShot,
    }

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
    [SerializeField, Header("이동속력 설정")]
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


    [HideInInspector]
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

    private void Awake()
    {
        Hp = maxHp;
    }
   
}
