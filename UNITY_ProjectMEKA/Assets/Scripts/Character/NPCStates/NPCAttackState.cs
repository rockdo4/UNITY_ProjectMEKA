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
        //enemyCtrl.ani.SetTrigger("Attack");

        timer += Time.deltaTime;
        if (timer > enemyCtrl.state.attackDelay)
        {
            timer = 0;
            enemyCtrl.ani.SetTrigger("Attack");
        }
        if (enemyCtrl.target.GetComponentInParent<PlayerController>() == null)
        {
            Debug.Log("target null");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }
        if(!enemyCtrl.target.activeSelf)
        {
            Debug.Log("Target not avtive");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }


        foreach(var a in enemyCtrl.rangeInPlayers)
        {
            if(a.GetComponentInParent<PlayerController>() == null)
            {
                enemyCtrl.rangeInPlayers.Remove(a);
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }
            else if(!a.activeSelf)
            {
                enemyCtrl.rangeInPlayers.Remove(a);
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }

        }
        

    }
}
