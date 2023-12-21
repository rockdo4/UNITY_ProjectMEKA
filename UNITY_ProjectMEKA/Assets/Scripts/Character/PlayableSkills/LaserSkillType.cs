using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkillType : SkillBase
{
    [SerializeField, Header("������ �����ֱ�")]
    public float skillDuration;
    [SerializeField, Header("����Ʈ �̸�")]
    public string effectName;
    [SerializeField, Header("���ۼ�Ʈ ������ 1�� 100%)")]
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

        if (player == null || AttackRange == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        ConvertTo2DArray();

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
                            hitCollider.GetComponentInParent<IAttackable>().OnAttack(player.state.damage * figure);
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
        

    }
    void OnDrawGizmos()
    {


        ConvertTo2DArray();

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

        Gizmos.color = Color.red; // ���� ����

        // ���� ���� ������ ��Ÿ���� ���� �׸���
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // ���� ũ��

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
