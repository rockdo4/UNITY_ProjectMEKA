using System.Collections.Generic;
using UnityEngine;

public class PlayableIdleState : PlayableBaseState
{

    //Dictionary<float, GameObject> players;
    List<KeyValuePair<float, GameObject>> players;
    GameObject[] enemys;
    GameObject[] playerses;
    PlayerController character;

    public PlayableIdleState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        playerCtrl.CurrentGridPos = new Vector3Int(Mathf.FloorToInt(playerCtrl.transform.position.x), Mathf.FloorToInt(playerCtrl.transform.position.y), Mathf.FloorToInt(playerCtrl.transform.position.z));
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (playerCtrl.state.Hp <= 0)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Die);

        }
        else 
        {
            players = new List<KeyValuePair<float, GameObject>>();
            SetPlayers();
            switch (playerCtrl.state.occupation)
            {
                case Defines.Occupation.Supporters:
                    CheckHealing();
                    break;
                default:
                    CheckEnemy();
                    break;

            }
        }

    }
    void CheckEnemy()
    {
        
        foreach (var en in playerCtrl.rangeInEnemys)
        {
            
            EnemyController enemy = en.GetComponentInParent<EnemyController>();
            if (enemy != null)
            {
                playerCtrl.target = en;
                enemy.HoIsHitMe = playerCtrl.gameObject;
                playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                return;

            }
        }
    }

    void SetPlayers()
    {
        playerses = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject co in playerses)
        {
           
            PlayerController pc = co.GetComponent<PlayerController>();
            float healthRatio = pc.state.Hp / pc.state.maxHp;
            players.Add(new KeyValuePair<float, GameObject>(healthRatio, co.gameObject));
            
        }



    }
    void CheckHealing()
    {
        foreach (var pl in playerCtrl.rangeInPlayers)
        {
            PlayerController player = pl.GetComponentInParent<PlayerController>();
            if(player == null)
            {
                return;
            }
            float currentHp = player.state.Hp / player.state.maxHp;
            if (character == null || (character.state.Hp / character.state.maxHp) > currentHp)
            {
                character = player;
            }
        }

        if (character != null && (character.state.Hp / character.state.maxHp) < 1f && character.gameObject.activeSelf)
        {
            playerCtrl.target = character.gameObject;
            Debug.Log("Healing");
            playerCtrl.SetState(PlayerController.CharacterStates.Healing);
        }
        else
        {
            character = null;
        }

    }
}
