﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Defines;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayableStateManager stateManager = new PlayableStateManager();
    [HideInInspector]
    public List<PlayableBaseState> states = new List<PlayableBaseState>();
    public GameObject FirePosition;
    [HideInInspector]
    public Animator ani;
    
    private Vector3 CurrentPos;

    [HideInInspector]
    public Vector3Int CurrentGridPos;
    [HideInInspector]
    public PlayerState state;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public int blockCount;
    [HideInInspector]
    public int maxBlockCount;
    [HideInInspector]
    public Collider[] enemys;
    //[HideInInspector]
    public List<GameObject> rangeInEnemys = new List<GameObject>();
    //[HideInInspector]
    public List<GameObject> rangeInPlayers = new List<GameObject>();
    [HideInInspector]
    public List<int> enemyBlockCount = new List<int>();

    [HideInInspector]
    public int skillCost;

    [HideInInspector]
    public float skillCoolTime;

    public List<Tile> attakableTiles = new List<Tile>();
    public List<Tile> arrangableTiles = new List<Tile>();
    [HideInInspector]
    public UnityEvent PlayerInit;
    public GameObject joystick;
    [HideInInspector]
    public CharacterIcon icon;
    [HideInInspector]
    public Tile currentTile;

    public Transform parentPos;
    public Transform ChildPos;

    public RuntimeAnimatorController animationController;
    public RuntimeAnimatorController currnetAnimationController;

    public enum CharacterStates
    {
        Arrange,
        Idle,
        Die,
        Attack,
        Healing,
    }
    [HideInInspector]
    public CharacterStates currentState;

    public StageManager stageManager;
    public bool isDie;


    private void Awake()
    {
        state = GetComponent<PlayerState>();
        SetBlockCount();
        ani = GetComponent<Animator>();
        PlayerInit = new UnityEvent();
        PlayerInit.AddListener(() =>
        {
            // except currentTile.arrangePossible = true;
            icon.gameObject.SetActive(true);
            stateManager.firstArranged = false;
            stateManager.secondArranged = false;
            stateManager.created = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            gameObject.SetActive(false);
        });
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();

        states.Add(new PlayableArrangeState(this));
        states.Add(new PlayableIdleState(this));
        states.Add(new PlayableDieState(this));

    }
    private void OnEnable()
    {
        CurrentPos = transform.position;
        CurrentGridPos = new Vector3Int(Mathf.FloorToInt(CurrentPos.x), Mathf.FloorToInt(CurrentPos.y), Mathf.FloorToInt(CurrentPos.z));
        //CreateColliders();

        rangeInEnemys.Clear();
        rangeInPlayers.Clear();
        enemyBlockCount.Clear();
        state.Hp = state.maxHp;
    }
    private void OnDisable()
    {
        if (states.Count != 0)
        {
            SetState(CharacterStates.Die);
        }
    }
    private void Start()
    {
        state.ConvertTo2DArray();

        if (state.occupation == Defines.Occupation.Hunter || state.occupation == Defines.Occupation.Castor)
        {
            states.Add(new PlayableProjectileAttackState(this));
        }
        else
        {
            states.Add(new PlayableAttackState(this));
        }
        states.Add(new PlayableHealingState(this));

        //CreateColliders();

        switch (state.skill)
        {
            case Skills.Snapshot:
                var s = gameObject.AddComponent<Snapshot>();
                break;
            case Skills.StunningBlow:
                var b = gameObject.AddComponent<StunningBlow>();
                break;
            case Skills.AmberSkill:
                var a = gameObject.AddComponent<AmberSkill>();
                break;
            case Skills.IYRASkill:
                var i = gameObject.AddComponent<IYRASkill>();
                break;
            case Skills.KALEASkill:
                var k = gameObject.AddComponent<KALEASkill>();
                break;
            case Skills.MERIASkill:
                var m = gameObject.AddComponent<MERIASkill>();
                break;
            case Skills.PALASkill:
                var p = gameObject.AddComponent<PALASkill>();
                break;
            case Skills.RYUSIENSkill:
                var r = gameObject.AddComponent<RYUSIENSkill>();
                break;
            case Skills.ISABELLASkill:
                var isa = gameObject.AddComponent<ISABELLASkill>();
                break;
        }
        state.cost = state.maxCost;

        switch (state.BulletType)
        {
            case ProjectileType.HitScan:
                gameObject.AddComponent<HitScan>();
                break;
            case ProjectileType.Instantaneous:
                gameObject.AddComponent<Instantaneous>();
                break;
        }
        parentPos = transform;
        ChildPos = GetComponentInChildren<CreateCollider>().gameObject.transform;
    }
    private void Update()
    {
        //Debug.Log(stateManager.currentBase is PlayableArrangeState);

        stateManager.Update();
        blockCount = enemyBlockCount.Count;
        state.cost += Time.deltaTime;
        //Debug.Log(state.cost);
        if(state.cost >= state.maxCost)
        {
            state.cost = state.maxCost;
        }
        //Debug.Log(CurrentGridPos);
        
        foreach (var a in rangeInEnemys)
        {
            if (!a.activeSelf)
            {
                rangeInEnemys.Remove(a);
            }
        }

        //OnClickDown();
        OnClickDown();

        

    }
    
    public void SetState(CharacterStates state)
    {
        stateManager.ChangeState(states[(int)state]);
        currentState = state;
    }

    public void Hit()
    {
        if (target == null)
        {
            return;
        }
        IAttackable take = target.GetComponentInParent<IAttackable>();
        
        if(take == null)
        {
            return;
        }
        //(몬스터 공격력 x 공격력 계수 x 스킬 계수 x 속성 데미지 계수)-(캐릭터 방어력 + 장비 방어력 수치) x 장비 방어력 계수
        
        take.OnAttack((state.damage + Rockpaperscissors() * 1f * 1f) - (target.GetComponentInParent<EnemyController>().state.amror + 1f) * 1f);
        //take.OnAttack(state.damage + Rockpaperscissors());
        
    }
    public float Rockpaperscissors()
    {
        float compatibility = state.damage * 0.1f;

        EnemyController enemy = target.GetComponentInParent<EnemyController>();
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
                maxBlockCount = 5;
                break;
            case Defines.Occupation.Striker:
                maxBlockCount = 3;
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
        if(obj != null)
        {
            Debug.Log(state.BulletName);
        }

        //obj.transform.LookAt(target.transform.position);

        switch (state.BulletType)
        {
            case ProjectileType.None:
                break;
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
                projectileA.Player = transform.gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
            case ProjectileType.PiercingShot:
                var projectileP = obj.GetComponent<PiercingShot>();
                projectileP.ResetState();
                obj.transform.position = FirePosition.transform.position;
                Vector3 newTargetPos = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);
                obj.transform.LookAt(newTargetPos);
                projectileP.damage = state.damage;
                projectileP.target = target.transform;
                projectileP.Player = gameObject;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
            case ProjectileType.ChainAttack:
                ChainAttack magic = obj.GetComponent<ChainAttack>();
                magic.ResetState(); 
                Vector3 newTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);
                obj.transform.position = newTargetPosition;
                magic.damage = state.damage;
                magic.target = target.transform;
                obj.SetActive(false);
                obj.SetActive(true);
                break;
            case ProjectileType.HitScan:
                GetComponent<HitScan>().Shoot();
                break;
            case ProjectileType.Instantaneous:
                GetComponent<Instantaneous>().Shoot();

                break;
            
        }


    }

    public void Healing()
    {
        if(target==null)
        {
            return;
        }

        IAttackable heal = target.GetComponentInParent<IAttackable>();
        var FlashInstance = ObjectPoolManager.instance.GetGo(state.hitName);
        FlashInstance.transform.position = target.transform.position;
        FlashInstance.transform.rotation = target.transform.rotation;
        FlashInstance.SetActive(false);
        FlashInstance.SetActive(true);
        FlashInstance.GetComponent<PoolAble>().ReleaseObject(2f);

        if (heal != null) 
        {
            heal.OnHealing(1f*state.damage);
        }
    }

    public void ArrangableTileSet(Defines.Occupation occupation)
    {
        string tag;
        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                tag = Tags.lowTile;
                break;
            default:
                tag = Tags.highTile;
                break;
        }

        var tileParent = GameObject.FindGameObjectWithTag(tag);
        var tileCount = tileParent.transform.childCount;
        var tiles = new List<Tile>();
        for (int i = 0; i < tileCount; ++i)
        {
            if (tileParent.transform.GetChild(i).GetComponentInChildren<Tile>().arrangePossible)
            {
                tiles.Add(tileParent.transform.GetChild(i).GetComponentInChildren<Tile>());
            }
        }
        arrangableTiles = tiles;
    }

    public void AttackableTileSet(Defines.Occupation occupation)
    {
        Debug.Log("어택타일 업데이트");
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);

        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                layerMask = lowTileMask;
                break;
            default:
                layerMask = lowTileMask | highTileMask;
                break;
        }

        Vector3 characterPosition = transform.position;
        Vector3 forward = -transform.forward;
        Vector3 right = transform.right;
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

        if (attakableTiles.Count > 0)
        {
            attakableTiles.Clear();
        }

        for (int i = 0; i < state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < state.AttackRange.GetLength(1); j++)
            {
                if (state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition;
                    var tilePosInt = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);

                    RaycastHit hit;
                    var tempPos = new Vector3(tilePosInt.x, tilePosInt.y - 10f, tilePosInt.z);
                    if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
                    {
                        var tileContoller = hit.transform.GetComponent<Tile>();
                        if (!tileContoller.isSomthingOnTile)
                        {
                            attakableTiles.Add(tileContoller);                            
                        }
                    }
                }
            }
        }
    }

    public void OnClickDown()
    {
        var otherCharacterArrange = stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.FirstArrange || stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.SecondArrange;
        var otherCharacterSetting = stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting;

        if(otherCharacterArrange || otherCharacterSetting)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer(Layers.playerCollider);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && Input.GetMouseButtonDown(0))
        {
            
            var same = hit.transform.parent.GetComponent<CharacterState>().name == state.name;
            if (same && stageManager.currentPlayer == null)
            {
                Debug.Log($"{state.name} player pointer down");
                SetState(CharacterStates.Arrange);
                stageManager.currentPlayer = this;
                stageManager.currentPlayerIcon = this.icon;
            }
        }
    }
}
