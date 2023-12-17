using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCIdleState : NPCBaseState
{

    public NPCIdleState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
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
        Debug.Log(enemyCtrl.name + "±âº»Áß");
        //timer += Time.deltaTime;
        //if (timer > enemyCtrl.state.attackDelay)
        //{
        //    timer = 0;
        //}
        enemyCtrl.SetState(NPCStates.Attack);
    }


}
