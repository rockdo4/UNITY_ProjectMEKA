using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;


public class CharacterState : MonoBehaviour
{
    [System.Serializable]
    private class PoolObject
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject perfab;
        // ��� �̸� ���� �س�������
        public int count;
    }

    [SerializeField, Header("�� ĳ���Ϳ��� ����� Ǯ")]
    private PoolObject[] objectInfos = null;

    public int[,] AttackRange;

    [SerializeField, Header("���� ����")]
    public int lv = 1;
    private int prevLv;

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
    [HideInInspector]
    public float maxShield = 0;

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

    public float Hp;

    private void Awake()
    {
        Hp = maxHp;
        prevLv = lv;
    }

	private void Update()
	{
		if(prevLv != lv)
        {
            prevLv = lv;
            var checkPlayer = gameObject.GetComponent<PlayerState>();
            if(checkPlayer == null)
            {
                return;
            }

            var table = DataTableMgr.GetTable<CharacterLevelTable>();
            var data = table.GetLevelData(id * 100 + lv);

            if(data != null)
            {
                checkPlayer.damage = data.CharacterDamage;
                checkPlayer.armor = data.CharacterArmor;
                checkPlayer.maxHp = data.CharacterHP;
                checkPlayer.Hp = data.CharacterHP;
            }
        }
	}
}
