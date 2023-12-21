using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;


public class CharacterState : MonoBehaviour
{
    
    public int[,] AttackRange;

    [SerializeField, Header("ID ����")]//p
    public int id;

    [SerializeField, Header("�̸� ����")]//p
    public string name;

    [SerializeField, Header("�Ӽ� ����")]//p,e
    public Defines.Property property;

    [SerializeField, Header("ü�� ����")]//p,e
    public float maxHp;

    [SerializeField, Header("���� ����")]//p,e
    public float armor;

    [SerializeField, Header("���ݷ� ����")]//p,e
    public float damage;

    [SerializeField, Header("���� ������ ����")]//p,e
    public float attackDelay;

    [SerializeField, Header("�� ��ġ ����")]
    public float shield;

    [SerializeField, Header("���Ÿ� Ÿ���Ͻ� ������ �߻�ü")]//p,e
    public string BulletName;

    [SerializeField, Header("�߻�ü Ÿ��")]//p,e
    public ProjectileType BulletType;

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
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1���� �迭�� ���̰� ��� ���� ���� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        AttackRange = new int[hang, yal];

        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

    }

    [HideInInspector]
    public int level;//����

    [HideInInspector]
    public int grade;//��� ������ 10���ڸ��� �ش��ϸ� 6������ ���簡��

    [HideInInspector]
    public float Hp;

    [HideInInspector]
    public int ID;

    private void Awake()
    {
        Hp = maxHp;
    }
   
}
