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
                        enemyCtrl.target = pl;
                        if (player.blockCount < player.maxBlockCount)
                        {
                            enemyCtrl.SetState(EnemyController.NPCStates.Attack);
                        }
                        return; 
                    }
                }
            }
        }
        
    }
}
