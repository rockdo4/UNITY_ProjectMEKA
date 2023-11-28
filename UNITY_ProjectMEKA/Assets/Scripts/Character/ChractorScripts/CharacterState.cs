using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Aoe,
        PiercingShot,
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

    //���� ���ɹ���
    public int[,] AttackRange;
    

    [SerializeField, Header("�� �нú� ����")]
    public List<Passive> passive;

    [SerializeField, Header("�� Ÿ�� ����")]
    public Defines.EnemyType enemyType;
    
    [SerializeField, Header("�Ӽ� ����")]
    public Defines.Property property;

    [SerializeField, Header("ĳ���� Ŭ���� ����")]
    public Defines.Occupation occupation;

    [SerializeField, Header("ü�� ����")]
    public float maxHp;

    [SerializeField, Header("���� ����")]
    public float amror;

    // 11.22, �����, �� �̵��� �ʿ�
    [SerializeField, Header("�̵��ӷ� ����,�� �̵��� �ʿ�")]
    public float speed;

    [SerializeField, Header("��ų ��Ÿ�� ����")]
    public float cooldown;

    [SerializeField, Header("���� �����Ÿ� ����")]
    public float range;

    [SerializeField, Header("���ݷ� ����")]
    public float damage;

    [SerializeField, Header("���� ������ ����")]
    public float attackDelay;

    [SerializeField, Header("����ġ�� ���� ���� ����")]
    public int exp;

    [SerializeField, Header("���Ÿ� Ÿ���Ͻ� ������ �߻�ü")]
    public string BulletName;

    [SerializeField, Header("�߻�ü Ÿ��")]
    public Type BulletType;

    [SerializeField, Header("�߻��Ҷ� ����Ʈ")]
    public string flashName;

    [SerializeField, Header("�¾����� ����Ʈ")]
    public string hitName;

    [SerializeField, Header("��")]
    public int hang;
    [SerializeField, Header("��")]
    public int yal;
    [SerializeField, Header("���ݹ�������")]
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

    //[HideInInspector]
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

    [HideInInspector]
    public bool isBlock = true;

    private void Awake()
    {
        Hp = maxHp;
    }
   
}
