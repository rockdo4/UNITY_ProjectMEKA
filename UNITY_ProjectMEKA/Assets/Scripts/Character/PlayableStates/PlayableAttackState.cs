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
        if(playerCtrl.state.Hp <=0)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Die);

        }
        else
        {
            timer += Time.deltaTime;
            if (timer > playerCtrl.state.attackDelay)
            {
                timer = 0;
                if(!playerCtrl.target.activeInHierarchy)
                {
                    playerCtrl.SetState(PlayerController.CharacterStates.Idle);
                    return;
                }

                playerCtrl.ani.SetTrigger("Attack");
                playerCtrl.SetState(PlayerController.CharacterStates.Idle);
            }
        }

    }
    
}
