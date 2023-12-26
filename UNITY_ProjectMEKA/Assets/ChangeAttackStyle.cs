using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeAttackStyle : MonoBehaviour
{
    private EnemyController enemy;
    public RuntimeAnimatorController newAnimation;
    void Start()
    {
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
        if(enemy.bossAttackCount >= 20)
        {
            enemy.ani.runtimeAnimatorController = newAnimation;
        }
    }
}
