using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NPCDestinationStates : NPCBaseState
{
    // 11.22, 김민지, 필요 없을 것 같아서 삭제
    //private float timer;
    //private float timerange;

    // 11.22, 김민지, 이동시 필요
    private Vector3 targetPos;
    private float threshold = 0.1f;

    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        // 11.22, 김민지, 이동 기능 변경
        targetPos = wayPoint[enemyCtrl.waypointCount].position;
    }

    public override void Exit()
    {
        // 11.22, 김민지, 이동 기능 변경
        //agent.isStopped = true;
    }

    public override void FixedUpdate()
    {
        MoveEnemy();

        Collider[] colliders = Physics.OverlapSphere(enemyCtrl.transform.position, enemyCtrl.state.range);

        foreach (Collider co in colliders)
        {
            if (co.CompareTag("Player"))
            {
                enemyCtrl.target = co.gameObject;
                PlayerController pl = co.GetComponent<PlayerController>();
                if (pl.blockCount < pl.maxBlockCount)
                {
                    enemyCtrl.SetState(EnemyController.NPCStates.Attack);
                }

                break;
            }
        }
    }

    public override void Update()
    {

    }

    public void MoveEnemy()
    {
        targetPos.y = enemyCtrl.transform.position.y;
        enemyCtrl.transform.LookAt(targetPos);
        var speed = enemyCtrl.gameObject.GetComponent<CharacterState>().speed;
        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold)
        {
            enemyCtrl.waypointCount++;

            if (enemyCtrl.waypointCount >= wayPoint.Length)
            {
                enemyCtrl.waypointCount = 0;
                enemyCtrl.transform.position = enemyCtrl.initPos;
                enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
                return;
            }
            targetPos = wayPoint[enemyCtrl.waypointCount].position;
        }
    }
}
