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

    public Vector3 CurrentPos;
    [HideInInspector]
    public Vector3Int CurrentGridPos;

    [HideInInspector]
    public bool isArrival = false;
    public Vector3Int forwardGrid; 
    
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

        // 포워드 방향으로 1 유닛 이동
        

       
        // 대각선 방향을 제거하고, 가장 큰 축을 기준으로 설정
        if (Mathf.Abs(forwardDirection.x) > Mathf.Abs(forwardDirection.z))
        {
            forwardDirection = new Vector3(Mathf.Sign(forwardDirection.x), 0, 0);
        }
        else
        {
            forwardDirection = new Vector3(0, 0, Mathf.Sign(forwardDirection.z));
        }

        // 포워드 방향으로 1 유닛 이동
        Vector3 newPosition = currentPosition + forwardDirection;

        // 그리드 인덱스 계산
        forwardGrid = new Vector3Int(Mathf.RoundToInt(newPosition.x), 0, Mathf.RoundToInt(newPosition.z));


        
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
        TakeDamage co = target.GetComponentInParent<TakeDamage>();
        if (co == null) return;
        co.OnAttack(state.damage + Rockpaperscissors());
    }
    public void Hit(float damage)
    {
        if (target == null)
        {
            return;
        }
        
        IAttackable co = target.GetComponentInParent<IAttackable>();
        co.OnAttack((state.damage + Rockpaperscissors() * 1f * 1f) - (target.GetComponentInParent<EnemyController>().state.armor + 1f) * 1f);

        co.OnAttack(state.damage * damage);
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
        if (target == null) return;
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
    //void CreateColliders()
    //{
    //    if (state == null || state.AttackRange == null || transform == null)
    //    {
    //        return;
    //    }


    //    //Vector3 forward = transform.right;
    //    //Vector3 right = -transform.forward;
    //    //Vector3 parentScale = transform.localScale;
    //    Vector3 forward = -transform.forward;
    //    Vector3 right = transform.right;
    //    Vector3 parentScale = transform.localScale;
    //    int characterRow = 0;
    //    int characterCol = 0;

    //    for (int i = 0; i < state.AttackRange.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < state.AttackRange.GetLength(1); j++)
    //        {
    //            if (state.AttackRange[i, j] == 2)
    //            {
    //                characterRow = i;
    //                characterCol = j;
    //            }
    //        }
    //    }

    //    for (int i = 0; i < state.AttackRange.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < state.AttackRange.GetLength(1); j++)
    //        {
    //            if (state.AttackRange[i, j] == 1)
    //            {
    //                Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
    //                Vector3 correctedPosition = new Vector3(relativePosition.x / parentScale.x, relativePosition.y / parentScale.y, relativePosition.z / parentScale.z);

    //                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
    //                collider.size = new Vector3(1 / parentScale.x, 3 / parentScale.y, 1 / parentScale.z);
    //                collider.center = correctedPosition;
    //                collider.isTrigger = true;
    //            }
    //        }
    //    }


    //}
    //private void OnDrawGizmos()
    //{
    //    if (state == null || state.AttackRange == null) return;

    //    Gizmos.color = Color.red; // 기즈모 색상 설정

    //    //Vector3 forward = transform.right;
    //    //Vector3 right = -transform.forward;
    //    //Vector3 parentScale = transform.localScale;


    //    Vector3 forward = -transform.forward;
    //    Vector3 right = transform.right;
    //    Vector3 parentScale = transform.localScale;

    //    int characterRow = 0;
    //    int characterCol = 0;

    //    // 캐릭터 위치 찾기
    //    for (int i = 0; i < state.AttackRange.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < state.AttackRange.GetLength(1); j++)
    //        {
    //            if (state.AttackRange[i, j] == 2)
    //            {
    //                characterRow = i;
    //                characterCol = j;
    //            }
    //        }
    //    }

    //    // 콜라이더 기즈모 그리기
    //    for (int i = 0; i < state.AttackRange.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < state.AttackRange.GetLength(1); j++)
    //        {
    //            if (state.AttackRange[i, j] == 1)
    //            {
    //                Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
    //                Vector3 correctedPosition = new Vector3(relativePosition.x / parentScale.x, relativePosition.y / parentScale.y, relativePosition.z / parentScale.z);
    //                Gizmos.DrawCube(transform.position + correctedPosition, new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z));
    //            }
    //        }
    //    }
    //}

}

