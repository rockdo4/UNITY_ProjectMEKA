using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NPCDestinationStates : NPCBaseState
{

    private Vector3 targetPos;
    private float threshold = 0.1f;

    GameObject[] players;
    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemyCtrl.transform.position = enemyCtrl.initPos;
        targetPos = wayPoint[enemyCtrl.waypointIndex].position;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        switch(enemyCtrl.moveType)
        {
            case Defines.MoveType.AutoTile:
                break;
            case Defines.MoveType.Waypoint:
            case Defines.MoveType.Straight:
                MoveEnemyWaypoint();
                break;
            case Defines.MoveType.WaypointRepeat:
                MoveEnemyRepeat(enemyCtrl.moveRepeatCount);
                break;
        }

        CheckEnemy();
    }
    void CheckEnemy()
    {
        Vector3 centerPos = new Vector3(enemyCtrl.CurrentGridPos.x + 0.5f, enemyCtrl.CurrentGridPos.y, enemyCtrl.CurrentGridPos.z + 0.5f);
        centerPos.y += enemyCtrl.CurrentPos.y;
        
        if(Vector3.Distance(enemyCtrl.transform.position, centerPos) > 0.1f)
        {
            return;
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        Vector3Int playerGridPos = enemyCtrl.CurrentGridPos;

        // �����Ÿ� ���� Ÿ���� Ȯ��
        int tileRange = Mathf.FloorToInt(enemyCtrl.state.range); // Ÿ�� �����Ÿ�
        for (int i = 1; i <= tileRange; i++)
        {
            Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(enemyCtrl.transform.forward) * i;
            Vector3 tileCenterWorldPos = new Vector3(forwardGridPos.x + 0.5f, forwardGridPos.y + 0.5f, forwardGridPos.z + 0.5f);
            foreach (GameObject pl in players)
            {
                PlayerController player = pl.GetComponent<PlayerController>();

                if (player != null)
                {
                    Vector3Int enemyGridPos = player.CurrentGridPos;
                    //Vector3 playerPos = player.transform.position;
                    float distanceToTileCenter = Vector3.Distance(enemyGridPos, tileCenterWorldPos);

                    if (enemyGridPos == forwardGridPos/*distanceToTileCenter <= 0.1f*/)
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

    public override void Update()
    {

    }

    public void MoveEnemyWaypoint()
    {
        targetPos.y = enemyCtrl.transform.position.y;
        enemyCtrl.transform.LookAt(targetPos);
        var speed = enemyCtrl.gameObject.GetComponent<CharacterState>().speed;
        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold)
        {
            enemyCtrl.waypointIndex++;

            if (enemyCtrl.waypointIndex >= wayPoint.Length)
            {
                enemyCtrl.waypointIndex = 0;
                enemyCtrl.transform.position = enemyCtrl.initPos;
                enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
                return;
            }
            targetPos = wayPoint[enemyCtrl.waypointIndex].position;
        }
    }

    public void MoveEnemyRepeat(int count)
    {
        // count��ŭ �ݺ�
        // ù��°�� ���� �� �Ͽ콺�� ���� �̵�


    }
}
