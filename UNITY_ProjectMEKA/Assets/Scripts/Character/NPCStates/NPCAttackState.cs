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
        Debug.Log("어택상태 들어옴");
        timer = 0;
    }

    public override void Exit()
    {
        Debug.Log("어택상태 나감");
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        ////enemyCtrl.ani.SetTrigger("Attack");
        //Debug.Log(enemyCtrl.name + 
        //"공격중");

        timer += Time.deltaTime;
        if (timer > enemyCtrl.state.attackDelay)
        {
            timer = 0;
            if(enemyCtrl.state.enemyType == Defines.EnemyType.OhYaBung)
            {
                if(enemyCtrl.rangeInPlayers.Count >= 2f)
                {
                    enemyCtrl.ani.SetTrigger("Attack");
                }
                else if(enemyCtrl.rangeInSecondPlayers.Count >= 1f)
                {
                    enemyCtrl.ani.SetTrigger("SecondAttack");
                }

            }
            else
            {
                enemyCtrl.ani.SetTrigger("Attack");
            }
        }
        if(enemyCtrl.target == null)
        {
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }
        if (enemyCtrl.target.GetComponentInParent<PlayerController>() == null)
        {
            Debug.Log("target null");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }
        if(!enemyCtrl.target.activeInHierarchy)
        {
            Debug.Log("Target not avtive");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }

        foreach (var a in enemyCtrl.rangeInPlayers)
        {
            if (a.GetComponentInParent<PlayerController>() == null)
            {
                if(enemyCtrl.rangeInPlayers.Contains(a))
                {
                    enemyCtrl.rangeInPlayers.Remove(a);

                }
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }
            else if (!a.activeSelf)
            {
                enemyCtrl.rangeInPlayers.Remove(a);
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }

        }


    }
}
