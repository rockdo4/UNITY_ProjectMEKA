using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSkill : SkillBase
{

    private EnemyController enemy;
    private float healthPercent;
    private bool isHyper;
    [SerializeField, Header("��")]//p,e
    public int hangs;
    [SerializeField, Header("��")]//p,e
    public int yals;
    [SerializeField, Header("���ݹ�������")]//p,e
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
        // 1���� �迭�� ���̰� ��� ���� ���� ��ġ�ϴ��� Ȯ��
        if (rangeAttacks.Length != hangs * yals)
        {
            Debug.LogError("1���� �迭�� ���̰� ��� ���� ���� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // �� 2���� �迭 ����
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

                // ���� ��ġ ������Ʈ
                saveGridPos = enemy.CurrentGridPos;
            }
           
        }
        float healthPercentage = enemy.state.Hp / enemy.state.maxHp;

        if (healthPercentage <= 0.25f)
        {
            // ü���� 25% ������ ��
            enemy.state.damage = saveDamage * 1.3f; // 30% ����
        }
        else if (healthPercentage <= 0.5f)
        {
            // ü���� 50% ������ ��
            enemy.state.damage = saveDamage * 1.2f; // 20% ����
        }
        else if (healthPercentage <= 0.75f)
        {
            // ü���� 75% ������ ��
            enemy.state.damage = saveDamage * 1.1f; // 10% ����
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
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        Convert2DArray();

        Vector3 forward = -enemy.transform.forward;
        Vector3 right = enemy.transform.right; 

        int characterRow = 0;
        int characterCol = 0;

        // �÷��̾��� ��ġ�� ã�� ����
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

        // ���� ������ �����ϰ� �ݶ��̴��� �����ϴ� ����
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 1)
                {
                    // �÷��̾� ��ġ�� �������� ������� ��ġ ���
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = enemy.transform.position + relativePosition;

                    // ���� ũ�⸦ ������ ������ ����
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

        Vector3 forward = -enemy.transform.forward; // �÷��̾��� ���� ������
        Vector3 right = enemy.transform.right; // �÷��̾��� ���� ������

        int characterRow = 0;
        int characterCol = 0;

        // �÷��̾��� ��ġ�� ã�� ����
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

        Gizmos.color = Color.red; // ���� ����

        // ���� ���� ������ ��Ÿ���� ���� �׸���
        for (int i = 0; i < AttackRanges.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRanges.GetLength(1); j++)
            {
                if (AttackRanges[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = enemy.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // ���� ũ��

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
