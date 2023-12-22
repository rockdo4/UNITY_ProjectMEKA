using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSkill : SkillBase
{

    private EnemyController enemy;
    private float healthPercent;
    private bool isHyper;
    [SerializeField, Header("행")]//p,e
    public int hangs;
    [SerializeField, Header("열")]//p,e
    public int yals;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttacks;
    [HideInInspector]
    public int[,] AttackRanges;
    private List<Collider> colliders = new List<Collider>();
    private Vector3Int saveGridPos;
    private GameObject obj;
    private float timer;
    private bool isStun = false;
    private bool isOne = false;
    private float saveDamage;
    public void Convert2DArray()
    {
        // 1차원 배열의 길이가 행과 열의 곱과 일치하는지 확인
        if (rangeAttacks.Length != hangs * yals)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        // 새 2차원 배열 생성
        AttackRanges = new int[hangs, yals];

        for (int i = 0; i < hangs; i++)
        {
            for (int j = 0; j < yals; j++)
            {
                AttackRanges[i, j] = rangeAttacks[i * yals + j];
            }
        }

    }
    private void Start()
    {
        enemy = GetComponent<EnemyController>();
        isHyper = false;
        saveGridPos = enemy.CurrentGridPos;
        saveDamage = enemy.state.damage;
    }
    private void OnEnable()
    {
        timer = 0;
        isStun = false;
        isOne = false;
        //enemy.state.damage = saveDamage;
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1f && !isStun)
        {
            isStun = true;
            enemy.stunTime = 5f;
            enemy.isMove = true;
            enemy.SetState(NPCStates.Stun);
        }

        healthPercent = (enemy.state.Hp / enemy.state.maxHp) * 100f;
        if(healthPercent <= 50f && !isOne)
        {
            isOne = true;
            enemy.SetState(NPCStates.Stun);
        }
        if(enemy.bossAttackCount >= 20)
        {
            enemy.bossAttackCount = 0;
            isHyper = true;
            if(obj == null)
            {
                obj = ObjectPoolManager.instance.GetGo("BossHyperEffect");
                obj.transform.position = gameObject.transform.position;
                obj.GetComponent<FallowTarget>().target = gameObject.transform;
                obj.SetActive(false);
                obj.SetActive(true);
            }
        }
        if(isHyper)
        {
            if (enemy.CurrentGridPos != saveGridPos)
            {
                UseSkill();

                // 이전 위치 업데이트
                saveGridPos = enemy.CurrentGridPos;
            }
           
        }
        float healthPercentage = enemy.state.Hp / enemy.state.maxHp;

        if (healthPercentage <= 0.25f)
        {
            // 체력이 25% 이하일 때
            enemy.state.damage = saveDamage * 1.3f; // 30% 증가
        }
        else if (healthPercentage <= 0.5f)
        {
            // 체력이 50% 이하일 때
            enemy.state.damage = saveDamage * 1.2f; // 20% 증가
        }
        else if (healthPercentage <= 0.75f)
        {
            // 체력이 75% 이하일 때
            enemy.state.damage = saveDamage * 1.1f; // 10% 증가
        }
        
    }
    public override void UseSkill()
    {
        CheckOverlapBoxes();
    }
    public void HyperAttack()
    {
        isHyper = false;
        enemy.ani.SetTrigger("Skill");
        obj.GetComponent<PoolAble>().ReleaseObject(1f);
    }
    public void BossSkill()
    {
        var sk = ObjectPoolManager.instance.GetGo("BossSkillEffect");
        sk.transform.position = gameObject.transform.position;
        sk.SetActive(false);
        sk.SetActive(true);
    }
    void CheckOverlapBoxes()
    {
        if (enemy == null || AttackRanges == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
        Convert2DArray();

        Vector3 forward = -enemy.transform.forward;
        Vector3 right = enemy.transform.right; 

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                    break;
                }
            }
        }

        // 상자 영역을 생성하고 콜라이더를 검출하는 루프
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 1)
                {
                    // 플레이어 위치를 기준으로 상대적인 위치 계산
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = enemy.transform.position + relativePosition;

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
       if(colliders.Count > 0 && colliders != null)
        {
            
            HyperAttack();
        }

    }
    void OnDrawGizmos()
    {
        if(AttackRanges == null || this == null) return;

        Convert2DArray();

        Vector3 forward = -enemy.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = enemy.transform.right; // 플레이어의 로컬 오른쪽

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                    break;
                }
            }
        }

        Gizmos.color = Color.red; // 색상 설정

        // 공격 가능 영역을 나타내는 상자 그리기
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = enemy.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // 상자 크기

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
