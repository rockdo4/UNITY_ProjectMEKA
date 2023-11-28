using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum NPCStates
{
    Idle,
    Move,
    Attack,
}


public class EnemyController : PoolAble
{ 
    private StateManager stateManager = new StateManager();
    [HideInInspector]
    public List<NPCBaseState> states = new List<NPCBaseState>();

    // 11.22, 김민지, 이동방식 변경으로 인해 추가
    //[HideInInspector]
    public Rigidbody rb;
    //[HideInInspector]
    public Vector3 initPos;
    //[HideInInspector]
    public int waypointIndex = 0;
    //[HideInInspector]
    public Defines.MoveType moveType;
    //[HideInInspector]
    public int moveRepeatCount;
    //[HideInInspector]
    public Transform[] wayPoint;

    //[HideInInspector]
    public Animator ani;
    //[HideInInspector]
    public CharacterState state;
    //[HideInInspector]
    public GameObject target;
    public GameObject healingTarget;
    public GameObject HoIsHitMe;
    public GameObject FirePosition;



    public Vector3 CurrentPos;
    [HideInInspector]
    public Vector3Int CurrentGridPos;//유니티 상 현제 위치의  타일위치

    [HideInInspector]
    public bool isArrival = false;

    private void OnEnable()
    {
        if (states.Count != 0)
        {
            SetState(NPCStates.Move);
        }
        isArrival = false;
    }
    private void Awake()
    {
        state = GetComponent<CharacterState>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }
    
    void Start()
    {
        state.ConvertTo2DArray();
        states.Add(new NPCIdleState(this));
        states.Add(new NPCDestinationStates(this));
        if (state.enemyType == Defines.EnemyType.LongDistance)
        {
            states.Add(new NPCProjectileAttackState(this));

        }
        else
        {
            states.Add(new NPCAttackState(this));
        }
        SetState(NPCStates.Move);
        foreach(var P in state.passive)
        {
            switch (P)
            {
                case CharacterState.Passive.Unstoppable:
                    gameObject.AddComponent<Unstoppable>(); 
                    break;
                case CharacterState.Passive.Explosion:
                    gameObject.AddComponent<Explosion>();
                    break;
                case CharacterState.Passive.BusterCall:
                    gameObject.AddComponent<BusterCall>();
                    break;
                case CharacterState.Passive.SpeedUp:
                    gameObject.AddComponent<SpeedUp>();
                    break;
                case CharacterState.Passive.Counterattack:
                    gameObject.AddComponent<Counterattack>();
                    break;
                case CharacterState.Passive.Spite:
                    gameObject.AddComponent<Spite>();
                    break;
                case CharacterState.Passive.Outlander:
                    gameObject.AddComponent<Outlander>();
                    break;
                case CharacterState.Passive.Tenacity:
                    gameObject.AddComponent<Tenacity>();
                    break;
                case CharacterState.Passive.Revenge:
                    gameObject.AddComponent<Revenge>();
                    break;
                case CharacterState.Passive.Mechanic:
                    gameObject.AddComponent<Mechanic>();
                    break;

            }
        }
    }

    private void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    private void Update()
    {
        stateManager.Update();
        CurrentPos = transform.position;
        CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
        //Debug.Log(state.damage);
    }

    public void SetState(NPCStates state)
    {
        stateManager.ChangeState(states[(int)state]);
    }
    
    public void Hit()
    {
        if(target == null)
        {
            return;
        }
        TakeDamage co = target.GetComponent<TakeDamage>();
        co.OnAttack(state.damage + Rockpaperscissors());
    }
    public void Hit(float damage)
    {
        if (target == null)
        {
            return;
        }
        
        TakeDamage co = target.GetComponent<TakeDamage>();
        co.OnAttack(state.damage * damage);
    }
    public void Healing(float damage)
    {
        if (target == null)
        {
            return;
        }

        TakeDamage co = healingTarget.GetComponent<TakeDamage>();
        co.OnHealing(state.damage * damage);
    }
    public void Fire()
    {
        if (target == null) return;
        //var obj = ObjectPoolManager.instance.GetGo("bullet");
        var obj = ObjectPoolManager.instance.GetGo(state.BulletName);

        //obj.transform.LookAt(target.transform.position);
        var projectile = obj.GetComponent<Bullet>();
        projectile.ResetState();
        obj.transform.position = FirePosition.transform.position;
        obj.transform.rotation = FirePosition.transform.rotation;
        projectile.damage = state.damage;
        projectile.target = target.transform;
        projectile.Player = gameObject;
        obj.SetActive(false);
        obj.SetActive(true);

        
    }
    public float Rockpaperscissors()
    {
        float compatibility = state.damage * 0.1f;

        PlayerController enemy = target.GetComponent<PlayerController>();

        if (state.property == enemy.state.property)
        {
            return 0;
        }

        switch (state.property)
        {
            case Defines.Property.Prime:
                return (enemy.state.property == Defines.Property.Edila) ? compatibility : -compatibility;
            case Defines.Property.Edila:
                return (enemy.state.property == Defines.Property.Grieve) ? compatibility : -compatibility;
            case Defines.Property.Grieve:
                return (enemy.state.property == Defines.Property.Prime) ? compatibility : -compatibility;
            default:
                return 0;
        }
    }
    private void OnDrawGizmos()
    {
        if (state == null || state.AttackRange == null || transform == null)
        {
            return; // 하나라도 null이면 Gizmos를 그리지 않음
        }
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Vector3 characterPosition = transform.position;
        //Vector3 forward = -transform.forward;
        Vector3 forward = transform.right;
        //Vector3 right = transform.right;
        Vector3 right = -transform.forward;

        int characterRow = 0;
        int characterCol = 0;

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

        for (int i = 0; i < state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < state.AttackRange.GetLength(1); j++)
            {
                if (state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 gizmoPosition = characterPosition + relativePosition;
                    Gizmos.DrawCube(gizmoPosition, Vector3.one);
                }
            }
        }
    }
}

