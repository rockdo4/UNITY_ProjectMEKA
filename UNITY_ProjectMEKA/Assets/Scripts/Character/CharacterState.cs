using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{

    public Defines.Property property;
    public Defines.Occupation occupation;
    // 231121, 김민지, 적 스폰 시 필요해서 추가함
    public Defines.EnemyType enemyType;

    public float maxHp;
    public float amror;
    public int ID;
    public float cooldown;
    public float range;
    public float minDamage;
    public float damage;
    public float criticalChance; // 0.0 == 0 % ,1.0 == 100%
    public float criticalMultiplier;
    public float Hp;
    public float attackDelay;


    private void Awake()
    {
        Hp = maxHp;
    }
   
}
