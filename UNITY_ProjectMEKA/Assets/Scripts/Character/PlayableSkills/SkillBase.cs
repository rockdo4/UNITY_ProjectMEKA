using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField, Header("��ų �ڽ�Ʈ")]//p
    public float skillCost;

    [SerializeField, Header("��ų ��Ÿ��")]//p
    public float skillCoolTime;

    //� ��ų���� ǥ��: �ڵ�, ���, �������� ����, �������� ���� 
    [SerializeField, Header("��ų Ÿ��")]
    public Defines.SkillType skillType;

    [HideInInspector]
    public bool isSkillUsing;

    [SerializeField, Header("�⺻ ���� ������ �����ϴ°�")]
    public bool isRangeOut;

    [SerializeField, Header("��")]//p,e
    public int hang;
    [SerializeField, Header("��")]//p,e
    public int yal;
    [SerializeField, Header("���ݹ�������")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;
    [HideInInspector]
    public List<Tile> attackableTiles;
    [HideInInspector]
    public int[,] attackRangeRot;
    public void ConvertTo2DArray()
    {
        // 1���� �迭�� ���̰� ��� ���� ���� ��ġ�ϴ��� Ȯ��
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1���� �迭�� ���̰� ��� ���� ���� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // �� 2���� �迭 ����
        AttackRange = new int[yal, hang];

        for (int i = 0; i < yal; i++)
        {
            for (int j = 0; j < hang; j++)
            {
                AttackRange[i, j] = rangeAttack[(hang - j - 1) * yal + i];
            }
        }
    }


    // 12.20, �����, �߰�
    public List<GameObject> targetList = new List<GameObject>();

    public abstract void UseSkill();
}
