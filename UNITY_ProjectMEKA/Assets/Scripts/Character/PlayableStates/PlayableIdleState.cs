using System.Collections.Generic;
using Unity.VisualScripting;
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
                if(playerCtrl.state.occupation == Defines.Occupation.Castor ||
                    playerCtrl.state.occupation == Defines.Occupation.Hunter)
                {
                    playerCtrl.target = en;
                    enemy.HoIsHitMe = playerCtrl.gameObject;

                    
                    // �ڽ� ������Ʈ�� �θ� ���� �������� �Ͻ������� �и�
                    Transform obj = playerCtrl.GetComponentInChildren<CreateCollider>().gameObject.transform;
                    obj.parent = null;
                    // �θ� ������Ʈ ȸ��
                    Vector3 lookTarget = new Vector3(playerCtrl.target.transform.position.x, playerCtrl.transform.position.y, playerCtrl.target.transform.position.z);
                    playerCtrl.gameObject.transform.LookAt(lookTarget);
                    // �ڽ� ������Ʈ�� �ٽ� �θ� ���� ������ ����
                    obj.parent = playerCtrl.transform;


                    if(playerCtrl.rangeInEnemys.Contains(playerCtrl.target))
                    {
                        playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                        return;
                    }
                    
                }
                else
                {
                    if(!enemy.state.isFly)
                    {
                        playerCtrl.target = en;
                        enemy.HoIsHitMe = playerCtrl.gameObject;

                        //Vector3 lookTarget = new Vector3(playerCtrl.target.transform.position.x, playerCtrl.transform.position.y, playerCtrl.target.transform.position.z);
                        //playerCtrl.gameObject.transform.LookAt(lookTarget);
                        //playerCtrl.GetComponentInChildren<CreateCollider>().gameObject.transform.position = playerCtrl.ChildPos.position;
                        //playerCtrl.GetComponentInChildren<CreateCollider>().gameObject.transform.rotation = playerCtrl.ChildPos.rotation;
                        Transform obj = playerCtrl.GetComponentInChildren<CreateCollider>().gameObject.transform;
                        obj.parent = null;
                        // �θ� ������Ʈ ȸ��
                        Vector3 lookTarget = new Vector3(playerCtrl.target.transform.position.x, playerCtrl.transform.position.y, playerCtrl.target.transform.position.z);
                        playerCtrl.gameObject.transform.LookAt(lookTarget);
                        // �ڽ� ������Ʈ�� �ٽ� �θ� ���� ������ ����
                        obj.parent = playerCtrl.transform;

                        if (playerCtrl.rangeInEnemys.Contains(playerCtrl.target))
                        {
                            playerCtrl.SetState(PlayerController.CharacterStates.Attack);
                            return;
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
