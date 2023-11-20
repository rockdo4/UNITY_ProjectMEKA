using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableIdleState : PlayableBaseState
{
    public PlayableIdleState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(playerCtrl.transform.position, playerCtrl.state.range);

        foreach (Collider co in colliders)
        {
            if (co.CompareTag("Enemy"))
            {
                playerCtrl.target = co.gameObject;
                playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                break;
            }
        }
    }
}
