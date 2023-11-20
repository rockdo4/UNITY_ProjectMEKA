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
        Debug.Log("playerAttack");
        timer = 0;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        //���� �����ȿ� ������ �ٽ� idle���·�
        timer += Time.deltaTime;
        if (timer > playerCtrl.state.attackDelay)
        {
            timer = 0;
            playerCtrl.Hit();//test
        }
        if(playerCtrl.target == null)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }
}
