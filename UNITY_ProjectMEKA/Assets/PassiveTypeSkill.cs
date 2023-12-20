using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveTypeSkill : SkillBase
{
    [SerializeField, Header("공격 범위가 따로 존재하는가")]
    public bool isAttackRage;
    
    [SerializeField, Header("증가치")]
    public float figure;

    [SerializeField, Header("사용할 이팩트 이름")]
    public string effectName;

    
    private PlayerController player;
    private List<Collider> colliders;
    private float timer;
    private float saveArmor;
    private GameObject obj;
    private bool isOneEffect;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkillUsing = true;
        isOneEffect = false;
        ConvertTo2DArray();
    }
    private void OnEnable()
    {
        isSkillUsing = true;
        colliders = new List<Collider>(); // 리스트 초기화
        isOneEffect = false;
    }
    private void OnDisable()
    {
        isSkillUsing = false;
        foreach (var co in colliders)
        {
            var pl = co.GetComponentInParent<PlayerController>();
            pl.state.armor -= pl.state.armor * figure;
        }
        if(obj != null)
        {
            obj.GetComponent<PoolAble>().ReleaseObject();
        }
    }
    private void Update()
    {
        if(!isOneEffect && player.currentState != PlayerController.CharacterStates.Arrange)
        {
            isOneEffect = true;
            obj = ObjectPoolManager.instance.GetGo(effectName);
            obj.transform.position = transform.position;
            obj.SetActive(false);
            obj.SetActive(true);
        }
        
        timer += Time.deltaTime;
        if(isAttackRage)
        {
            if (timer >= 0.3f)
            {
                timer = 0f;
                CheckOverlapBoxes();
            }
        }
        
    }
    public override void UseSkill()
    {
    }
    void CheckOverlapBoxes()
    {

        if (player == null || AttackRange == null || transform == null)
        {
            return;
        }
        

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
                        if (hitCollider.CompareTag("PlayerCollider") && !colliders.Contains(hitCollider))
                        {
                            var pl = hitCollider.GetComponentInParent<PlayerController>();
                            pl.state.armor += pl.state.armor * figure;
                            colliders.Add(hitCollider);
                        }
                    }
                }
            }
        }
        

    }
    void OnDrawGizmos()
    {

        if (player == null || AttackRange == null) return;

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
