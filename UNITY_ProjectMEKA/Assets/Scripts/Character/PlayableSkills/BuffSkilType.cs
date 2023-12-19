using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkilType : SkillBase
{
    

    public enum buffType
    {
        damage,
        attackSpeed,
        AddCost,
        Armor,
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

    [SerializeField, Header("공격 범위가 따로 존재하는가")]
    public bool isAttackRage;

    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;

    private PlayerController player;
    private float timer;
    private float duration;
    private float saveDamage;
    private float saveSpeed;
    private float saveArmor;
    private GameObject obj;
    private List<Collider> colliders;
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
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration) 
            {
                duration = 0f;
                //isSkillUsing = false;
                player.state.attackDelay = saveSpeed;
                player.ani.speed = 1;
                player.state.damage = saveDamage;
                player.state.armor = saveArmor;
                if(obj != null)
                {
                    obj.GetComponent<PoolAble>().ReleaseObject();

                }
            }

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

                    break;
                case Defines.SkillType.Instant:
                    //즉발 시전->자신 || 주변 다른 캐릭터->어떤 능력치 수정-> 계산방법 % 배율 -> 이팩트 생성 -> 적용
                    
                    if (isAttackRage)
                    {
                        player.ani.SetTrigger("Skill");
                    }
                    else
                    {
                        
                        InstantSkill();

                    }
                    break;
                case Defines.SkillType.SnipingSingle:
                    //선택된 놈이 넘어올것
                    break;
                case Defines.SkillType.SnipingArea:
                    //선택된 영역의 놈들이 넘어올것
                    break;
            }
        }
    }
    public void InstantSkill()
    {
        Debug.Log("다했네");
        switch(type)
        {
            case buffType.attackSpeed:
                //공속
                //애니매이션 빠르게
                InstantSkillAttackSpeedBuff();
                break;
            case buffType.damage:
                InstantSkillAttackDamageBuff();
                //공격력만증가
                break;
            case buffType.AddCost:
                InstantSkillAddCost();
                break;
        }
        
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

        if (player == null || AttackRange == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
        ConvertTo2DArray();

        // 플레이어의 로컬 포워드 및 로컬 오른쪽 방향을 설정
        Vector3 forward = -player.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = player.transform.right; // 플레이어의 로컬 오른쪽

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
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

        // 상자 영역을 생성하고 콜라이더를 검출하는 루프
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    // 플레이어 위치를 기준으로 상대적인 위치 계산
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    // 상자 크기를 고정된 값으로 설정
                    Vector3 boxSize = new Vector3(1, 5, 1);
                    Collider[] hitColliders = Physics.OverlapBox(correctedPosition, boxSize / 2, Quaternion.identity);

                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.CompareTag("PlayerCollider") && !colliders.Contains(hitCollider))
                        {
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
        // 콜라이더 리스트를 순회하며 공격 로직 수행
        foreach (var hitCollider in colliders)
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    var pl = hitCollider.GetComponentInParent<PlayerController>();
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
                    
                    var player = hitCollider.GetComponentInParent<PlayerController>();
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
    public void ConvertTo2DArray()
    {
        // 1차원 배열의 길이가 행과 열의 곱과 일치하는지 확인
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        // 새 2차원 배열 생성
        AttackRange = new int[hang, yal];

        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

    }
    void OnDrawGizmos()
    {

        if(!isAttackRage)
        {
            return;
        }
        ConvertTo2DArray();

        Vector3 forward = -player.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = player.transform.right; // 플레이어의 로컬 오른쪽

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
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

        Gizmos.color = Color.red; // 색상 설정

        // 공격 가능 영역을 나타내는 상자 그리기
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // 상자 크기

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
