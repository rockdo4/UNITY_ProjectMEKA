using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableAttackState : PlayableBaseState
{
    private float timer;
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
        if(/*!playerCtrl.target.activeSelf||*/playerCtrl.target == null)//���� ������ �ִ� ���� ������ ���� ����
        {
            Debug.Log("���� ���� Idle");
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }
}
