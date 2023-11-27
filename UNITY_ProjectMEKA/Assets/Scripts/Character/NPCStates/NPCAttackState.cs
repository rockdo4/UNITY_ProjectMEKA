using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCAttackState : NPCBaseState
{
    private float timer;
    public NPCAttackState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        
        timer = 0;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if(timer > enemyCtrl.state.attackDelay)
        {
            timer = 0;
            enemyCtrl.ani.SetTrigger("Attack");
            //enemyCtrl.Hit();//test
        }
        if(enemyCtrl.target == null)
        {
            enemyCtrl.SetState(NPCStates.Move);
        }
    }
}
