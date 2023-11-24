using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayableProjectileAttackState : PlayableBaseState
{
    private float timer;
    GameObject[] enemys;

    public PlayableProjectileAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        //timer = 0;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = playerCtrl.state.attackDelay;
            //timer = 0;
            playerCtrl.ani.SetTrigger("Attack");
            //CheckEnemy();
            playerCtrl.Fire();
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
        if (playerCtrl.target == null)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }

    void CheckEnemy()
    {
        //타겟의 위치가 자신의 공격 사거리 안에 없다면 타겟을 널로 바꾸고 idle상태로 변경한다
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        Vector3Int playerGridPos = playerCtrl.CurrentGridPos;

        int tileRange = Mathf.FloorToInt(playerCtrl.state.range); // 타일 사정거리
        for (int i = 1; i <= tileRange; i++)
        {
            Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(playerCtrl.transform.forward) * i;

            if(playerCtrl.target.GetComponent<EnemyController>().CurrentGridPos != forwardGridPos) 
            {
                playerCtrl.target = null;
                playerCtrl.SetState(PlayerController.CharacterStates.Idle);

            }




            //foreach (GameObject en in enemys)
            //{
            //    EnemyController enemy = en.GetComponent<EnemyController>();
            //    if (enemy != null)
            //    {
            //        Vector3Int enemyGridPos = enemy.CurrentGridPos;

            //        if (enemyGridPos == forwardGridPos)
            //        {
            //            playerCtrl.target = en;
            //            playerCtrl.SetState(PlayerController.CharacterStates.Attack);
            //            return;
            //        }
            //    }
            //}
        }

    }
}
