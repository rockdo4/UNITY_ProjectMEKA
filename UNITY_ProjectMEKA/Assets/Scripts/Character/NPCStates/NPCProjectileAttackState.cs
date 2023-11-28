using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCProjectileAttackState : NPCBaseState
{
    float timer;
    public NPCProjectileAttackState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = enemyCtrl.state.attackDelay;
            enemyCtrl.ani.SetTrigger("Attack");
            enemyCtrl.SetState(NPCStates.Move);
        }
        if (enemyCtrl.target == null)
        {
            enemyCtrl.SetState(NPCStates.Move);
        }


    }
}
