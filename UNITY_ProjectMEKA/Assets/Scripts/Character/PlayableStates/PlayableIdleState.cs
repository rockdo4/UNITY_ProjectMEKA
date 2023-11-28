using System.Collections.Generic;
using UnityEngine;

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

        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right; 
        int characterRow = 0; 
        int characterCol = 0; 

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 gizmoPosition = characterPosition + relativePosition;
                    Vector3Int Pos = new Vector3Int(Mathf.FloorToInt(gizmoPosition.x), Mathf.FloorToInt(gizmoPosition.y), Mathf.FloorToInt(gizmoPosition.z));

                    foreach (GameObject en in enemys)
                    {
                        EnemyController enemy = en.GetComponent<EnemyController>();
                        if (enemy != null)
                        {
                            Vector3Int enemyGridPos = enemy.CurrentGridPos;

                            if (enemyGridPos == Pos)
                            {
                                //Debug.Log("AttackEnemy");
                                playerCtrl.target = en;
                                enemy.HoIsHitMe = playerCtrl.gameObject;
                                playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                                return;
                            }
                        }
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
        }
    }
}
