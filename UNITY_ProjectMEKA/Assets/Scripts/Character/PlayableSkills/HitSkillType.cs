using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitSkillType : SkillBase
{
    public enum BattingEffect
    {
        Stun,
        Damage,
        DamageAndAttackDamageUp,

    }
    [SerializeField, Header("이팩트 이름")]
    public string effectName;

    [SerializeField, Header("타격 효과")]
    public BattingEffect effect;

    [SerializeField, Header("타격후 공격력 증가가 있는가")]
    public bool isDamageUp;

    [SerializeField, Header("단일 대상 공격인가")]
    public bool isSingle;

    [SerializeField, Header("스킬 지속시간")]
    public float skillDuration;

    [SerializeField, Header("증가 방식(퍼센트 or 배율)")]
    public Defines.IncrementalForm inc;

    [SerializeField, Header("얼마나 증가(배율이면 그대로, 퍼세트면 1이 100%)")]
    public float figure;

    [SerializeField,Header("공격 범위가 따로 존재하는가")]
    public bool isAttackRage;

    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;

    [SerializeField, Header("데미지 타이밍 프레임 시작 몇초후")]
    public float delay;
    [SerializeField, Header("공격 애니매이션이 따로 있는가")]
    public bool isAttackAnimation;
    



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

    private float saveDamage;
    private float saveAttackDelay;
    private float timer;
    private float duration;
    private PlayerController player;
    private List<Collider> colliders;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        saveDamage = player.state.damage;
        saveAttackDelay = player.state.attackDelay;
        isSkillUsing = false;
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
                player.state.damage = saveDamage;
                player.state.attackDelay = saveAttackDelay;
            }
        }
    }
    public override void UseSkill()
    {
        if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.Auto:
                    
                    break;
                case Defines.SkillType.Instant:
                    player.ani.SetTrigger("Skill");
                    if(isDamageUp)
                    {
                        DamageUp();
                    }
                    if (isAttackRage )
                    {
                        if(!isAttackAnimation)
                        {
                            var obj = ObjectPoolManager.instance.GetGo(effectName);
                            Debug.Log(obj);
                            Vector3 pos = player.transform.position/* + player.transform.forward * 1f*/;
                            pos.y += 0.5f; // y축 위치 조정

                            obj.transform.position = pos;
                            obj.transform.rotation = player.transform.rotation;

                            obj.SetActive(false);
                            obj.SetActive(true);
                            CheckOverlapBoxes();
                        }
                        
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
    public void DamageUp()
    {
        switch(inc)
        {
            case Defines.IncrementalForm.Magnification:
                player.state.damage *= figure;
                break;
            case Defines.IncrementalForm.Percentage:
                player.state.damage = saveDamage + (saveDamage * figure);
                break;
        }
        
    }
    public void OnlyDamage()
    {
        var obj = ObjectPoolManager.instance.GetGo(effectName);
        Vector3 pos = player.transform.position + player.transform.forward * 1f;
        pos.y += 0.5f; // y축 위치 조정

        obj.transform.position = pos;
        obj.transform.rotation = player.transform.rotation;

        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(2f);
        if(isAttackRage)
        {
            CheckOverlapBoxes();
        }
        else
        {
            foreach (var p in player.rangeInEnemys)
            {
                p.GetComponentInParent<IAttackable>().OnAttack(saveDamage * figure);
            }
        }
        
    }
    public void Stunnig()
    {
        var obj = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pos = player.transform.position + player.transform.forward * 1f;
        pos.y += 0.5f; // y축 위치 조정

        obj.transform.position = pos;
        obj.transform.rotation = player.transform.rotation;
        
        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(2f);

        switch(effect)
        {
            case BattingEffect.Damage:

                break;
            case BattingEffect.Stun:
                StunAttack();
                break;
        }

        foreach (var p in player.rangeInEnemys)
        {
            p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
        }

    }
    public void PalaSkillAttack()
    {
        var t = player.target.GetComponentInParent<IAttackable>();
        t.OnAttack(saveDamage * 1.2f);
    }
    public void StunAttack()
    {
        if(isSingle)
        {
            foreach (var p in player.rangeInEnemys)
            {
                p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
                return;
            }
        }
        else
        {
            foreach (var p in player.rangeInEnemys)
            {
                p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
            }
        }
       
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
                        if (hitCollider.CompareTag("EnemyCollider") && !colliders.Contains(hitCollider))
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
            IAttackable attackable = hitCollider.GetComponentInParent<IAttackable>();
            if (attackable != null)
            {
                switch (inc)
                {
                    case Defines.IncrementalForm.Percentage:
                        attackable.OnAttack(saveDamage + (saveDamage * figure));
                        break;
                    case Defines.IncrementalForm.Magnification:
                        attackable.OnAttack(saveDamage * figure);

                        break;
                }

            }
        }

    }
    void OnDrawGizmos()
    {
        

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
