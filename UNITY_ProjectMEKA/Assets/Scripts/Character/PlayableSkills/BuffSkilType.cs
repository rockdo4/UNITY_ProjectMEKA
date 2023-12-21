using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class BuffSkilType : SkillBase
{
    

    public enum buffType
    {
        damage,
        attackSpeed,
        AddCost,
        Armor,
        CriticalChance,
        EnemyArmor,
        Healing,
        ArmorAndCost
    }


    [SerializeField, Header("스킬 지속시간")]
    public float skillDuration;

    [SerializeField, Header("자신에게 쓰는가")]
    public bool isMe;

    [SerializeField, Header("단일인가")]
    public bool isSingle;

    [SerializeField, Header("이팩트 이름은 무엇인가")]
    public string effectName;

    [SerializeField, Header("선택해서 사용하는가")]
    public bool isChoice;

    [SerializeField, Header("어떤 능력치를 변경하는가")]
    public buffType type;

    [SerializeField, Header("증가 방식(퍼센트 or 배율)")]
    public Defines.IncrementalForm Inc;

    [SerializeField, Header("얼마나 증가(배율이면 그대로, 퍼세트면 1이 100%)")]
    public float figure;
    [SerializeField, Header("코스트 초당 몇증가?")]
    public float addCost;

    [SerializeField, Header("공격 범위가 따로 존재하는가")]
    public bool isAttackRage;
    [SerializeField, Header("공격 애니매이션이 존재하는가")]
    public bool isAttackAnimaiton;

    

    private PlayerController player;
    private float timer;
    private float duration;
    private float saveDamage;
    private float saveSpeed;
    private float saveArmor;
    private float saveCriticalChance;
    private GameObject obj;
    private List<Collider> colliders;
    private float tTimer;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillCoolTime;
        duration = 0f;
        isSkillUsing = false;
        saveSpeed = player.state.attackDelay;
        saveDamage = player.state.damage;
        saveArmor = player.state.armor;
        ConvertTo2DArray();
        saveCriticalChance = player.state.critChance;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            if(type == buffType.ArmorAndCost)
            {
                tTimer += Time.deltaTime;
                if(tTimer>=1f)
                {
                    tTimer = 0;
                    var cos = GameObject.FindGameObjectWithTag("StageManager");
                    cos.GetComponent<StageManager>().currentCost += addCost;
                }
            }
            duration += Time.deltaTime;
            if(duration >= skillDuration) 
            {
                duration = 0f;
                isSkillUsing = false;
                player.state.attackDelay = saveSpeed;
                player.ani.speed = 1;
                player.state.damage = saveDamage;
                player.state.armor = saveArmor;
                player.state.critChance = saveCriticalChance;
                if(obj != null)
                {
                    obj.GetComponent<PoolAble>().ReleaseObject();

                }
            }

        }
        if(skillType == Defines.SkillType.Auto && player.currentState != PlayerController.CharacterStates.Arrange)
        {
           
            UseSkill();
            
        }


    }
    public override void UseSkill()
    {
        if(player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch(skillType)
            {
                case Defines.SkillType.Auto:
                    AutoHealingSkill();
                    break;
                case Defines.SkillType.Instant:
                    //��� ����->�ڽ� || �ֺ� �ٸ� ĳ����->� �ɷ�ġ ����-> ����� % ���� -> ����Ʈ ���� -> ����
                    
                    if (isAttackAnimaiton)
                    {
                        player.ani.SetTrigger("Skill");
                    }
                    else
                    {
                        
                        InstantSkill();

                    }
                    break;
                case Defines.SkillType.SnipingSingle:
                    //���õ� ���� �Ѿ�ð�
                    break;
                case Defines.SkillType.SnipingArea:
                    //���õ� ������ ����� �Ѿ�ð�
                    break;
            }
        }
    }
    
    public void AutoHealingSkill()
    {
        player.state.Hp += player.state.maxHp * figure;
        if(player.state.Hp >= player.state.maxHp) 
        {
            player.state.Hp = player.state.maxHp;
        }

        
    }
    public void InstantSkill()
    {
        Debug.Log("���߳�");
        switch(type)
        {
            case buffType.attackSpeed:
                //����
                //�ִϸ��̼� ������
                InstantSkillAttackSpeedBuff();
                break;
            case buffType.damage:
                InstantSkillAttackDamageBuff();
                //���ݷ¸�����
                break;
            case buffType.AddCost:
                InstantSkillAddCost();
                break;
            case buffType.CriticalChance:
                InstantSkillAddCritical();
                break;
            case buffType.EnemyArmor:
                InstantSkillDeductEnemyArmor();
                break;
            case buffType.ArmorAndCost:
                InstantSkillArmorAndCost();
                break;
        }
        
    }
    public void InstantSkillArmorAndCost()
    {
        player.state.armor += saveArmor + figure;
    }
    public void InstantSkillDeductEnemyArmor()
    {
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        ConvertTo2DArray();
        PoolBuffEffact();
        // �÷��̾��� ���� ������ �� ���� ������ ������ ����
        Vector3 forward = -player.transform.forward; // �÷��̾��� ���� ������
        Vector3 right = player.transform.right; // �÷��̾��� ���� ������

        int characterRow = 0;
        int characterCol = 0;

        // �÷��̾��� ��ġ�� ã�� ����
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                    break;
                }
            }
        }

        // ���� ������ �����ϰ� �ݶ��̴��� �����ϴ� ����
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    // �÷��̾� ��ġ�� �������� ������� ��ġ ���
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    // ���� ũ�⸦ ������ ������ ����
                    Vector3 boxSize = new Vector3(1, 5, 1);
                    Collider[] hitColliders = Physics.OverlapBox(correctedPosition, boxSize / 2, Quaternion.identity);

                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.CompareTag("EnemyCollider") && !colliders.Contains(hitCollider))
                        {
                            var en = hitCollider.GetComponentInParent<EnemyController>();
                            en.state.armor -= en.state.armor * figure;
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
    }
    public void InstantSkillAddCritical()
    {
        PoolBuffEffact();
        player.state.critChance += figure;
    }
    public void InstantSkillAddCost()
    {

        PoolBuffEffactAll();
        var cos = GameObject.FindGameObjectWithTag("StageManager");
        cos.GetComponent<StageManager>().currentCost += player.rangeInPlayers.Count;
    }

    public void InstantSkillAttackSpeedBuff()
    {
        if(isMe)
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    player.state.attackDelay -= saveSpeed * figure;
                    player.ani.speed += 1 * figure;
                    PoolBuffEffact();
                    break;
                case Defines.IncrementalForm.Magnification:
                    player.state.attackDelay *= figure;
                    player.ani.speed = figure;
                    PoolBuffEffact();
                    break;
            }
        }
        else
        {

        }
    }
    public void InstantSkillAttackDamageBuff()
    {
        if (isMe)
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    player.state.damage += saveDamage * figure;
                    PoolBuffEffact();
                    break;
                case Defines.IncrementalForm.Magnification:
                    player.state.damage *= figure;
                    PoolBuffEffact();
                    break;
            }
        }
        else
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    player.state.damage += saveDamage * figure;
                    PoolBuffEffactAll();
                    break;
                case Defines.IncrementalForm.Magnification:
                    player.state.damage *= figure;
                    PoolBuffEffactAll();
                    break;
            }
        }
    }
    public void PoolBuffEffact()
    {
        obj = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pos = gameObject.transform.position;
        pos.y += 0.5f;
        obj.transform.position = pos;

        obj.SetActive(false);
        obj.SetActive(true);
    }
    public void PoolBuffEffactAll()
    {
        var obs = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pobs = gameObject.transform.position;
        pobs.y += 0.5f;
        obs.transform.position = pobs;

        obs.GetComponent<PoolAble>().ReleaseObject(3f);

        obs.SetActive(false);
        obs.SetActive(true);
        foreach (var a in player.rangeInPlayers)
        {
            var oh = ObjectPoolManager.instance.GetGo(effectName);

            Vector3 pos = a.GetComponentInParent<PlayerController>().gameObject.transform.position;
            pos.y += 0.5f;
            oh.transform.position = pos;

            oh.GetComponent<PoolAble>().ReleaseObject(3f);

            oh.SetActive(false);
            oh.SetActive(true);
        }
    }
    public void BuffSkill()
    {
        CheckOverlapBoxes();

    }
    void CheckOverlapBoxes()
    {

        //if (player == null || AttackRange == null || transform == null)
        //{
        //    return;
        //}
        //colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        //ConvertTo2DArray();

        //// �÷��̾��� ���� ������ �� ���� ������ ������ ����
        //Vector3 forward = -player.transform.forward; // �÷��̾��� ���� ������
        //Vector3 right = player.transform.right; // �÷��̾��� ���� ������

        //int characterRow = 0;
        //int characterCol = 0;

        //// �÷��̾��� ��ġ�� ã�� ����
        //for (int i = 0; i < AttackRange.GetLength(0); i++)
        //{
        //    for (int j = 0; j < AttackRange.GetLength(1); j++)
        //    {
        //        if (AttackRange[i, j] == 2)
        //        {
        //            characterRow = i;
        //            characterCol = j;
        //            break;
        //        }
        //    }
        //}

        //// ���� ������ �����ϰ� �ݶ��̴��� �����ϴ� ����
        //for (int i = 0; i < AttackRange.GetLength(0); i++)
        //{
        //    for (int j = 0; j < AttackRange.GetLength(1); j++)
        //    {
        //        if (AttackRange[i, j] == 1)
        //        {
        //            // �÷��̾� ��ġ�� �������� ������� ��ġ ���
        //            Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
        //            Vector3 correctedPosition = player.transform.position + relativePosition;

        //            // ���� ũ�⸦ ������ ������ ����
        //            Vector3 boxSize = new Vector3(1, 5, 1);
        //            Collider[] hitColliders = Physics.OverlapBox(correctedPosition, boxSize / 2, Quaternion.identity);

        //            foreach (var hitCollider in hitColliders)
        //            {
        //                if (hitCollider.CompareTag("PlayerCollider") && !colliders.Contains(hitCollider))
        //                {
        //                    colliders.Add(hitCollider);
        //                }
        //            }
        //        }
        //    }
        //}
        //// �ݶ��̴� ����Ʈ�� ��ȸ�ϸ� ���� ���� ����
        if (player == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
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
        Vector3 characterPosition = player.transform.position;
        Vector3 forward = -player.transform.forward; // 캐릭터가 바라보는 반대 방향
        Vector3 right = player.transform.right; // 캐릭터의 오른쪽 방향

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
        foreach(var a in attackableTiles)
        {
            foreach(var b in a.objectsOnTile)
            {
                if(b.tag == "Player")
                {
                    switch (Inc)
                    {
                        case Defines.IncrementalForm.Percentage:
                            var pl = b.GetComponent<PlayerController>();
                            pl.state.armor += saveArmor * figure;
                            var obs = ObjectPoolManager.instance.GetGo(effectName);

                            Vector3 pobs = pl.gameObject.transform.position;
                            pobs.y += 0.5f;
                            obs.transform.position = pobs;

                            obs.GetComponent<PoolAble>().ReleaseObject(skillDuration);

                            obs.SetActive(false);
                            obs.SetActive(true);
                            break;
                        case Defines.IncrementalForm.Magnification:

                            var player = b.GetComponent<PlayerController>();
                            player.state.armor = saveArmor * figure;
                            var obsa = ObjectPoolManager.instance.GetGo(effectName);

                            Vector3 pobss = player.gameObject.transform.position;
                            pobss.y += 0.5f;
                            obsa.transform.position = pobss;

                            obsa.GetComponent<PoolAble>().ReleaseObject(skillDuration);

                            obsa.SetActive(false);
                            obsa.SetActive(true);
                            break;
                    }
                }
            }
        }
        //foreach (var hitCollider in colliders)
        //{
        //    switch (Inc)
        //    {
        //        case Defines.IncrementalForm.Percentage:
        //            var pl = hitCollider.GetComponentInParent<PlayerController>();
        //            pl.state.armor += saveArmor * figure;
        //            var obs = ObjectPoolManager.instance.GetGo(effectName);

        //            Vector3 pobs = pl.gameObject.transform.position;
        //            pobs.y += 0.5f;
        //            obs.transform.position = pobs;

        //            obs.GetComponent<PoolAble>().ReleaseObject(skillDuration);

        //            obs.SetActive(false);
        //            obs.SetActive(true);
        //            break;
        //        case Defines.IncrementalForm.Magnification:
                    
        //            var player = hitCollider.GetComponentInParent<PlayerController>();
        //            player.state.armor = saveArmor * figure;
        //            var obsa = ObjectPoolManager.instance.GetGo(effectName);

        //            Vector3 pobss = player.gameObject.transform.position;
        //            pobss.y += 0.5f;
        //            obsa.transform.position = pobss;

        //            obsa.GetComponent<PoolAble>().ReleaseObject(skillDuration);

        //            obsa.SetActive(false);
        //            obsa.SetActive(true);
        //            break;
        //    }
            
        //}

    }
    
}
