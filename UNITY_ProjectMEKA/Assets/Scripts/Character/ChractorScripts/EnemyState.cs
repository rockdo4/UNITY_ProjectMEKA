using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class EnemyState : CharacterState
{
    //public enum Type
    //{
    //    Bullet,
    //    Aoe,
    //    PiercingShot,
    //    ChainAttack,
    //    HitScan,
    //    Instantaneous,
    //}
    //public enum Passive
    //{
    //    None,//����
    //    Unstoppable,//���� �Ұ�
    //    Explosion,//����
    //    BusterCall,//���� ����
    //    SpeedUp,//�̼� ����
    //    Counterattack,//����
    //    Spite,//����
    //    Outlander,//�ƿ�����
    //    Tenacity,//������ ����
    //    Revenge,//����
    //    Mechanic,//�����

    //}

    //���� ���ɹ���
    //public int[,] AttackRange;

    //[SerializeField, Header("ĳ���� ID ����")]
    //public int id;

    //[SerializeField, Header("ĳ���� �̸� ����")]
    //public string name;

    [SerializeField, Header("�� �нú� ����")]//e
    public List<Passive> passive;

    [SerializeField, Header("�� Ÿ�� ����")]//e
    public Defines.EnemyType enemyType;

    //[SerializeField, Header("�Ӽ� ����")]//p,e
    //public Defines.Property property;

    //[SerializeField, Header("ĳ���� Ŭ���� ����")]//p
    //public Defines.Occupation occupation;

    //[SerializeField, Header("ü�� ����")]//p,e
    //public float maxHp;

    //[SerializeField, Header("���� ����")]//p,e
    //public float amror;

    // 11.22, �����, �� �̵��� �ʿ�
    [SerializeField, Header("�̵��ӷ� ����,�� �̵��� �ʿ�")]//e
    public float speed;

    //[SerializeField, Header("��ų ��Ÿ�� ����")]//p
    //public float cooldown;

    //[SerializeField, Header("���ݷ� ����")]//p,e
    //public float damage;

    //[SerializeField, Header("���� ������ ����")]//p,e
    //public float attackDelay;

    //[SerializeField, Header("���Ÿ� Ÿ���Ͻ� ������ �߻�ü")]//p,e
    //public string BulletName;

    //[SerializeField, Header("�߻�ü Ÿ��")]//p,e
    //public Type BulletType;

    //[SerializeField, Header("�߻��Ҷ� ����Ʈ")]//p,e
    //public string flashName;

    //[SerializeField, Header("�¾����� ����Ʈ")]//p,e
    //public string hitName;

    //[SerializeField, Header("��")]//p,e
    //public int hang;
    //[SerializeField, Header("��")]//p,e
    //public int yal;
    //[SerializeField, Header("���ݹ�������")]//p,e
    //public int[] rangeAttack;

    [SerializeField, Header("�� ������, ������ ����(��üũ ������)")]//e
    public bool isFly;


    //public void ConvertTo2DArray()
    //{
    //    // 1���� �迭�� ���̰� ��� ���� ���� ��ġ�ϴ��� Ȯ��
    //    if (rangeAttack.Length != hang * yal)
    //    {
    //        Debug.LogError("1���� �迭�� ���̰� ��� ���� ���� ��ġ���� �ʽ��ϴ�.");
    //        return;
    //    }

    //    // �� 2���� �迭 ����
    //    AttackRange = new int[hang, yal];

    //    // 1���� �迭�� �����͸� 2���� �迭�� ��ȯ
    //    for (int i = 0; i < hang; i++)
    //    {
    //        for (int j = 0; j < yal; j++)
    //        {
    //            AttackRange[i, j] = rangeAttack[i * yal + j];
    //        }
    //    }

    //    //return AttackRange; // ��ȯ�� 2���� �迭 ��ȯ
    //}

    //[HideInInspector]
    //public int level;//����

    //[HideInInspector]
    //public int grade;//��� ������ 10���ڸ��� �ش��ϸ� 6������ ���簡��

    //[HideInInspector]
    //public int experience;//�� ����ġ �̻��̿��� ������ ++

    //[HideInInspector]
    //public int currentExperience;

    //[HideInInspector]
    //public float Hp;

    //[HideInInspector]
    //public int ID;

    
    public bool isBlock = true;

    //private void Awake()
    //{
    //    Hp = maxHp;
    //}
}
