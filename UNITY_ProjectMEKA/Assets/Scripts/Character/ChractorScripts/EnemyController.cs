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
        states.Add(new NPCIdleState(this));
        states.Add(new NPCDestinationStates(this));
        states.Add(new NPCAttackState(this));
        SetState(NPCStates.Move);
        foreach(var P in state.passive)
        {
            //Unstoppable,//저지 불가
            //Explosion,//자폭 (완)
            //BusterCall,//지원 전술 (완)
            //SpeedUp,//이속 증가 (완)
            //Counterattack,//역습 (완)
            //Spite,//악의 (완)
            //Outlander,//아웃랜더
            //Tenacity,//망자의 집념(완)
            //Revenge,//보복(완)
            //Mechanic,//정비공 (완)


            switch (P)
            {
                case CharacterState.Passive.Unstoppable:
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
}

