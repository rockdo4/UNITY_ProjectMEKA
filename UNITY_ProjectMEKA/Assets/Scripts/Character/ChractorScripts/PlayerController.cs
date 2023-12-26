using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Defines;
using static UnityEngine.ParticleSystem;


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
    //[HideInInspector]
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

    [SerializeField, Header("Hit이팩트가 있는가?")]
    public bool isHitEffect;
    [SerializeField, Header("있다면 이팩트 이름은 무엇인가")]
    public string effectName;

    public RuntimeAnimatorController animationController;
    public RuntimeAnimatorController currnetAnimationController;
    private bool CharacterArrangeOne;
    private float addCostTimer;
    public Transform firstLookPos;
    public Vector3Int skillPivot;
    public TrailRenderer trail;
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
    private (Vector3, Tile) mouseClickInfo;
    private bool isDragging;
    public bool isDie;
    public bool isSkillPossible;

    private void Awake()
    {
        skillState = GetComponent<SkillBase>();
        state = GetComponent<PlayerState>();
        SetBlockCount();
        ani = GetComponent<Animator>();
        trail = GetComponentInChildren<TrailRenderer>();
        if(trail != null) 
        {
            trail.gameObject.SetActive(false);
        }
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
        if (skillState != null)
        {
            if (stageManager.ingameStageUIManager.isSkillTileWindow && (skillState.skillType != SkillType.Instant))
            {
                mouseClickInfo = OnClickSkillTile();
                if (isDragging && prevAttackableSkillTiles != attackableSkillTiles)
                {
                    switch (skillState.skillType)
                    {
                        case SkillType.SnipingArea:
                            AttackableSkillAreaTileSet(mouseClickInfo.Item1);
                            break;
                        case SkillType.SnipingSingle:
                            AttackableSkillSingleTileSet(mouseClickInfo.Item2);
                            break;
                    }
                }
            }
        }
    }
    
    public void SetState(CharacterStates state)
    {
        stateManager.ChangeState(states[(int)state]);
        currentState = state;
        Debug.Log($"{gameObject.name} state : {state}");
    }

    public void Hit()
    {
        
        if (target == null)
        {
            return;
        }
        IAttackable take = target.GetComponentInParent<IAttackable>();
        Vector3 enemyPos = target.GetComponentInParent<EnemyController>().gameObject.transform.position;
        if(enemyPos != null)
        {
            enemyPos.y += 0.5f;
            var obbj = ObjectPoolManager.instance.GetGo("EnemyHitEffect");
            obbj.transform.position = enemyPos;
            obbj.SetActive(false);
            obbj.SetActive(true);
            obbj.GetComponent<PoolAble>().ReleaseObject(1f);
        }
        
        if(take == null)
        {
            return;
        }
        //(몬스터 공격력 x 공격력 계수 x 스킬 계수 x 속성 데미지 계수)-(캐릭터 방어력 + 장비 방어력 수치) x 장비 방어력 계수
        
        if (Random.Range(0f, 1f) <= state.critChance)
        {
            var calculatedDamage = ((state.damage * state.fatalDamage) + Rockpaperscissors()) - (target.GetComponentInParent<EnemyController>().state.armor);
            take.OnAttack(calculatedDamage);
            Debug.Log(calculatedDamage);
        }
        else
        {
            var calculatedDamage = (state.damage + Rockpaperscissors()) - (target.GetComponentInParent<EnemyController>().state.armor);
            take.OnAttack(calculatedDamage);
            Debug.Log(calculatedDamage);

        }
        if(isHitEffect)
        {
            var ef = ObjectPoolManager.instance.GetGo(effectName);
            Vector3 pos = gameObject.transform.position;
            pos.y += 0.5f;
            pos.z += 0.1f;
            ef.transform.position = pos;
            ef.transform.rotation = gameObject.transform.rotation;
            ef.SetActive(false);
            ef.SetActive(true);
            ef.GetComponent<PoolAble>().ReleaseObject(0.3f);
            
        }
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

        //StartCoroutine(AttackDelay());
    }
    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.2f);
        ani.speed = 0.1f;
        yield return new WaitForSeconds(state.attackDelay);
        ani.speed = 1;
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

    public (Vector3, Tile) OnClickSkillTile()
    {
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
        layerMask = lowTileMask | highTileMask;
        RaycastHit hit;

        // 레이를 쏴서 타일에 맞았을 때, 그 타일이 어태커블 타일이면, 마우스 포지션에 해당 좌표의 vector3 int값 저장
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var rayCast = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
        if (rayCast && Input.GetMouseButton(0))
        {
            mouseClickInfo.Item1 = hit.point;
            Debug.Log("마우스포지션 업데이트 : " + mouseClickInfo.Item1);
            isDragging = true;
            return (mouseClickInfo.Item1, hit.transform.GetComponent<Tile>());
        }

        if(Input.GetMouseButtonUp(0) && attackableSkillTiles.Count != 0)
        {
            isDragging = false;
            switch (skillState.skillType)
            {
                case SkillType.SnipingArea:
                    OnClickUpSkillAreaTile();
                    attackableSkillTiles.Clear();
                    break;
                case SkillType.SnipingSingle:
                    OnClickUpSkillSingleTile();
                    attackableSkillTiles.Clear();
                    break;
            }
        }
        return (mouseClickInfo.Item1, null);
    }

    public void OnClickUpSkillAreaTile()
    {
        if (isSkillPossible)
        {
            Debug.Log("스킬 발동! 부와아아아앙아ㅏㄱ");
            // 스킬 타일들 순회
            foreach (var skillTile in attackableSkillTiles)
            {
                foreach (var obj in skillTile.objectsOnTile)
                {
                    skillState.targetList.Add(obj);
                }
            }
            skillState.UseSkill();
            Time.timeScale = 1.0f;
            stageManager.currentPlayer = null;
            stageManager.ingameStageUIManager.isSkillTileWindow = false;
        }
        else
        {
            Debug.Log("잘못된 타일 선택.");
            stageManager.ingameStageUIManager.ClearTileMesh();
        }
    }

    public void OnClickUpSkillSingleTile()
    {
        if(isSkillPossible)
        {
            Debug.Log("OnClickUpSkillSingleTile");
            Debug.Log("싱글 스킬 발동! 부와아아아앙아ㅏㄱ");
            // 스킬 타일들 순회
            foreach (var skillTile in attackableSkillTiles)
            {
                foreach (var obj in skillTile.objectsOnTile)
                {
                    skillState.targetList.Add(obj);
                }
            }
            skillState.UseSkill();
            Time.timeScale = 1.0f;
            stageManager.currentPlayer = null;
            stageManager.ingameStageUIManager.isSkillTileWindow = false;
        }
        isSkillPossible = false;
    }

    public void AttackableSkillAreaTileSet(Vector3 mousePosition)
    {
        stageManager.ingameStageUIManager.ClearTileMesh();
        prevAttackableSkillTiles.Clear();
        if (attackableSkillTiles.Count > 0)
        {
            attackableSkillTiles.Clear();
        }

        var mousePosInt = Utils.Vector3ToVector3Int(mousePosition);
        skillPivot = Utils.Vector3ToVector3Int(mousePosition);
        List<Vector3Int> skillRange = new List<Vector3Int>();
        var playerPosInt = Utils.Vector3ToVector3Int(transform.position);
        Vector3Int defaultOffset = new Vector3Int();
        Vector3Int skillOffset = mousePosInt - playerPosInt;
        int[,] tempAttackRange = skillState.AttackRange;

        // Check mouse position is in the attackable tiles
        foreach (var attackableTile in attackableTiles)
        {
            if(attackableTile.index == mousePosInt)
            {
                Debug.Log("어택타일 위 : " + mousePosInt);
                isSkillPossible = true;
                break;
            }
            else
            {
                isSkillPossible = false;
            }
        }

        // apply skill attack tile rotation
        var isOffsetXZero = skillOffset.x == 0;
        if (isOffsetXZero && skillOffset.z < 0)
        {
            // 아래로 회전
            tempAttackRange = Utils.RotateArray(skillState.AttackRange, 2);

        }
        else if (!isOffsetXZero)
        {
            if (skillOffset.x > 0)
            {
                // 우회전
                tempAttackRange = Utils.RotateArray(skillState.AttackRange, 1);
            }
            else
            {
                // 좌회전
                tempAttackRange = Utils.RotateArray(skillState.AttackRange, 3);
            }
        }

        //// Get default offsets : offset between player tile and skill tile pivot
        for (int i = 0; i < tempAttackRange.GetLength(0); i++) // row
        {
            for (int j = 0; j < tempAttackRange.GetLength(1); j++) // col
            {
                if (tempAttackRange[i, j] == 1)
                {
                    skillRange.Add(new Vector3Int(i, 0, j));
                }
                else if(tempAttackRange[i, j] == 2)
                {
                    skillRange.Add(new Vector3Int(i, 0, j));
                    //skillPivot = new Vector3Int(i, 0, j);
                    defaultOffset.x = playerPosInt.x - i;
                    defaultOffset.z = playerPosInt.z - j;
                    defaultOffset.y = 0;
                }
            }
        }

        // skill world positions => add to attackable tiles
        RaycastHit hit;
        var lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        var highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
        var layerMask = lowTileMask | highTileMask;
        for (int i = 0; i < skillRange.Count; ++i)
        {
            // skill attack range by world pos
            skillRange[i] += defaultOffset; // offset between player tile and skill tile 2
            skillRange[i] += skillOffset; // offset between player tile and mouse tile

            // skill attack range => attackableSkillTiles applyied tile height(first hit tile)
            
            var tempPos = skillRange[i];
            tempPos.y += 10;
            if (Physics.Raycast(tempPos, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                var tileContoller = hit.transform.GetComponent<Tile>();
                attackableSkillTiles.Add(tileContoller);
                prevAttackableSkillTiles.Add(tileContoller);
            }
        }
        
        // change tile meshes
        if(isSkillPossible)
        {
            stageManager.ingameStageUIManager.ChangeSkillTileMesh();
        }
        else
        {
            stageManager.ingameStageUIManager.ChangeUnActiveTileMesh();
        }
    }

    public void AttackableSkillSingleTileSet(Tile selectedTile)
    {
        Debug.Log("AttackableSkillSingleTileSet" + selectedTile);
        stageManager.ingameStageUIManager.ClearTileMesh();
        prevAttackableSkillTiles.Clear();
        if (attackableSkillTiles.Count > 0)
        {
            attackableSkillTiles.Clear();
        }

        attackableSkillTiles.Add(selectedTile);
        prevAttackableSkillTiles.Add(selectedTile);
        stageManager.ingameStageUIManager.ChangeSkillTileMesh();
        isSkillPossible = true;
    }

    public void OnClickDownCharacter()
    {
        var otherCharacterArrange = stageManager.ingameStageUIManager.windowMode == WindowMode.FirstArrange || stageManager.ingameStageUIManager.windowMode == WindowMode.SecondArrange;
        var otherCharacterSetting = stageManager.ingameStageUIManager.windowMode == WindowMode.Setting;
        var otherCharacterUsingSkill = stageManager.ingameStageUIManager.isSkillTileWindow;

        if(otherCharacterArrange || otherCharacterSetting || otherCharacterUsingSkill)
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
