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

        // �����Ÿ� ���� Ÿ���� Ȯ��
        int tileRange = Mathf.FloorToInt(enemyCtrl.state.range); // Ÿ�� �����Ÿ�
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
                        // �� �߰�: ���⿡ ���� ó���ϴ� ������ �߰�
                        enemyCtrl.target = pl;
                        if (player.blockCount < player.maxBlockCount)
                        {
                            enemyCtrl.SetState(EnemyController.NPCStates.Attack);
                        }
                            
                        Debug.Log("attack");
                        return; // ���� �߰������Ƿ� �Լ� ����
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
        //        // �÷��̾�κ��� �������� �Ÿ� ���
        //        float distanceToEnemy = Vector3.Distance(enemyCtrl.transform.position, player.transform.position);

        //        // �����Ÿ� ���� ���� �ִ��� Ȯ��
        //        if (distanceToEnemy <= enemyCtrl.state.range)
        //        {
        //            // �÷��̾��� ���� ����� �� ������ ���� ���� ���
        //            Vector3 directionToEnemy = (player.transform.position - enemyCtrl.transform.position).normalized;
        //            float dotProduct = Vector3.Dot(enemyCtrl.transform.forward, directionToEnemy);

        //            if (dotProduct > 0)
        //            {
        //                // �� �߰�: ���⿡ ���� ó���ϴ� ������ �߰�
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
