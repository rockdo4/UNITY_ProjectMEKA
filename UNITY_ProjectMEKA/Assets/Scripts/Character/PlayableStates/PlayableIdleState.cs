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
        //태그로 적 전부 찾아서 갖고옴
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        //캐릭터 위치, 전방, right?
        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right; 
        int characterRow = 0; 
        int characterCol = 0; 

        //attackrange 사거리 이차원 배열 받아서
        //캐릭터 좌표 찾음 캐릭터 == 2
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


        //이차원 배열 순회하면서 해당 인덱스가 1인 경우에만 기즈모 그려줌 
        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    //i, j 2차원 배열 순회하면서 i는 캐릭터 정면 j는 캐릭터 오른쪽 기준으로
                    // 1 1 0
                    // 1 1 0
                    // 2 1 0,  2는 캐릭터, 1은 공격범위
                   
                    //1 - 2, 1 - 2 // -1, -1
                        

                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 gizmoPosition = characterPosition + relativePosition;

                    //기즈모 여기서 그림
                    Vector3Int Pos = new Vector3Int(Mathf.FloorToInt(gizmoPosition.x), Mathf.FloorToInt(gizmoPosition.y), Mathf.FloorToInt(gizmoPosition.z));

                    //찾은 적 모두 순회
                    foreach (GameObject en in enemys)
                    {
                        EnemyController enemy = en.GetComponent<EnemyController>();
                        if (enemy != null)
                        {
                            //적이 위치한 인덱스 받아옴
                            Vector3Int enemyGridPos = enemy.CurrentGridPos;

                            //기즈모 그려지는 인덱스랑 적 인덱스 위치가 같으면
                            if (enemyGridPos == Pos)
                            {
                                //공격함
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

    // 11.29, 김민지, 타일 포지션 구해놔야 해서 추가(위 함수 일부 복사함)
    public void UpdateAttackPositions()
    {
        //캐릭터 위치, 전방, right?
        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right;
        int characterRow = 0;
        int characterCol = 0;

        //attackrange 사거리 이차원 배열 받아서
        //캐릭터 좌표 찾음 캐릭터 == 2
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

        //이차원 배열 순회하면서 해당 인덱스가 1인 경우에만 기즈모 그려줌 
        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    //i, j 2차원 배열 순회하면서 i는 캐릭터 정면 j는 캐릭터 오른쪽 기준으로
                    // 1 1 0
                    // 1 1 0
                    // 2 1 0,  2는 캐릭터, 1은 공격범위

                    //1 - 2, 1 - 2 // -1, -1


                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition;

                    //기즈모 여기서 그림
                    var pos = new Vector3Int(Mathf.FloorToInt(tilePosition.x), Mathf.FloorToInt(tilePosition.y), Mathf.FloorToInt(tilePosition.z));
                    playerCtrl.attakableTilePositions.Add(pos);

                }
            }
        }
        Debug.Log($"공격타일 개수 : {playerCtrl.attakableTilePositions.Count}");
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
