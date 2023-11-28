using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    float timer = 3f;
    EnemyController enemy;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 )
        {
            timer = 3f;
            if(enemy.state.Hp <=enemy.state.maxHp) 
            {
                enemy.state.Hp += enemy.state.maxHp * 0.05f;
                //enemy.state.Hp += enemy.state.damage * 500f;
                if(enemy.state.Hp >= enemy.state.maxHp)
                {
                    enemy.state.Hp = enemy.state.maxHp;
                }
            }
        }


    }
}
