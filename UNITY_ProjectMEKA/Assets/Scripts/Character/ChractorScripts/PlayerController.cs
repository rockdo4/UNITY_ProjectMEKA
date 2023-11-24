using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        obj.transform.position = FirePosition.transform.position; // 발사 위치 설정
        //obj.transform.rotation = transform.rotation; // 회전 초기화
        obj.transform.rotation = FirePosition.transform.rotation; // 회전 초기화
        obj.SetActive(true); // 오브젝트 활성화

        obj.transform.LookAt(target.transform);
        
        switch (state.BulletType)
        {
            case CharacterState.Type.Bullet:
                var projectile = obj.GetComponent<Bullet>();
                projectile.ResetState();
                obj.transform.localPosition = gameObject.transform.position;
                obj.transform.localRotation = Quaternion.identity;
                projectile.transform.LookAt(target.transform.position);
                projectile.damage = state.damage;
                projectile.target = target.transform;
                break;
            case CharacterState.Type.Aoe:
                var projectileA = obj.GetComponent<AOE>();
                projectileA.ResetState();
                obj.transform.localPosition = gameObject.transform.position;
                obj.transform.localRotation = Quaternion.identity;
                projectileA.transform.LookAt(target.transform.position);
                projectileA.damage = state.damage;
                projectileA.target = target.transform;
                break;
            case CharacterState.Type.PiercingShot:
                var projectileP = obj.GetComponent<PiercingShot>();
                projectileP.ResetState();
                projectileP.StartPos = FirePosition.transform;
                projectileP.Init();
                //obj.transform.localPosition = gameObject.transform.position;
                //obj.transform.localRotation = Quaternion.identity;
                ////projectileP.transform.LookAt(target.transform.position);
                projectileP.damage = state.damage;
                projectileP.target = target.transform;
                //projectileP.StartPos = transform;
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
    
}
