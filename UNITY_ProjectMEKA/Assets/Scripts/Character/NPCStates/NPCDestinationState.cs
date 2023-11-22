using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDestinationStates : NPCBaseState
{
    private float timer;
    private float timerange;
    
    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        timerange = 1;
        // 11.22, 김민지, 이동 기능 변경
        //agent.isStopped = false;
    }

    public override void Exit()
    {
        // 11.22, 김민지, 이동 기능 변경
        //agent.isStopped = true;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerange)
        {
            // 11.22, 김민지, 이동 기능 변경
            //timer = 0;
            //agent.SetDestination(wayPoint[0].position);
            //if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            //{
            //    enemyCtrl.SetState(EnemyController.NPCStates.Idle);
            //}

            // 
        }

        Collider[] colliders = Physics.OverlapSphere(enemyCtrl.transform.position, enemyCtrl.state.range);

        foreach (Collider co in colliders)
        {
            if (co.CompareTag("Player"))
            {
                enemyCtrl.target = co.gameObject;
                PlayerController pl = co.GetComponent<PlayerController>();  
                if(pl.blockCount < pl.maxBlockCount)
                {
                    enemyCtrl.SetState(EnemyController.NPCStates.Attack);
                }

                break;
            }
        }
    }


}
