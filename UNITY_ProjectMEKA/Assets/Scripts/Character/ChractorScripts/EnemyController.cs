using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{ 
    private StateManager stateManager = new StateManager();
    private List<NPCBaseState> states = new List<NPCBaseState>();

    // 11.22, 김민지, 이동방식 변경으로 인해 추가
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Vector3 initPos;
    [HideInInspector]
    public int waypointCount = 0;

    public CharacterState state;
    public Transform[] wayPoint;
    public GameObject target;

    public enum NPCStates
    {
        Idle,
        Move,
        Attack,

    }
    private void OnEnable()
    {
        // 11.22, 김민지, enemy 재활용 시 move상태 enter 함수 호출용도
        if (states.Count != 0)
        {
            SetState(NPCStates.Move);
        }
    }
    private void Awake()
    {
        state = GetComponent<CharacterState>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        states.Add(new NPCIdleState(this));
        states.Add(new NPCDestinationStates(this));
        states.Add(new NPCAttackState(this));
        SetState(NPCStates.Move);
    }

    private void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    private void Update()
    {
        stateManager.Update();
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

