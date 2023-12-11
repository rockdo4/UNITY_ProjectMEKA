using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayableProjectileAttackState : PlayableBaseState
{
    private float timer;
    GameObject[] enemys;
    private int count;
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
        if (playerCtrl.state.Hp <= 0)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Die);

        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = playerCtrl.state.attackDelay;
                if (!playerCtrl.target.activeInHierarchy)
                {
                    playerCtrl.SetState(PlayerController.CharacterStates.Idle);
                    return;
                }
                playerCtrl.ani.SetTrigger("Attack");
                playerCtrl.SetState(PlayerController.CharacterStates.Idle);
            }
            if (playerCtrl.target.activeInHierarchy)
            {
                playerCtrl.SetState(PlayerController.CharacterStates.Idle);
            }
        }
        
    }
    
}
