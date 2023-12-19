using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSkill : SkillBase
{

    private EnemyController enemy;
    private float healthPercent;
    private bool isHyper;
    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;
    private List<Collider> colliders = new List<Collider>();
    private Vector3Int saveGridPos;
    private GameObject obj;
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


    private void Start()
    {
        enemy = GetComponent<EnemyController>();
        isHyper = false;
        saveGridPos = enemy.CurrentGridPos;
    }
    private void Update()
    {
        healthPercent = (enemy.state.Hp / enemy.state.maxHp) * 100f;
        if(healthPercent <= 50f)
        {

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
                // 타일을 이동했음을 감지하고 CheckOverlapBoxes 메서드 호출
                UseSkill();

                // 이전 위치 업데이트
                saveGridPos = enemy.CurrentGridPos;
            }
           
        }
        //if (enemy.CurrentGridPos != saveGridPos)
        //{
        //    // 타일을 이동했음을 감지하고 CheckOverlapBoxes 메서드 호출
        //    UseSkill();

        //    // 이전 위치 업데이트
        //    saveGridPos = enemy.CurrentGridPos;
        //}
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
        if (enemy == null || AttackRange == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
        ConvertTo2DArray();

        Vector3 forward = -enemy.transform.forward;
        Vector3 right = enemy.transform.right; 

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
            //enemy.ani.SetTrigger();
            //Debug.Log("주모 여기 스킬!");
            HyperAttack();
        }

    }
    void OnDrawGizmos()
    {


        ConvertTo2DArray();

        Vector3 forward = -enemy.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = enemy.transform.right; // 플레이어의 로컬 오른쪽

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
                    Vector3 correctedPosition = enemy.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // 상자 크기

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
