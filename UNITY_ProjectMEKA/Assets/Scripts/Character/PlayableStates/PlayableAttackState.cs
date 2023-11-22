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
            playerCtrl.Hit();//test
        }
        if(playerCtrl.target == null)//현재 때리고 있는 적이 죽으면 상태 변경
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }
}
