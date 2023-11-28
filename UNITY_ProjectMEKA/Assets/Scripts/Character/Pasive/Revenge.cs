using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revenge : MonoBehaviour
{
    EnemyController enemy;
    float saveCurrentHp;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    void OnEnable()
    {
        saveCurrentHp = enemy.state.Hp;
    }

    void Update()
    {
        if (saveCurrentHp > enemy.state.Hp)
        {
            saveCurrentHp = enemy.state.Hp;
            TakeDamage co = enemy.HoIsHitMe.GetComponent<TakeDamage>();
            co.OnAttack(enemy.state.damage * 0.2f);
            
        }
    }
}
