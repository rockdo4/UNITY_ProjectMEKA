using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayableProjectileAttackState : PlayableBaseState
{
    private float timer;
    GameObject[] enemys;
    private int count;
    bool isTarget = false;
    public PlayableProjectileAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        isTarget = false;
    }

    public override void Exit()
    {
        //timer = 0;

        if(playerCtrl.ani.GetCurrentAnimatorStateInfo(0).loop)
        {
            playerCtrl.ani.SetTrigger("Idle");
        }
        playerCtrl.target = null;
    }

    public override void Update()
    {
        if (!playerCtrl.rangeInEnemys.Contains(playerCtrl.target))
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }

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
                
                if(playerCtrl.target == null)
                {
                    playerCtrl.SetState(PlayerController.CharacterStates.Idle);

                }
                if (playerCtrl.target.activeInHierarchy || playerCtrl.rangeInEnemys.Contains(playerCtrl.target))//59
                {
                    playerCtrl.ani.SetTrigger("Attack");
                    return;
                }
            }
            
        }

        
    }
   
}
