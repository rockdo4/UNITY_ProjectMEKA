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

            

            if (!CheckDistance())
            {
                enemyCtrl.SetState(NPCStates.Move);
            }
            
            
        }
        if (enemyCtrl.target == null)
        {
            enemyCtrl.SetState(NPCStates.Move);
        }


    }

    bool CheckDistance()
    {
        Vector3Int currentGridPosition = new Vector3Int(
                Mathf.FloorToInt(enemyCtrl.transform.position.x),
                Mathf.FloorToInt(enemyCtrl.transform.position.y),
                Mathf.FloorToInt(enemyCtrl.transform.position.z)
        );

        Vector3Int targetGridPosition = new Vector3Int(
            Mathf.FloorToInt(enemyCtrl.target.transform.position.x),
            Mathf.FloorToInt(enemyCtrl.target.transform.position.y),
            Mathf.FloorToInt(enemyCtrl.target.transform.position.z)
        );

        Vector3Int forwardGridPosition = new Vector3Int(
            Mathf.FloorToInt(enemyCtrl.transform.position.x + enemyCtrl.transform.forward.x),
            Mathf.FloorToInt(enemyCtrl.transform.position.y + enemyCtrl.transform.forward.y),
            Mathf.FloorToInt(enemyCtrl.transform.position.z + enemyCtrl.transform.forward.z)
        );

        return (targetGridPosition - currentGridPosition) == (forwardGridPosition - currentGridPosition);
    }
}
