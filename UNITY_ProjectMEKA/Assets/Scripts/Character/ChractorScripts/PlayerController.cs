using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    private PlayableStateManager stateManager = new PlayableStateManager();
    private List<PlayableBaseState> states = new List<PlayableBaseState>();
    public GameObject projectilePrefab;
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

    }
    private void OnEnable()//인게임에서 배치할때 오브젝트풀링을 고려함
    {
        CurrentPos = transform.position;
        CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
        
    }
    void Start()
    {
        states.Add(new PlayableIdleState(this));
        states.Add(new PlayableDieState(this));
        if(state.occupation == Defines.Occupation.Hunter || state.occupation == Defines.Occupation.Castor)
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
        //transform.LookAt(target.transform.position);
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Bullet projectile = projectileObject.GetComponent<Bullet>();
        if(projectile == null)
        {
            AOE boom = projectileObject.GetComponent<AOE>();
            if(boom == null)
            {
                PiercingShot piercingShot = projectileObject.GetComponent<PiercingShot>();
                piercingShot.damage = state.damage;
                piercingShot.target = target.transform;
                return;
            }
            boom.damage = state.damage;
            boom.target = target.transform;
            return;
        }
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
            heal.OnHealing(1f*state.damage);
        }
    }
    public void OnDrawGizmos()
    {
        if (state != null)
        {
            Gizmos.color = Color.red;
            Vector3 endPos = transform.position + transform.forward * state.range;

            Gizmos.DrawLine(transform.position,  endPos);
        }
        
    }
}
