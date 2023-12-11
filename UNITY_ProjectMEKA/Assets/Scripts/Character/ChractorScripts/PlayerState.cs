using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Aoe,
        PiercingShot,
        ChainAttack,
        HitScan,
        Instantaneous,
    }
    public enum Passive
    {
        None,//����
        Unstoppable,//���� �Ұ�
        Explosion,//����
        BusterCall,//���� ����
        SpeedUp,//�̼� ����
        Counterattack,//����
        Spite,//����
        Outlander,//�ƿ�����
        Tenacity,//������ ����
        Revenge,//����
        Mechanic,//�����

    }
    public enum Skills
    {
        None,
        Snapshot,
        StunningBlow,
        Test2,
        Test3,
    }

    //���� ���ɹ���
    public int[,] AttackRange;

    [SerializeField, Header("ĳ���� ID ����")]//p
    public int id;

    [SerializeField, Header("ĳ���� �̸� ����")]//p
    public string name;

    [SerializeField, Header("ĳ���� ��ġ �ڽ�Ʈ")]//p
    public int arrangeCost;

    [SerializeField, Header("ĳ���� ��ġ ��Ÿ��")]//p
    public float arrangeCoolTime;

    [SerializeField, Header("ĳ���� ��ų ����")]//p
    public Skills skill;

    [HideInInspector]
    public float cost;

    [SerializeField, Header("ĳ���� �ִ� �ñ׸�")]//p
    public float maxCost;

    [SerializeField, Header("ĳ���� ��ų �ڽ�Ʈ")]//p
    public float skillCost;

    [SerializeField, Header("��ų ��Ÿ��")]//p
    public float skillCoolTime;

    [SerializeField, Header("��ų ���ӽð�")]//p
    public float skillDuration;

    [SerializeField, Header("�Ӽ� ����")]//p,e
    public Defines.Property property;

    [SerializeField, Header("ĳ���� Ŭ���� ����")]//p
    public Defines.Occupation occupation;

    [SerializeField, Header("ü�� ����")]//p,e
    public float maxHp;

    [SerializeField, Header("���� ����")]//p,e
    public float amror;

    [SerializeField, Header("��ų ��Ÿ�� ����")]//p
    public float cooldown;

    [SerializeField, Header("���ݷ� ����")]//p,e
    public float damage;

    [SerializeField, Header("���� ������ ����")]//p,e
    public float attackDelay;

    [SerializeField, Header("���Ÿ� Ÿ���Ͻ� ������ �߻�ü")]//p,e
    public string BulletName;

    [SerializeField, Header("�߻�ü Ÿ��")]//p,e
    public Type BulletType;

    [SerializeField, Header("�߻��Ҷ� ����Ʈ")]//p,e
    public string flashName;

    [SerializeField, Header("�¾����� ����Ʈ")]//p,e
    public string hitName;

    [SerializeField, Header("��")]//p,e
    public int hang;
    [SerializeField, Header("��")]//p,e
    public int yal;
    [SerializeField, Header("���ݹ�������")]//p,e
    public int[] rangeAttack;

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

        // 1���� �迭�� �����͸� 2���� �迭�� ��ȯ
        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

        //return AttackRange; // ��ȯ�� 2���� �迭 ��ȯ
    }

    [HideInInspector]
    public int level;//����

    [HideInInspector]
    public int grade;//��� ������ 10���ڸ��� �ش��ϸ� 6������ ���簡��

    [HideInInspector]
    public int experience;//�� ����ġ �̻��̿��� ������ ++

    [HideInInspector]
    public int currentExperience;

    [HideInInspector]
    public float Hp;

    [HideInInspector]
    public int ID;

    private void Awake()
    {
        Hp = maxHp;
    }
}
