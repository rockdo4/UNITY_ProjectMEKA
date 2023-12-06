using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStunState : NPCBaseState
{
    float timer;
    public NPCStunState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        Debug.Log("NPC Is Stun");
        enemyCtrl.ani.SetTrigger("Idle");
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
        if(timer >= 3f)
        {
            timer = 0;
            enemyCtrl.SetState(NPCStates.Attack);
        }
    }
}
