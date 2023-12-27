using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;

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

   

    [SerializeField, Header("������ Ÿ�̹� ������ ���� ������")]
    public float delay;
    [SerializeField, Header("���� �ִϸ��̼��� ���� �ִ°�")]
    public bool isAttackAnimation;
    
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
        currentSkillTimer = timer; 
        if (isSkillUsing)
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
            var en = p.GetComponentInParent<EnemyController>();
            en.stunTime = 3f;
            en.SetState(NPCStates.Stun);
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
        
        if (player == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        ConvertTo2DArray();

        Vector3 front = player.transform.forward;
        front.y = 0; // ���� ���⸸ ����ϱ� ���� y �� ������Ʈ�� 0���� ����

        // ����ȭ�Ͽ� ���� ���ͷ� ����
        front.Normalize();

        // 4���� �ֿ� ������ ��Ÿ���� ����
        Vector3 north = Vector3.forward;
        Vector3 south = Vector3.back;
        Vector3 east = Vector3.right;
        Vector3 west = Vector3.left;

        // ���� ����� ������ ã�� ���� ������ ���
        float maxDot = Mathf.Max(Vector3.Dot(front, north), Vector3.Dot(front, south),
                                 Vector3.Dot(front, east), Vector3.Dot(front, west));

        // ����ġ������ �� ������ �Ǵ�
        if (maxDot == Vector3.Dot(front, north))
        {
            // ������ ���� ����
            //attackRangeRot = AttackRange;//���� ���� ���������� ���ݹ���������
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
        }
        else if (maxDot == Vector3.Dot(front, south))
        {
            // ������ ���� ����
            //attackRangeRot = Utils.RotateArray(AttackRange,2);
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
        }
        else if (maxDot == Vector3.Dot(front, east))
        {
            // ������ ���� ����
            //attackRangeRot = Utils.RotateArray(AttackRange, 1);
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
        }
        else if (maxDot == Vector3.Dot(front, west))
        {
            // ������ ���� ����
            //attackRangeRot = Utils.RotateArray(AttackRange, 3);
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
        }

        // Ÿ�� ���̾� ����ũ�� �����ϱ� ���� ���� �ʱ�ȭ
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);

        layerMask = lowTileMask | highTileMask;
        // ĳ������ ���� ��ġ�� ������ ���
        Vector3 characterPosition = player.transform.position;
        Vector3 forward = -player.transform.forward; // ĳ���Ͱ� �ٶ󺸴� �ݴ� ����
        Vector3 right = player.transform.right; // ĳ������ ������ ����

        // ĳ������ ���� Ÿ�� ��ġ (��� ��)�� ã�� ���� ���� �ʱ�ȭ
        int characterRow = 0;
        int characterCol = 0;
        
        // state.AttackRange �迭�� ��ȸ�Ͽ� ĳ������ ��ġ�� ã��
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

        // ���� ������ Ÿ�� ����Ʈ�� �ʱ�ȭ
        if (attackableTiles.Count > 0)
        {
            attackableTiles.Clear();
        }

        // ���� ������ Ÿ���� ã�� ���� state.AttackRange �迭�� �ٽ� ��ȸ
        for (int i = 0; i < attackRangeRot.GetLength(0); i++)
        {
            for (int j = 0; j < attackRangeRot.GetLength(1); j++)
            {
                if (attackRangeRot[i, j] == 1) // '1'�� ���� ������ Ÿ���� ��Ÿ��
                {
                    // ĳ���ͷκ��� ������� ��ġ ���
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition; // ���� Ÿ�� ��ġ ���

                    // Ÿ�� ��ġ�� ���������� ��ȯ
                    var tilePosInt = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);

                    // ����ĳ��Ʈ�� ����Ͽ� Ÿ�� �˻�
                    RaycastHit hit;
                    var tempPos = new Vector3(tilePosInt.x, tilePosInt.y - 10f, tilePosInt.z);
                    if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
                    {
                        // ����ĳ��Ʈ�� Ÿ�Ͽ� ������ �ش� Ÿ�� ��Ʈ�ѷ��� ������
                        var tileContoller = hit.transform.GetComponent<Tile>();
                        if (!tileContoller.isSomthingOnTile) // Ÿ�� ���� �ٸ� ���� ������
                        {
                            attackableTiles.Add(tileContoller); // ���� ������ Ÿ�� ����Ʈ�� �߰�
                            
                        }
                    }
                }
            }
        }

        foreach(var a in attackableTiles)
        {
            foreach(var b in a.objectsOnTile)
            {
                if(b.tag == "Enemy")
                {
                    IAttackable attackable = b.GetComponent<IAttackable>();
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
        }
    }
    

}
