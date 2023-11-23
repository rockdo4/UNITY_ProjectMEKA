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

    // 11.22, �����, �̵���� �������� ���� �߰�
    [HideInInspector]
    public Rigidbody rb;
    //[HideInInspector]
    public Vector3 initPos;
    [HideInInspector]
    public int waypointIndex = 0;
    [HideInInspector]
    public Defines.MoveType moveType;
    [HideInInspector]
    public int moveRepeatCount;

    public CharacterState state;
    public Transform[] wayPoint;
    public GameObject target;

    private Vector3 CurrentPos;
    [HideInInspector]
    public Vector3Int CurrentGridPos;//����Ƽ �� ���� ��ġ��  Ÿ����ġ

    public enum NPCStates
    {
        Idle,
        Move,
        Attack,

    }
    private void OnEnable()
    {
        // 11.22, �����, enemy ��Ȱ�� �� move���� enter �Լ� ȣ��뵵
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
        CurrentPos = transform.position;
        CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
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

