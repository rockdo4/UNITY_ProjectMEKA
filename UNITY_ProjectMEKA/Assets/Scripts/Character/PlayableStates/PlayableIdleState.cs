using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Linq;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayableIdleState : PlayableBaseState
{

    //Dictionary<float, GameObject> players;
    List<KeyValuePair<float, GameObject>> players;
    GameObject[] enemys;
    GameObject[] playerses;

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
    void CheckEnemy()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        Vector3Int playerGridPos = playerCtrl.CurrentGridPos;

        int tileRange = Mathf.FloorToInt(playerCtrl.state.range); // 타일 사정거리
        for (int i = 1; i <= tileRange; i++)
        {
            Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(playerCtrl.transform.forward) * i;

            foreach (GameObject en in enemys)
            {
                EnemyController enemy = en.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    Vector3Int enemyGridPos = enemy.CurrentGridPos;

                    if (enemyGridPos == forwardGridPos)
                    {
                        playerCtrl.target = en;
                        playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                        Debug.Log("attack");
                        return; 
                    }
                }
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
        Vector3Int playerGridPos = playerCtrl.CurrentGridPos;
        float minHealthRatio = float.MaxValue;
        GameObject targetPlayer = null;

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector3Int checkPos = new Vector3Int(playerGridPos.x + x, playerGridPos.y, playerGridPos.z + z);

                foreach (var player in players)
                {
                    PlayerController pc = player.Value.GetComponent<PlayerController>();
                    if (pc != null && pc.CurrentGridPos == checkPos)
                    {
                        float healthRatio = pc.state.Hp / pc.state.maxHp;
                        if (healthRatio < minHealthRatio)
                        {
                            minHealthRatio = healthRatio;
                            targetPlayer = player.Value;
                        }
                    }
                }
            }
        }

        if (targetPlayer != null && minHealthRatio < 1f)
        {
            playerCtrl.target = targetPlayer;
            playerCtrl.SetState(PlayerController.CharacterStates.Healing);
            Debug.Log("Healing");
        }
    }
}
