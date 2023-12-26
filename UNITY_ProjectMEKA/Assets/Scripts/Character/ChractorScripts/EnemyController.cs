using System.Collections.Generic;
using UnityEngine;
using static Defines;
public enum NPCStates
{
    Idle,
    Move,
    Attack,
    Stun,
}


public class EnemyController : PoolAble
{
    [HideInInspector]
    public StateManager stateManager = new StateManager();
    [HideInInspector]
    public List<NPCBaseState> states = new List<NPCBaseState>();
    public string victonEffectName;
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
    public EnemyState state;
    //[HideInInspector]
    public GameObject target;
    public GameObject healingTarget;
    public GameObject HoIsHitMe;
    public GameObject FirePosition;
    public List<GameObject> rangeInPlayers = new List<GameObject>();
    public List<GameObject> rangeInSecondPlayers = new List<GameObject>();//보스 공격범위
    public NPCStates currentState;
    public Vector3 CurrentPos;
    [HideInInspector]
    public Vector3Int CurrentGridPos;

    [HideInInspector]
    public bool isArrival = false;
    [HideInInspector]
    public Vector3Int forwardGrid;
    //[HideInInspector]
    public int bossAttackCount;

    [HideInInspector]
    public float stunTime;
    [HideInInspector]
    public bool isMove;

    private void OnEnable()
    {
        if (states.Count != 0)
        {
            SetState(NPCStates.Move);
        }
        isArrival = false;
        //CreateColliders();

        state.Hp = state.maxHp;
        rangeInPlayers.Clear();
        bossAttackCount = 0;

    }


	private void Awake()
    {
        state = GetComponent<EnemyState>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        state.isBlock = true;
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
        states.Add(new NPCStunState(this));
        SetState(NPCStates.Move);
        foreach(var P in state.passive)
        {
            switch (P)
            {
                case Passive.Unstoppable:
                    gameObject.AddComponent<Unstoppable>(); 
                    break;
                case Passive.Explosion:
                    gameObject.AddComponent<Explosion>();
                    break;
                case Passive.BusterCall:
                    gameObject.AddComponent<BusterCall>();
                    break;
                case Passive.SpeedUp:
                    gameObject.AddComponent<SpeedUp>();
                    break;
                case Passive.Counterattack:
                    gameObject.AddComponent<Counterattack>();
                    break;
                case Passive.Spite:
                    gameObject.AddComponent<Spite>();
                    break;
                case Passive.Outlander:
                    gameObject.AddComponent<Outlander>();
                    break;
                case Passive.Tenacity:
                    gameObject.AddComponent<Tenacity>();
                    break;
                case Passive.Revenge:
                    gameObject.AddComponent<Revenge>();
                    break;
                case Passive.Mechanic:
                    gameObject.AddComponent<Mechanic>();
                    break;

            }
        }
        //CreateColliders();
    }

    private void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position; // 현재 위치
        Vector3 forwardDirection = transform.forward; // 포워드 방향
        
        if (Mathf.Abs(forwardDirection.x) > Mathf.Abs(forwardDirection.z))
        {
            forwardDirection = new Vector3(Mathf.Sign(forwardDirection.x), 0, 0);
        }
        else
        {
            forwardDirection = new Vector3(0, 0, Mathf.Sign(forwardDirection.z));
        }

        //Vector3 newPosition = currentPosition + forwardDirection * 2f;//전방 2타일 감지하는 로직
        Vector3 newPosition = currentPosition + forwardDirection;

        //forwardGrid = new Vector3Int(Mathf.FloorToInt(newPosition.x), 0, Mathf.FloorToInt(newPosition.z));
        forwardGrid = new Vector3Int(Mathf.RoundToInt(newPosition.x), 0, Mathf.RoundToInt(newPosition.z));
        
        stateManager.Update();
        CurrentPos = transform.position;
        //CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
        CurrentGridPos = new Vector3Int(Mathf.RoundToInt(CurrentPos.x), Mathf.RoundToInt(CurrentPos.y), Mathf.RoundToInt(CurrentPos.z));
        
    }
    
    public void SetState(NPCStates state)
    {
        stateManager.ChangeState(states[(int)state]);
        currentState = state;
        Debug.Log($"{gameObject.name}: {state}");
    }
    public void WheelWindEffect()
    {
        var poolObject = ObjectPoolManager.instance.GetGo(victonEffectName);
        poolObject.transform.position = gameObject.transform.position;
        poolObject.SetActive(false);
        poolObject.SetActive(true);
        poolObject.GetComponent<PoolAble>().ReleaseObject(1.2f);
    }

    public void Hit()
    {
        if(target == null)
        {
            return;
        }
        IAttackable co = target.GetComponentInParent<IAttackable>();
        if (co == null) return;
        co.OnAttack(state.damage + Rockpaperscissors());
        if (state.enemyType == EnemyType.OhYaBung || state.enemyType == EnemyType.YangSehyung)
        {
            bossAttackCount++;
        }

    }
    public void Hit(float damage)
    {
        if (target == null)
        {
            return;
        }
        
        IAttackable co = target.GetComponentInParent<IAttackable>();

        co.OnAttack(state.damage * damage);
    }
    public void StingerHit()
    {
        foreach(var a in rangeInPlayers)
        {
            IAttackable at = a.GetComponentInParent<IAttackable>();
            if(at == null) continue;
            at.OnAttack(state.damage + Rockpaperscissors());
        }
        if(state.enemyType == EnemyType.OhYaBung||state.enemyType == EnemyType.YangSehyung)
        {
            bossAttackCount++;
        }

    }
    public void WheelWind()
    {
        foreach(var player in rangeInPlayers)
        {
            player.GetComponentInParent<IAttackable>().OnAttack(state.damage * 0.3f);
        }   
    }
    public void Healing(float damage)
    {
        if (target == null)
        {
            return;
        }

        IAttackable co = healingTarget.GetComponent<IAttackable>();
        co.OnHealing(state.damage * damage);
    }
    
    public void Fire()
    {
        if (target == null || this.gameObject == null) return;
        //var obj = ObjectPoolManager.instance.GetGo("bullet");
        var obj = ObjectPoolManager.instance.GetGo(state.BulletName);

        //obj.transform.LookAt(target.transform.position);
        //if(obj == null) return; 

        switch (state.BulletType)
        {
            case ProjectileType.Bullet:
                var projectile = obj.GetComponent<Bullet>();
                projectile.ResetState();
                obj.transform.position = FirePosition.transform.position;
                obj.transform.rotation = FirePosition.transform.rotation;
                projectile.damage = state.damage;
                projectile.target = target.transform;
                projectile.Player = gameObject;
                
                obj.SetActive(false);
                if (obj == null)
                {
                    return;
                }
                obj.SetActive(true);
                break;
            case ProjectileType.Aoe:
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
            case ProjectileType.PiercingShot:
                var projectileP = obj.GetComponent<PiercingShot>();
                projectileP.ResetState();
                obj.transform.position = FirePosition.transform.position;
                obj.transform.LookAt(target.transform.position);
                projectileP.damage = state.damage;
                projectileP.target = target.transform;
                projectileP.Player = gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
        }


    }
    public float Rockpaperscissors()
    {
        float compatibility = state.damage * 0.1f;

        PlayerController enemy = target.GetComponentInParent<PlayerController>();

        if(enemy == null)
        {
            return 0;
        }

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

