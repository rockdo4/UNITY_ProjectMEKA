using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Linq;

public class PlayableIdleState : PlayableBaseState
{

    //Dictionary<float, GameObject> players;
    List<KeyValuePair<float, GameObject>> players;
    public PlayableIdleState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        players = new List<KeyValuePair<float, GameObject>>();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(playerCtrl.transform.position, playerCtrl.state.range);
        if (playerCtrl.state.occupation == Defines.Occupation.Supporters)
        {
            foreach (Collider co in colliders)
            {
                if (co.CompareTag("Player"))
                {
                    PlayerController pc = co.gameObject.GetComponent<PlayerController>();
                    float healthRatio = pc.state.Hp / pc.state.maxHp;
                    players.Add(new KeyValuePair<float, GameObject>(healthRatio, co.gameObject));
                }
            }

            if (players.Count == 0)
            {
                return;
            }

            players.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));
            if(players[0].Key >= 1f)
            {
                return;
            }
            GameObject go = players[0].Value;

            if (go == null)
            {
                return;
            }

            playerCtrl.target = go;
            playerCtrl.SetState(PlayerController.CharacterStates.Healing);

        }
        else
        {
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
    
}
