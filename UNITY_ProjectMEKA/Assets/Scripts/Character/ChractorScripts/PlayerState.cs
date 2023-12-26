using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class PlayerState : CharacterState
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
    //public enum Skills
    //{
    //    None,
    //    Snapshot,
    //    StunningBlow,
    //    Test2,
    //    Test3,
    //}

    ////���� ���ɹ���
    //public int[,] AttackRange;

    //[SerializeField, Header("ĳ���� ID ����")]//p
    //public int id;

    //[SerializeField, Header("ĳ���� �̸� ����")]//p
    //public string name;

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

    [SerializeField, Header("ĳ���� Ŭ���� ����")]//p
    public Defines.Occupation occupation;

    [SerializeField, Header("ġ��Ÿ Ȯ�� 0 ~ 1")]//p
    public float critChance;

    [SerializeField, Header("ġ��Ÿ ���� �ۼ�Ʈ 1 = 100")]//p
    public float fatalDamage = 1;

    [SerializeField, Header("�������� �켱Ÿ���ΰ�")]
    public bool isFirstTargetIsFly;

    [HideInInspector]
    public int experience;//�� ����ġ �̻��̿��� ������ ++

    [HideInInspector]
    public int currentExperience;

}
