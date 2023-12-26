using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeAttackStyle : MonoBehaviour
{
    private EnemyController enemy;
    public RuntimeAnimatorController newAnimation;
    private bool isOnes;
    void Start()
    {
        enemy = GetComponent<EnemyController>();
        isOnes = false;
    }
    private void OnDisable()
    {
        isOnes = false;
    }
    void Update()
    {
        if(enemy.bossAttackCount >= 20)
        {
            enemy.ani.runtimeAnimatorController = newAnimation;
            if(enemy.target != null && !isOnes) 
            {
                isOnes = true;
                enemy.ani.SetTrigger("Attack");
                enemy.SetState(NPCStates.Idle);
            }
        }
    }
}
