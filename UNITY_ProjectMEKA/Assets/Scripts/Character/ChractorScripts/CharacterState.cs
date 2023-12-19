using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;


public class CharacterState : MonoBehaviour
{
    
    public int[,] AttackRange;

    [SerializeField, Header("ID 설정")]//p
    public int id;

    [SerializeField, Header("이름 설정")]//p
    public string name;

    [SerializeField, Header("속성 설정")]//p,e
    public Defines.Property property;

    [SerializeField, Header("체력 설정")]//p,e
    public float maxHp;

    [SerializeField, Header("방어력 설정")]//p,e
    public float armor;

    [SerializeField, Header("공격력 설정")]//p,e
    public float damage;

    [SerializeField, Header("공격 딜레이 설정")]//p,e
    public float attackDelay;



    [SerializeField, Header("원거리 타입일시 장착할 발사체")]//p,e
    public string BulletName;

    [SerializeField, Header("발사체 타입")]//p,e
    public ProjectileType BulletType;

    [SerializeField, Header("발사할때 이팩트")]//p,e
    public string flashName;

    [SerializeField, Header("맞았을때 이팩트")]//p,e
    public string hitName;

    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;

    public void ConvertTo2DArray()
    {
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        AttackRange = new int[hang, yal];

        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

    }

    [HideInInspector]
    public int level;//레벨

    [HideInInspector]
    public int grade;//등급 레벨의 10의자리에 해당하며 6까지만 존재가능

    [HideInInspector]
    public float Hp;

    [HideInInspector]
    public int ID;

    private void Awake()
    {
        Hp = maxHp;
    }
   
}
