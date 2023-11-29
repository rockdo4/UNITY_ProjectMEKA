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
        //�±׷� �� ���� ã�Ƽ� �����
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        //ĳ���� ��ġ, ����, right?
        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right; 
        int characterRow = 0; 
        int characterCol = 0; 

        //attackrange ��Ÿ� ������ �迭 �޾Ƽ�
        //ĳ���� ��ǥ ã�� ĳ���� == 2
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


        //������ �迭 ��ȸ�ϸ鼭 �ش� �ε����� 1�� ��쿡�� ����� �׷��� 
        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    //i, j 2���� �迭 ��ȸ�ϸ鼭 i�� ĳ���� ���� j�� ĳ���� ������ ��������
                    // 1 1 0
                    // 1 1 0
                    // 2 1 0,  2�� ĳ����, 1�� ���ݹ���
                   
                    //1 - 2, 1 - 2 // -1, -1
                        

                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 gizmoPosition = characterPosition + relativePosition;

                    //����� ���⼭ �׸�
                    Vector3Int Pos = new Vector3Int(Mathf.FloorToInt(gizmoPosition.x), Mathf.FloorToInt(gizmoPosition.y), Mathf.FloorToInt(gizmoPosition.z));

                    //ã�� �� ��� ��ȸ
                    foreach (GameObject en in enemys)
                    {
                        EnemyController enemy = en.GetComponent<EnemyController>();
                        if (enemy != null)
                        {
                            //���� ��ġ�� �ε��� �޾ƿ�
                            Vector3Int enemyGridPos = enemy.CurrentGridPos;

                            //����� �׷����� �ε����� �� �ε��� ��ġ�� ������
                            if (enemyGridPos == Pos)
                            {
                                //������
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

    // 11.29, �����, Ÿ�� ������ ���س��� �ؼ� �߰�(�� �Լ� �Ϻ� ������)
    public void UpdateAttackPositions()
    {
        //ĳ���� ��ġ, ����, right?
        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right;
        int characterRow = 0;
        int characterCol = 0;

        //attackrange ��Ÿ� ������ �迭 �޾Ƽ�
        //ĳ���� ��ǥ ã�� ĳ���� == 2
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

        //������ �迭 ��ȸ�ϸ鼭 �ش� �ε����� 1�� ��쿡�� ����� �׷��� 
        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    //i, j 2���� �迭 ��ȸ�ϸ鼭 i�� ĳ���� ���� j�� ĳ���� ������ ��������
                    // 1 1 0
                    // 1 1 0
                    // 2 1 0,  2�� ĳ����, 1�� ���ݹ���

                    //1 - 2, 1 - 2 // -1, -1


                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition;

                    //����� ���⼭ �׸�
                    var pos = new Vector3Int(Mathf.FloorToInt(tilePosition.x), Mathf.FloorToInt(tilePosition.y), Mathf.FloorToInt(tilePosition.z));
                    playerCtrl.attakableTilePositions.Add(pos);

                }
            }
        }
        Debug.Log($"����Ÿ�� ���� : {playerCtrl.attakableTilePositions.Count}");
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
