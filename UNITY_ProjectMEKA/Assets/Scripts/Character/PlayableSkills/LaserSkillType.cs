using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class LaserSkillType : SkillBase
{
    [SerializeField, Header("레이저 생명주기")]
    public float skillDuration;
    [SerializeField, Header("이팩트 이름")]
    public string effectName;
    [SerializeField, Header("몇퍼센트 데미지 1이 100%)")]
    public float figure;

    private PlayerController player;
    private float timer;
    private float duration;
    private GameObject obj;
    private List<Collider> colliders;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkillUsing = false;
        timer = skillCoolTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration)
            {
                duration = 0;
                isSkillUsing = false;
                player.ani.speed = 1;
                obj.GetComponent<PoolAble>().ReleaseObject();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }
    }

    public override void UseSkill()
    {
        if (player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.Auto:

                    break;
                case Defines.SkillType.Instant:
                    InstantSkill();
                    break;
                case Defines.SkillType.SnipingSingle:

                    break;
                case Defines.SkillType.SnipingArea:

                    break;
            }
        }
    }
    public void InstantSkill()
    {
        player.ani.SetTrigger("Skill");
        
    }
    public void LaserBeamPoolEffect()
    {
        player.ani.speed = 0;
        obj = ObjectPoolManager.instance.GetGo(effectName);
        var laser = obj.GetComponent<Laser>();
        laser.player = player;
        obj.SetActive(false);
        obj.SetActive(true);
        CheckOverlapBoxes();
    }
    void CheckOverlapBoxes()
    {

        if (player == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
        ConvertTo2DArray();
        
        //Vector3 front = player.transform.forward;
        Vector3 front = player.firstLookPos.forward;
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
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("앞쪽");
        }
        else if (maxDot == Vector3.Dot(front, south))
        {
            // 남쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("뒤쪽");
        }
        else if (maxDot == Vector3.Dot(front, east))
        {
            // 동쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("오른쪽");
        }
        else if (maxDot == Vector3.Dot(front, west))
        {
            // 서쪽을 보고 있음
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("왼쪽");
        }

        // 타일 레이어 마스크를 설정하기 위한 변수 초기화
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);

        layerMask = lowTileMask | highTileMask;
        // 캐릭터의 현재 위치와 방향을 계산
        Vector3 characterPosition = player.firstLookPos.position;
        Vector3 forward = -player.firstLookPos.forward; // 캐릭터가 바라보는 반대 방향
        Vector3 right = player.firstLookPos.right; // 캐릭터의 오른쪽 방향

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
                        
                    }
                }
            }
        }

        player.SetState(PlayerController.CharacterStates.Idle);
    }
    
}
