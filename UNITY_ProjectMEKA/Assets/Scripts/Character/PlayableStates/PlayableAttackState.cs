using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayableAttackState : PlayableBaseState
{
    private float timer;
    public PlayableAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        if(playerCtrl.trail != null)
        {
            playerCtrl.trail.gameObject.SetActive(true);
        }
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
                if(playerCtrl.target == null ||!playerCtrl.target.activeInHierarchy)
                {
                    playerCtrl.target = null;//문제생기면 제거
                    playerCtrl.SetState(PlayerController.CharacterStates.Idle);
                    return;
                }

                
                playerCtrl.ani.SetTrigger("Attack");
                //playerCtrl.SetState(PlayerController.CharacterStates.Idle);
            }
        }

    }
    
}
