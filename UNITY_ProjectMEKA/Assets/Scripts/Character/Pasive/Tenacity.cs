using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenacity : MonoBehaviour
{
    EnemyController enemy;
    bool isOne = false;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    private void OnEnable()
    {
        isOne = false;
    }
    void Update()
    {
        if(enemy.state.Hp <=0)
        {
            if (!isOne)
            {
                TakeDamage co = enemy.HoIsHitMe.GetComponent<TakeDamage>();
                co.OnAttack(enemy.state.damage * 5f);
                isOne = true;
            }
        }
        
    }
}
