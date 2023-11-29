using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    public void UpdateAttackPositions()
    {
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

        if(playerCtrl.attakableTilePositions.Count > 0)
        {
            playerCtrl.attakableTilePositions.Clear();
        }

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition;
                    var tilePosInt = new Vector3(tilePosition.x,tilePosition.y,tilePosition.z);

                    playerCtrl.attakableTilePositions.Add(tilePosInt);
                }
            }
        }
        var playerPosInt = new Vector3(characterPosition.x, characterPosition.y, characterPosition.z);
        playerCtrl.attakableTilePositions.Add(playerPosInt);
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
