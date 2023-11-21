using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    private PlayableStateManager stateManager = new PlayableStateManager();
    private List<PlayableBaseState> states = new List<PlayableBaseState>();
    public GameObject projectilePrefab;

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

    }
    void Start()
    {
        states.Add(new PlayableIdleState(this));
        states.Add(new PlayableDieState(this));
        if(state.occupation == CharacterState.Occupation.Hunter || state.occupation == CharacterState.Occupation.Castor)
        {
            states.Add(new PlayableProjectileAttackState(this));
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
        TakeDamage take = target.GetComponent<TakeDamage>();

        take.OnAttack(state.damage + Rockpaperscissors());
        Debug.Log("PlayerAttack" + (state.damage + Rockpaperscissors()));
    }
    public float Rockpaperscissors()
    {
        float compatibility = state.damage * 0.1f;

        EnemyController enemy = target.GetComponent<EnemyController>();

        if (state.property == enemy.state.property)
        {
            return 0;
        }
     
        switch (state.property)
        {
            case CharacterState.Property.Prime:
                return (enemy.state.property == CharacterState.Property.Edila) ? compatibility :  -compatibility;
            case CharacterState.Property.Edila:
                return (enemy.state.property == CharacterState.Property.Grieve) ? compatibility : -compatibility;
            case CharacterState.Property.Grieve:
                return (enemy.state.property == CharacterState.Property.Prime) ? compatibility : -compatibility;
            default:
                return 0;
        }
    }
   
    public void SetBlockCount()
    {
        switch (state.occupation)
        {
            case CharacterState.Occupation.Guardian:
                maxBlockCount = 4;
                break;
            case CharacterState.Occupation.Striker:
                maxBlockCount = 2;
                break;
            case CharacterState.Occupation.Castor:
                maxBlockCount = 0;
                break;
            case CharacterState.Occupation.Hunter:
                maxBlockCount = 0;
                break;
            case CharacterState.Occupation.Supporters:
                break;
            default:
                break;
        }

    }

    public void Fire()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Bullet projectile = projectileObject.GetComponent<Bullet>();
        projectile.damage = state.damage;
        projectile.target = target.transform;
    }

    public void Healing()
    {
        if(target==null)
        {
            return;
        }
        TakeDamage heal = target.GetComponent<TakeDamage>();
        if (heal != null) 
        {
            Debug.Log("HEALING!!!");
            heal.OnAttack(-1f*state.damage);
        }
    }
}
