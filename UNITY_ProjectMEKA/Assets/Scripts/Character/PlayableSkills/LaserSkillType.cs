using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkillType : SkillBase
{
    [SerializeField, Header("레이저 생명주기")]
    public float skillDuration;
    [SerializeField, Header("이팩트 이름")]
    public string effectName;
    [SerializeField, Header("몇퍼센트 데미지 1이 100%)")]
    public float figure;

    private PlayerController player;
    private float timer;
    private float duration;
    private GameObject obj;
    private List<Collider> colliders;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkillUsing = false;
        timer = skillCoolTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration)
            {
                duration = 0;
                isSkillUsing = false;
                player.ani.speed = 1;
                obj.GetComponent<PoolAble>().ReleaseObject();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }
    }

    public override void UseSkill()
    {
        if (player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.Auto:

                    break;
                case Defines.SkillType.Instant:
                    InstantSkill();
                    break;
                case Defines.SkillType.SnipingSingle:

                    break;
                case Defines.SkillType.SnipingArea:

                    break;
            }
        }
    }
    public void InstantSkill()
    {
        player.ani.SetTrigger("Skill");
        
    }
    public void LaserBeamPoolEffect()
    {
        player.ani.speed = 0;
        obj = ObjectPoolManager.instance.GetGo(effectName);
        var laser = obj.GetComponent<Laser>();
        laser.player = player;
        obj.SetActive(false);
        obj.SetActive(true);
        CheckOverlapBoxes();
    }
    void CheckOverlapBoxes()
    {

        if (player == null || AttackRange == null || transform == null)
        {
            return;
        }
        colliders = new List<Collider>(); // 리스트 초기화
        ConvertTo2DArray();

        // 플레이어의 로컬 포워드 및 로컬 오른쪽 방향을 설정
        Vector3 forward = -player.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = player.transform.right; // 플레이어의 로컬 오른쪽

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                    break;
                }
            }
        }

        // 상자 영역을 생성하고 콜라이더를 검출하는 루프
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    // 플레이어 위치를 기준으로 상대적인 위치 계산
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    // 상자 크기를 고정된 값으로 설정
                    Vector3 boxSize = new Vector3(1, 5, 1);
                    Collider[] hitColliders = Physics.OverlapBox(correctedPosition, boxSize / 2, Quaternion.identity);

                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.CompareTag("EnemyCollider") && !colliders.Contains(hitCollider))
                        {
                            hitCollider.GetComponentInParent<IAttackable>().OnAttack(player.state.damage * figure);
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
        

    }
    void OnDrawGizmos()
    {


        ConvertTo2DArray();

        Vector3 forward = -player.transform.forward; // 플레이어의 로컬 포워드
        Vector3 right = player.transform.right; // 플레이어의 로컬 오른쪽

        int characterRow = 0;
        int characterCol = 0;

        // 플레이어의 위치를 찾는 루프
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                    break;
                }
            }
        }

        Gizmos.color = Color.red; // 색상 설정

        // 공격 가능 영역을 나타내는 상자 그리기
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // 상자 크기

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
