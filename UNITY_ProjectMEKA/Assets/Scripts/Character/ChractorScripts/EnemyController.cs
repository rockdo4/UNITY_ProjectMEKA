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
    public CharacterState state;
    public Transform[] wayPoint;
    public GameObject target;
    public enum NPCStates
    {
        Idle,
        Move,
        Attack,

    }
    private void Awake()
    {
        state = GetComponent<CharacterState>();
    }
    void Start()
    {
        states.Add(new NPCIdleState(this));
        states.Add(new NPCDestinationStates(this));
        states.Add(new NPCAttackState(this));
        
        SetState(NPCStates.Move);
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
            case CharacterState.Property.Prime:
                return (enemy.state.property == CharacterState.Property.Edila) ? compatibility : -compatibility;
            case CharacterState.Property.Edila:
                return (enemy.state.property == CharacterState.Property.Grieve) ? compatibility : -compatibility;
            case CharacterState.Property.Grieve:
                return (enemy.state.property == CharacterState.Property.Prime) ? compatibility : -compatibility;
            default:
                return 0;
        }
    }
  
}

