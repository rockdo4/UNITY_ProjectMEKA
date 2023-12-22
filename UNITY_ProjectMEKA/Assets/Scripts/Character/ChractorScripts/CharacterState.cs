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
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }

    [SerializeField, Header("현 캐릭터에서 사용할 풀")]
    private PoolObject[] objectInfos = null;

    public int[,] AttackRange;

    [SerializeField, Header("레벨 설정")]
    public int lv = 1;
    private int prevLv;

    [SerializeField, Header("ID 설정")]//p
    public int id;

    [SerializeField, Header("이름 설정")]//p
    public string name;

    [SerializeField, Header("속성 설정")]//p,e
    public Defines.Property property;

    [SerializeField, Header("체력 설정")]//p,e
    public float maxHp;

    [SerializeField, Header("방어력 설정")]//p,e
    public float armor;

    [SerializeField, Header("공격력 설정")]//p,e
    public float damage;

    [SerializeField, Header("공격 딜레이 설정")]//p,e
    public float attackDelay;

    [SerializeField, Header("방어막 수치 설정")]
    public float shield;
    [HideInInspector]
    public float maxShield = 0;

    [SerializeField, Header("원거리 타입일시 장착할 발사체")]//p,e
    public string BulletName;

    [SerializeField, Header("발사체 타입")]//p,e
    public ProjectileType BulletType;

    [SerializeField, Header("발사할때 이팩트")]//p,e
    public string flashName;

    [SerializeField, Header("맞았을때 이팩트")]//p,e
    public string hitName;

    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;

    public void ConvertTo2DArray()
    {
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
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
    public int level;//레벨

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
