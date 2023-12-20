using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingSkillType : SkillBase
{
    
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
    public override void UseSkill()
    {
        switch (skillType)
        {
            case Defines.SkillType.SnipingSingle:
                break;
            case Defines.SkillType.SnipingArea:

                break;

        }
        
    }
    public void IsSkillAttackArea()
    {

    }

}
