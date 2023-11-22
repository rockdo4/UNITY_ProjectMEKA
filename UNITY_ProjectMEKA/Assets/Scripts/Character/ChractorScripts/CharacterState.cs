using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
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
