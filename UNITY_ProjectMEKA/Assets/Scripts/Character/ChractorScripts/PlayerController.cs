using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    //[HideInInspector]
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

    public List<Tile> attackableTiles = new List<Tile>();
    public List<Tile> arrangableTiles = new List<Tile>();
    public List<Tile> attackableSkillTiles = new List<Tile>();
    public List<Tile> prevAttackableSkillTiles = new List<Tile>();
    public SkillBase skillState;

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
    private bool CharacterArrangeOne;
    private float addCostTimer;
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
    public TileManager tileManager;
    public bool isDie;

    private void Awake()
    {
        skillState = GetComponent<SkillBase>();
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
        
        //CreateColliders();

        rangeInEnemys.Clear();
        rangeInPlayers.Clear();
        enemyBlockCount.Clear();
        state.Hp = state.maxHp;
        CharacterArrangeOne = false;
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
        //if(!CharacterArrangeOne)
        //{
        //    CurrentPos = transform.position;
        //    CurrentGridPos = new Vector3Int(Mathf.RoundToInt(CurrentPos.x), 0, Mathf.RoundToInt(CurrentPos.z));

        //}

        //Debug.Log(stateManager.currentBase is PlayableArrangeState);
        //Debug.Log($"{CurrentGridPos},{gameObject.name}");
        stateManager.Update();
        blockCount = enemyBlockCount.Count;

        addCostTimer += Time.deltaTime;
        if(addCostTimer >= 2f)
        {
            addCostTimer = 0;
            state.cost += 2f;
            if (state.cost >= state.maxCost)
            {
                state.cost = state.maxCost;
            }
        }
        
        
        foreach (var a in rangeInEnemys)
        {
            if (!a.activeSelf)
            {
                rangeInEnemys.Remove(a);
            }
        }

        //OnClickDown();
        OnClickDownCharacter();

        Vector3 mousePosition = Vector3.zero;
        if(skillState.isSkillUsing)
        {
            mousePosition = OnClickDownSkillTile();
            if(mousePosition != Camera.main.transform.position && prevAttackableSkillTiles != attackableSkillTiles)
            {
                AttackableSkillTileSet(mousePosition);
            }
        }
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
        
        take.OnAttack((state.damage + Rockpaperscissors() * 1f * 1f) - (target.GetComponentInParent<EnemyController>().state.armor + 1f) * 1f);
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
                foreach (var tile in stageManager.tileManager.lowTiles)
                {
                    if (tile.Item1.arrangePossible)
                    {
                        arrangableTiles.Add(tile.Item1);
                    }
                }
                break;
            default:
                foreach (var tile in stageManager.tileManager.highTiles)
                {
                    if (tile.Item1.arrangePossible)
                    {
                        arrangableTiles.Add(tile.Item1);
                    }
                }
                break;
        }

        // modified code
        //바닥타일들 가져와서
        // arrangePossible 검사하고 arrangableTiles에 ADD

        //// origin code
        //var tileParent = GameObject.FindGameObjectWithTag(tag);
        //var tileCount = tileParent.transform.childCount;
        //var tiles = new List<Tile>();
        //for (int i = 0; i < tileCount; ++i)
        //{
        //    if (tileParent.transform.GetChild(i).GetComponentInChildren<Tile>().arrangePossible)
        //    {
        //        tiles.Add(tileParent.transform.GetChild(i).GetComponentInChildren<Tile>());
        //    }
        //}
        //arrangableTiles = tiles;
    }

    public void AttackableTileSet(Defines.Occupation occupation)
    {
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

        if (attackableTiles.Count > 0)
        {
            attackableTiles.Clear();
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
                            attackableTiles.Add(tileContoller);                            
                        }
                    }
                }
            }
        }
    }

    public Vector3 OnClickDownSkillTile()
    {
        var skill = skillState as BuffSkilType;
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
        layerMask = lowTileMask | highTileMask;
        Vector3 mousePosition = Camera.main.transform.position;
        RaycastHit hit1;

        // 레이를 쏴서 타일에 맞았을 때, 그 타일이 어태커블 타일이면, 마우스 포지션에 해당 좌표의 vector3 int값 저장
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit1, Mathf.Infinity, layerMask) && Input.GetMouseButton(0))
        {
            mousePosition = hit1.point;
        }
        return mousePosition;
    }

    public void AttackableSkillTileSet(Vector3 mousePosition)
    {
        // temporary code
        var skill = skillState as BuffSkilType;
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
        layerMask = lowTileMask | highTileMask;
        int mouseRow = 0;
        int mouseCol = 0;

        for (int i = 0; i < skill.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < skill.AttackRange.GetLength(1); j++)
            {
                if (skill.AttackRange[i, j] == 2)
                {
                    mouseRow = i;
                    mouseCol = j;
                }
            }
        }

        foreach (var tile in prevAttackableSkillTiles)
        {
            //tile.ClearTileMesh();
            stageManager.ingameStageUIManager.ClearTileMesh();
        }
        prevAttackableSkillTiles.Clear();

        if (attackableSkillTiles.Count > 0)
        {
            attackableSkillTiles.Clear();
        }

        for (int i = 0; i < skill.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < skill.AttackRange.GetLength(1); j++)
            {
                if (skill.AttackRange[i, j] == 1 || skill.AttackRange[i, j] == 2)
                {
                    Vector3 relativePosition = (i - mouseRow) * Vector3.forward + (j - mouseCol) * Vector3.right;

                    // modified code
                    var mousePosInt = Utils.Vector3ToVector3Int(mousePosition);
                    Vector3 tilePosition = mousePosInt + relativePosition;
                    Vector3Int tilePosInt = Utils.Vector3ToVector3Int(tilePosition);
                    foreach( var attackableTile in attackableTiles)
                    {
                        foreach (var tile in stageManager.tileManager.allTiles)
                        {
                            var xEqual = tile.Item2.x == tilePosInt.x;
                            var zEqual = tile.Item2.z == tilePosInt.z;
                            if (xEqual && zEqual && tile.Item1.arrangePossible)
                            {
                                attackableSkillTiles.Add(tile.Item1);
                                prevAttackableSkillTiles.Add(tile.Item1);
                                if (attackableTile.index == mousePosInt)
                                {
                                    stageManager.ingameStageUIManager.ChangeSkillTileMesh();
                                }
                                else
                                {
                                    stageManager.ingameStageUIManager.ChangeUnActiveTileMesh();
                                }
                            }
                        }
                        break;
                    }

                    // origin code
                    //Vector3 tilePosition = mousePosition + relativePosition;
                    //var tilePosInt = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);
                    //RaycastHit hit2;
                    //var tempPos = new Vector3(tilePosInt.x, tilePosInt.y - 10f, tilePosInt.z);
                    //if (Physics.Raycast(tempPos, Vector3.up, out hit2, Mathf.Infinity, layerMask))
                    //{
                    //    var tileContoller = hit2.transform.GetComponent<Tile>();
                    //    if (!tileContoller.isSomthingOnTile)
                    //    {
                    //        attackableSkillTiles.Add(tileContoller);
                    //        prevAttackableSkillTiles.Add(tileContoller);
                    //        tileContoller.SetTileMaterial(Tile.TileMaterial.Skill);
                    //    }
                    //}
                }
            }
        }
    }

    public void OnClickDownCharacter()
    {
        var otherCharacterArrange = stageManager.ingameStageUIManager.windowMode == WindowMode.FirstArrange || stageManager.ingameStageUIManager.windowMode == WindowMode.SecondArrange;
        var otherCharacterSetting = stageManager.ingameStageUIManager.windowMode == WindowMode.Setting;

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
