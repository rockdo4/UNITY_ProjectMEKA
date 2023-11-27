using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spite : MonoBehaviour
{
    //본 패시브를 가진 몬스터는 공격 중인 캐릭터의 
    //    잔여 체력이 50% 미만일 때 2배의 대미지를 준다. 
    //    (본 효과의 2배 가산은 속성별 대미지까지 계산한 후 가장 마지막에 처리한다.)

    private EnemyController enemy;
    private float saveDamage;
    private float dubleDamage;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    private void Start()
    {
        saveDamage = enemy.state.damage;
        dubleDamage = enemy.state.damage * 2f;
    }
    private void Update()
    {
        if((enemy.target.GetComponent<PlayerController>().state.Hp/ enemy.target.GetComponent<PlayerController>().state.maxHp) <= 0.5f)
        {
            enemy.state.damage = dubleDamage;
        }
        else
        {
            enemy.state.damage = saveDamage;
        }
    }
}
