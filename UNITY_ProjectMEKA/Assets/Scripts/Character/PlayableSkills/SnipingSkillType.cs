using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Defines;
using System;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class SnipingSkillType : SkillBase
{

    public enum SkillType
    {
        Attack,
        PlayerDamageUp,
        Shield,
    }

    private PlayerController player;
    [SerializeField, Header("몇퍼센트 데미지")]
    public float figure;
    [SerializeField, Header("사용할 이팩트 이름")]
    public string effectName;
    [SerializeField, Header("스킬 지속시간")]
    public float duration;
    
    [SerializeField, Header("어떤 스킬(공격, 버프 등등")]
    public SkillType skillT;
    private bool isSniping;
    private float timer;
    private Vector3Int skillPivot;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillCoolTime;
        ConvertTo2DArray();
    }
    private void OnEnable()
    {
        isSniping = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        //player.SetState(PlayerController.CharacterStates.Arrange);//testcode
    }
    public override void UseSkill()
    {
        
        switch (skillType)
        {
            case Defines.SkillType.SnipingSingle:
                SnipingSingle();
                break;
            case Defines.SkillType.SnipingArea:
                SnipingArea();
                break;

        }
        
    }
    public void SnipingArea()
    {
        if (targetList.Any())
        {
            if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
            {
                timer = 0;
                player.state.cost -= skillCost;
                isSkillUsing = true;
                switch (skillT)
                {
                    case SkillType.Attack:
                        AreaSnipingAttack();
                        break;
                    case SkillType.PlayerDamageUp:
                        break;
                    case SkillType.Shield:
                        break;
                }
            }
        }
    }
    public void AreaSnipingAttack()
    {
        attackableTiles = new List<Tile>(player.attackableSkillTiles);
        StartCoroutine(Damage5Second(attackableTiles));
        var obj = ObjectPoolManager.instance.GetGo(effectName);
        var pos = player.skillPivot;
        obj.transform.position = pos;
        obj.SetActive(false);
        obj.SetActive(true);
        
    }
    IEnumerator Damage5Second(List<Tile> tileList)
    {
        int i = 0;
        while(i < 5) 
        {
            
            CheckOverlapBoxes();
            i++;
            yield return new WaitForSeconds(1f);
            //yield return null;
        }
    }
    public void SnipingSingle()
    {
        if (targetList.Any())
        {
            if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
            {
                timer = 0;
                player.state.cost -= skillCost;
                isSkillUsing = true;
                switch(skillT)
                {
                    case SkillType.Attack:
                        SingleSnipingAttack();
                        break;
                    case SkillType.PlayerDamageUp:
                        SingleSnipingPlayerDamageUp();
                        break;
                    case SkillType.Shield:
                        SingleSnipingPlayerShield();
                        break;
                }
                
            }
        }
    }
    
    public void ShieldSkill()
    {
        foreach (var a in targetList)
        {
            if (a.tag == "Player")
            {
                var c = a.GetComponent<PlayerController>();
                c.state.shield = figure;
                var obj = ObjectPoolManager.instance.GetGo(effectName);
                obj.transform.position = c.gameObject.transform.position;
                obj.GetComponent<Shield>().player = c;
                obj.SetActive(false);
                obj.SetActive(true);
                isSkillUsing = false;
            }
        }
    }
    public void SingleSnipingPlayerShield()
    {
        player.ani.SetTrigger("Skill");
    }
    public void SingleSnipingPlayerDamageUp()
    {
        foreach(var a in targetList)
        {
            if(a.tag == "Player")
            {
                var pl = a.GetComponent<PlayerController>();
                StartCoroutine(UpState(pl.state.damage, pl));
                pl.state.damage *= figure;
                var ob = ObjectPoolManager.instance.GetGo(effectName);
                ob.transform.position = pl.gameObject.transform.position;
                ob.SetActive(false);
                ob.SetActive(true);
                ob.GetComponent<PoolAble>().ReleaseObject(duration);
                return;
            }
        }
    }
    IEnumerator UpState(float originState,PlayerController player)
    {
        yield return new WaitForSeconds(duration);
        player.state.damage = originState;
        isSkillUsing = false;
    }
    public void SingleSnipingAttack()
    {
        var s = FindObjectWithHighestHpRatio(targetList);
        var e = s.GetComponent<EnemyController>();
        var obj = ObjectPoolManager.instance.GetGo(effectName);
        obj.GetComponent<FallowTarget>().target = e.gameObject.transform;
        StartCoroutine(AttackDelay(s));
        Vector3 pos = s.gameObject.transform.position;
        pos.y += 0.5f;
        obj.transform.position = pos;
        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(3f);
    }
    IEnumerator AttackDelay(GameObject s)
    {
        yield return new WaitForSeconds(2f);
        s.GetComponent<IAttackable>().OnAttack(player.state.damage * figure);
        targetList.Clear();
        isSkillUsing = false;
    }
    
    GameObject FindObjectWithHighestHpRatio(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }

        GameObject highestHpRatioObject = null;
        float highestHpRatio = 0;

        foreach (var obj in list)
        {
            if (obj.tag == "Enemy")
            {
                EnemyController health = obj.GetComponent<EnemyController>();
                if (health != null)
                {
                    if (highestHpRatioObject == null)
                    {
                        highestHpRatioObject = obj;
                        highestHpRatio = GetHpRatio(health);
                    }
                    else
                    {
                        float hpRatio = GetHpRatio(health);
                        if (hpRatio > highestHpRatio)
                        {
                            highestHpRatio = hpRatio;
                            highestHpRatioObject = obj;
                        }
                    }
                }
            }
        }

        return highestHpRatioObject;
    }

    float GetHpRatio(EnemyController health)
    {
        if (health.state.maxHp > 0)
            return (float)health.state.Hp / health.state.maxHp;
        return 0;
    }
    void CheckOverlapBoxes()
    {

        if (player == null || transform == null)
        {
            return;
        }
        ConvertTo2DArray();

        Vector3 front = player.transform.forward;
        front.y = 0; // 수평 방향만 고려하기 위해 y 축 컴포넌트를 0으로 설정

        // 정규화하여 단위 벡터로 만듦
        front.Normalize();

        // 4개의 주요 방향을 나타내는 벡터
        Vector3 north = Vector3.forward;
        Vector3 south = Vector3.back;
        Vector3 east = Vector3.right;
        Vector3 west = Vector3.left;

        // 가장 가까운 방향을 찾기 위해 내적을 사용
        float maxDot = Mathf.Max(Vector3.Dot(front, north), Vector3.Dot(front, south),
                                 Vector3.Dot(front, east), Vector3.Dot(front, west));

        // 스위치문으로 각 방향을 판단
        if (maxDot == Vector3.Dot(front, north))
        {
            // 북쪽을 보고 있음
            attackRangeRot = AttackRange;
        }
        else if (maxDot == Vector3.Dot(front, south))
        {
            // 남쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 2);
        }
        else if (maxDot == Vector3.Dot(front, east))
        {
            // 동쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 1);
        }
        else if (maxDot == Vector3.Dot(front, west))
        {
            // 서쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
        }

        // 타일 레이어 마스크를 설정하기 위한 변수 초기화
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);

        layerMask = lowTileMask | highTileMask;
        // 캐릭터의 현재 위치와 방향을 계산
        //Vector3 characterPosition = player.transform.position;
        Vector3 characterPosition = player.skillPivot;
        //Vector3 forward = -player.transform.forward; 
        Vector3 forward = -player.skillPivot; // 캐릭터가 바라보는 반대 방향
        //Vector3 right = player.transform.right; 
        Vector3 right = player.skillPivot; // 캐릭터의 오른쪽 방향

        // 캐릭터의 현재 타일 위치 (행과 열)를 찾기 위한 변수 초기화
        int characterRow = 0;
        int characterCol = 0;

        // state.AttackRange 배열을 순회하여 캐릭터의 위치를 찾음
        for (int i = 0; i < attackRangeRot.GetLength(0); i++)
        {
            for (int j = 0; j < attackRangeRot.GetLength(1); j++)
            {
                if (attackRangeRot[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        // 공격 가능한 타일 리스트를 초기화
        if (attackableTiles.Count > 0)
        {
            attackableTiles.Clear();
        }

        // 공격 가능한 타일을 찾기 위해 state.AttackRange 배열을 다시 순회
        for (int i = 0; i < attackRangeRot.GetLength(0); i++)
        {
            for (int j = 0; j < attackRangeRot.GetLength(1); j++)
            {
                if (attackRangeRot[i, j] == 1) // '1'은 공격 가능한 타일을 나타냄
                {
                    // 캐릭터로부터 상대적인 위치 계산
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition; // 절대 타일 위치 계산

                    // 타일 위치를 정수형으로 변환
                    var tilePosInt = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);

                    // 레이캐스트를 사용하여 타일 검사
                    RaycastHit hit;
                    var tempPos = new Vector3(tilePosInt.x, tilePosInt.y - 10f, tilePosInt.z);
                    if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
                    {
                        // 레이캐스트가 타일에 닿으면 해당 타일 컨트롤러를 가져옴
                        var tileContoller = hit.transform.GetComponent<Tile>();
                        if (!tileContoller.isSomthingOnTile) // 타일 위에 다른 것이 없으면
                        {
                            attackableTiles.Add(tileContoller); // 공격 가능한 타일 리스트에 추가

                        }
                    }
                }
            }
        }

        foreach (var a in attackableTiles)
        {
            foreach (var b in a.objectsOnTile)
            {
                if (b.tag == "Enemy")
                {
                    IAttackable attackable = b.GetComponent<IAttackable>();
                    if (attackable != null)
                    {
                        attackable.OnAttack(player.state.damage * figure);
                        //attackable.OnAttack(100f);
                     
                    }
                }
            }
        }
    }
}
