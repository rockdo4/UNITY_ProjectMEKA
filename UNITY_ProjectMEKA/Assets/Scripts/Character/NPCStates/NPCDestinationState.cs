using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDestinationStates : NPCBaseState
{
    private float timer;
    private float timerange;
    GameObject[] players;
    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        timerange = 1;
        //agent.isStopped = false;
    }

    public override void Exit()
    {
        //agent.isStopped = true;
    }

    public override void Update()
    {
        //timer += Time.deltaTime;
        //if (timer > timerange)
        //{
        //    timer = 0;
        //    //agent.SetDestination(wayPoint[0].position);
        //    //if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        //    //{
        //    //    enemyCtrl.SetState(EnemyController.NPCStates.Idle);
        //    //}
        //}

        //Collider[] colliders = Physics.OverlapSphere(enemyCtrl.transform.position, enemyCtrl.state.range);

        //foreach (Collider co in colliders)
        //{
        //    if (co.CompareTag("Player"))
        //    {
        //        enemyCtrl.target = co.gameObject;
        //        PlayerController pl = co.GetComponent<PlayerController>();  
        //        if(pl.blockCount < pl.maxBlockCount)
        //        {
        //            enemyCtrl.SetState(EnemyController.NPCStates.Attack);
        //        }

        //        break;
        //    }
        //}
        CheckEnemy();
    }
    void CheckEnemy()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        Vector3Int playerGridPos = enemyCtrl.CurrentGridPos;

        // 사정거리 내의 타일을 확인
        int tileRange = Mathf.FloorToInt(enemyCtrl.state.range); // 타일 사정거리
        for (int i = 1; i <= tileRange; i++)
        {
            Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(enemyCtrl.transform.forward) * i;

            foreach (GameObject pl in players)
            {
                PlayerController player = pl.GetComponent<PlayerController>();
                if (player != null)
                {
                    Vector3Int enemyGridPos = player.CurrentGridPos;

                    if (enemyGridPos == forwardGridPos)
                    {
                        // 적 발견: 여기에 적을 처리하는 로직을 추가
                        enemyCtrl.target = pl;
                        if (player.blockCount < player.maxBlockCount)
                        {
                            enemyCtrl.SetState(EnemyController.NPCStates.Attack);
                        }
                            
                        Debug.Log("attack");
                        return; // 적을 발견했으므로 함수 종료
                    }
                }
            }
        }
        //players = GameObject.FindGameObjectsWithTag("Player");

        //Vector3Int playerGridPos = enemyCtrl.CurrentGridPos;
        //Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(enemyCtrl.transform.forward);

        //foreach (GameObject pl in players)
        //{
        //    PlayerController player = pl.GetComponent<PlayerController>();
        //    if (player != null)
        //    {
        //        // 플레이어로부터 적까지의 거리 계산
        //        float distanceToEnemy = Vector3.Distance(enemyCtrl.transform.position, player.transform.position);

        //        // 사정거리 내에 적이 있는지 확인
        //        if (distanceToEnemy <= enemyCtrl.state.range)
        //        {
        //            // 플레이어의 전방 방향과 적 사이의 방향 벡터 계산
        //            Vector3 directionToEnemy = (player.transform.position - enemyCtrl.transform.position).normalized;
        //            float dotProduct = Vector3.Dot(enemyCtrl.transform.forward, directionToEnemy);

        //            if (dotProduct > 0)
        //            {
        //                // 적 발견: 여기에 적을 처리하는 로직을 추가
        //                enemyCtrl.target = pl;
        //                enemyCtrl.SetState(EnemyController.NPCStates.Attack);
        //                Debug.Log("attack");
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
