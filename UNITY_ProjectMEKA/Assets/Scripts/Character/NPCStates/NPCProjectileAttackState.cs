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
            if(enemyCtrl.state.isMovingShot)
            {
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }
            if (enemyCtrl.state.isFly)
            {
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }
            if (!CheckDistance())
            {
                enemyCtrl.SetState(NPCStates.Move);
                enemyCtrl.ani.SetTrigger("Run");
            }
            
        }
        if (enemyCtrl.target.GetComponentInParent<PlayerController>() == null)
        {
            Debug.Log("target null");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }
        if (!enemyCtrl.target.activeSelf)
        {
            Debug.Log("Target not avtive");
            enemyCtrl.SetState(NPCStates.Move);
            enemyCtrl.ani.SetTrigger("Run");
        }

        foreach (var a in enemyCtrl.rangeInPlayers)
        {
            if (a.GetComponentInParent<PlayerController>() == null)
            {
                if (enemyCtrl.rangeInPlayers.Contains(a))
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
