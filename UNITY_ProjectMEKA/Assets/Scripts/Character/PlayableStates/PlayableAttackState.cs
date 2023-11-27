using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableAttackState : PlayableBaseState
{
    private float timer;
    private GameObject[] enemys;
    public PlayableAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
        timer = 0;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > playerCtrl.state.attackDelay)
        {
            timer = 0;
            //playerCtrl.Hit();//test
            playerCtrl.ani.SetTrigger("Attack");
        }
        if(/*!playerCtrl.target.activeSelf||*/playerCtrl.target == null)//현재 때리고 있는 적이 죽으면 상태 변경
        {
            Debug.Log("상태 변경 Idle");
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
        if(IsInAttackRange(playerCtrl.target.GetComponent<EnemyController>().CurrentGridPos))
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }

    }
    bool IsInAttackRange(Vector3Int targetPosition)
    {
        
        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right;
        int characterRow = 0;
        int characterCol = 0;

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 gizmoPosition = characterPosition + relativePosition;
                    Vector3Int Pos = new Vector3Int(Mathf.FloorToInt(gizmoPosition.x), Mathf.FloorToInt(gizmoPosition.y), Mathf.FloorToInt(gizmoPosition.z));

                    if (targetPosition == Pos)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    
}
