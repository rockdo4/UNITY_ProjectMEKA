using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PoolAble
{
    private PlayableStateManager stateManager = new PlayableStateManager();
    private List<PlayableBaseState> states = new List<PlayableBaseState>();
    public GameObject FirePosition;
    [HideInInspector]
    public Animator ani;
    
    private Vector3 CurrentPos;

    [HideInInspector]
    public Vector3Int CurrentGridPos;
    [HideInInspector]
    public CharacterState state;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public int blockCount;
    [HideInInspector]
    public int maxBlockCount;

    public enum CharacterStates
    {
        Idle,
        Die,
        Attack,
        Healing,
    }
    private void Awake()
    {
        state = GetComponent<CharacterState>();
        SetBlockCount();
        ani = GetComponent<Animator>();
    }
    private void OnEnable()//인게임에서 배치할때 오브젝트풀링을 고려함
    {
        CurrentPos = transform.position;
        CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
        
    }
    void Start()
    {
        state.ConvertTo2DArray();
        states.Add(new PlayableIdleState(this));
        states.Add(new PlayableDieState(this));
        if(state.occupation == Defines.Occupation.Hunter || state.occupation == Defines.Occupation.Castor)
        {
            states.Add(new PlayableProjectileAttackState(this));
            //state.ConvertTo2DArray();

            //for(int i = 0; i < state.AttackRange.GetLength(0); ++i)
            //{
            //    for(int j = 0; j < state.AttackRange.GetLength(1); ++j)
            //    {
            //        Debug.Log(state.AttackRange[i, j] + "\n");
            //    }
            //}

           
            //foreach(float state in states.AttackRange) { }
            //state.AttackRange
            //= new int[5,5]{
            //    { 0,0,1,0,0},
            //    { 0,1,1,1,0},
            //    { 0,1,1,1,0},
            //    { 0,1,1,1,0},
            //    { 0,1,2,1,0},
            //};
        }
        else
        {
            states.Add(new PlayableAttackState(this));
        }
        states.Add(new PlayableHealingState(this));

        SetState(CharacterStates.Idle);

        
    }
    private void Update()
    {
        stateManager.Update();
        Collider[] colliders = Physics.OverlapSphere(transform.position, state.range);
        Collider[] enemyColliders = Array.FindAll(colliders, co => co.CompareTag("Enemy"));

        blockCount = enemyColliders.Length;
    }

    public void SetState(CharacterStates state)
    {
        stateManager.ChangeState(states[(int)state]);
    }

    public void Hit()
    {
        if (target == null)
        {
            return;
        }
        IAttackable take = target.GetComponent<IAttackable>();

        take.OnAttack(state.damage + Rockpaperscissors());
        
    }
    public float Rockpaperscissors()//상성
    {
        float compatibility = state.damage * 0.1f;

        EnemyController enemy = target.GetComponent<EnemyController>();

        if (state.property == enemy.state.property)
        {
            return 0;
        }
     
        switch (state.property)
        {
            case Defines.Property.Prime:
                return (enemy.state.property == Defines.Property.Edila) ? compatibility :  -compatibility;
            case Defines.Property.Edila:
                return (enemy.state.property == Defines.Property.Grieve) ? compatibility : -compatibility;
            case Defines.Property.Grieve:
                return (enemy.state.property == Defines.Property.Prime) ? compatibility : -compatibility;
            default:
                return 0;
        }
    }
   
    public void SetBlockCount()
    {
        switch (state.occupation)
        {
            case Defines.Occupation.Guardian:
                maxBlockCount = 4;
                break;
            case Defines.Occupation.Striker:
                maxBlockCount = 2;
                break;
            case Defines.Occupation.Castor:
                maxBlockCount = 0;
                break;
            case Defines.Occupation.Hunter:
                maxBlockCount = 0;
                break;
            case Defines.Occupation.Supporters:
                break;
            default:
                break;
        }

    }
    
    public void Fire()
    {
        if(target == null) return;
        //var obj = ObjectPoolManager.instance.GetGo("bullet");
        var obj = ObjectPoolManager.instance.GetGo(state.BulletName);
        //obj.transform.position = transform.position; // 발사 위치 설정
        //obj.transform.position = FirePosition.transform.position; // 발사 위치 설정
        ////obj.transform.rotation = transform.rotation; // 회전 초기화
        //obj.transform.rotation = Quaternion.identity; // 회전 초기화
        

        obj.transform.LookAt(target.transform);
        
        switch (state.BulletType)
        {
            case CharacterState.Type.Bullet:
                var projectile = obj.GetComponent<Bullet>();
                projectile.ResetState();
                obj.transform.position = FirePosition.transform.position;
                obj.transform.rotation = FirePosition.transform.rotation;
                projectile.damage = state.damage;
                projectile.target = target.transform;
                projectile.Player = gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
            case CharacterState.Type.Aoe:
                var projectileA = obj.GetComponent<AOE>();
                projectileA.ResetState();
                obj.transform.position = FirePosition.transform.position;
                obj.transform.rotation = FirePosition.transform.rotation;
                projectileA.damage = state.damage;
                projectileA.target = target.transform;
                projectileA.Player = gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
            case CharacterState.Type.PiercingShot:
                var projectileP = obj.GetComponent<PiercingShot>();
                projectileP.ResetState();
                obj.transform.position = FirePosition.transform.position;
                obj.transform.rotation = FirePosition.transform.rotation;
                projectileP.damage = state.damage;
                projectileP.target = target.transform;
                projectileP.Player = gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
        }
        

    }

    public void Healing()
    {
        if(target==null)
        {
            return;
        }
        IAttackable heal = target.GetComponent<IAttackable>();

        if (heal != null) 
        {
            heal.OnHealing(1f*state.damage);
        }
    }
    private void OnDrawGizmos()
    {
        if (state == null || state.AttackRange == null || transform == null)
        {
            return; // 하나라도 null이면 Gizmos를 그리지 않음
        }
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Vector3 characterPosition = transform.position; // 캐릭터의 현재 위치
        Vector3 forward = -transform.forward; // 캐릭터가 왼쪽을 바라보므로 Unity의 왼쪽 방향이 포워드
        Vector3 right = transform.right; // 캐릭터의 오른쪽 방향이 실제로는 Unity의 포워드

        int characterRow = 0; // 캐릭터의 행 위치
        int characterCol = 0; // 캐릭터의 열 위치

        // 캐릭터 위치 ('2') 찾기
        for (int i = 0; i < state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < state.AttackRange.GetLength(1); j++)
            {
                if (state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        // 공격 가능 범위 ('1')에 대한 Gizmos 그리기
        for (int i = 0; i < state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < state.AttackRange.GetLength(1); j++)
            {
                if (state.AttackRange[i, j] == 1)
                {
                    // 배열에서 캐릭터를 기준으로 한 상대 위치 계산
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    // 월드 좌표에 상대 위치 더하기
                    Vector3 gizmoPosition = characterPosition + relativePosition;
                    // Gizmos 큐브 그리기
                    Gizmos.DrawCube(gizmoPosition, Vector3.one); // 큐브 사이즈는 1x1x1
                }
            }
        }
    }
}
