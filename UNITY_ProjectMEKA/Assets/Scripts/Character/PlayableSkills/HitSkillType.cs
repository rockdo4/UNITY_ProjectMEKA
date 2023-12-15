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
    [SerializeField, Header("����Ʈ �̸�")]
    public string effectName;

    [SerializeField, Header("Ÿ�� ȿ��")]
    public BattingEffect effect;

    [SerializeField, Header("Ÿ���� ���ݷ� ������ �ִ°�")]
    public bool isDamageUp;

    [SerializeField, Header("���� ��� �����ΰ�")]
    public bool isSingle;

    [SerializeField, Header("��ų ���ӽð�")]
    public float skillDuration;

    [SerializeField, Header("���� ���(�ۼ�Ʈ or ����)")]
    public Defines.IncrementalForm inc;

    [SerializeField, Header("�󸶳� ����(�����̸� �״��, �ۼ�Ʈ�� 1�� 100%)")]
    public float figure;

    [SerializeField,Header("���� ������ ���� �����ϴ°�")]
    public bool isAttackRage;

    [SerializeField, Header("��")]//p,e
    public int hang;
    [SerializeField, Header("��")]//p,e
    public int yal;
    [SerializeField, Header("���ݹ�������")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;

    [SerializeField, Header("������ Ÿ�̹� ������ ���� ������")]
    public float delay;
    [SerializeField, Header("���� �ִϸ��̼��� ���� �ִ°�")]
    public bool isAttackAnimation;
    



    public void ConvertTo2DArray()
    {
        // 1���� �迭�� ���̰� ��� ���� ���� ��ġ�ϴ��� Ȯ��
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1���� �迭�� ���̰� ��� ���� ���� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // �� 2���� �迭 ����
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
                            pos.y += 0.5f; // y�� ��ġ ����

                            obj.transform.position = pos;
                            obj.transform.rotation = player.transform.rotation;

                            obj.SetActive(false);
                            obj.SetActive(true);
                            CheckOverlapBoxes();
                        }
                        
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
        pos.y += 0.5f; // y�� ��ġ ����

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
        pos.y += 0.5f; // y�� ��ġ ����

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
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
        // �ݶ��̴� ����Ʈ�� ��ȸ�ϸ� ���� ���� ����
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
