using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

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

        if (player == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
        ConvertTo2DArray();
        
        //Vector3 front = player.transform.forward;
        Vector3 front = player.firstLookPos.forward;
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
            attackRangeRot = AttackRange;
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("����");
        }
        else if (maxDot == Vector3.Dot(front, south))
        {
            // ������ ���� ����
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("����");
        }
        else if (maxDot == Vector3.Dot(front, east))
        {
            // ������ ���� ����
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("������");
        }
        else if (maxDot == Vector3.Dot(front, west))
        {
            // ������ ���� ����
            attackRangeRot = Utils.RotateArray(AttackRange, 3);
            Debug.Log("����");
        }

        // Ÿ�� ���̾� ����ũ�� �����ϱ� ���� ���� �ʱ�ȭ
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
        int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);

        layerMask = lowTileMask | highTileMask;
        // ĳ������ ���� ��ġ�� ������ ���
        Vector3 characterPosition = player.firstLookPos.position;
        Vector3 forward = -player.firstLookPos.forward; // ĳ���Ͱ� �ٶ󺸴� �ݴ� ����
        Vector3 right = player.firstLookPos.right; // ĳ������ ������ ����

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
