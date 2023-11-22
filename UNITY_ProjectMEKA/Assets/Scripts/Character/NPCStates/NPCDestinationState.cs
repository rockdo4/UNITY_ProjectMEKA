using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDestinationStates : NPCBaseState
{
    private float timer;
    private float timerange;

    // 11.22, ±ËπŒ¡ˆ, ¿ÃµøΩ√ « ø‰
    private Vector3 targetPos;
    private float threshold = 0.1f;
    private int waypointCount = 0;

    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
        targetPos = wayPoint[0].position;
    }

    public override void Enter()
    {
        // 11.22, ±ËπŒ¡ˆ, ¿Ãµø ±‚¥… ∫Ø∞Ê
        //timerange = 1;
        //agent.isStopped = false;
    }

    public override void Exit()
    {
        // 11.22, ±ËπŒ¡ˆ, ¿Ãµø ±‚¥… ∫Ø∞Ê
        //agent.isStopped = true;
    }

    public override void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > timerange)
        {
            timer = 0;

            // 11.22, ±ËπŒ¡ˆ, ¿Ãµø ±‚¥… ∫Ø∞Ê
            //agent.SetDestination(wayPoint[0].position);
            //if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            //{
            //    enemyCtrl.SetState(EnemyController.NPCStates.Idle);
            //}
            MoveEnemy();

        }

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
        enemyCtrl.transform.LookAt(targetPos);
        var speed = enemyCtrl.gameObject.GetComponent<CharacterState>().speed;
        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold)
        {
            waypointCount++;
            if (waypointCount >= wayPoint.Length)
                return;

            targetPos = wayPoint[waypointCount].position;
        }

        //if ()
        //{
        //    enemyCtrl.SetState(EnemyController.NPCStates.Idle);
        //    Debug.Log("idle state");
        //}

    }
}
