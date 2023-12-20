using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveTypeSkill : SkillBase
{
    [SerializeField, Header("���� ������ ���� �����ϴ°�")]
    public bool isAttackRage;
    
    [SerializeField, Header("����ġ")]
    public float figure;

    [SerializeField, Header("����� ����Ʈ �̸�")]
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
        colliders = new List<Collider>(); // ����Ʈ �ʱ�ȭ
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
        

        // �÷��̾��� ���� ������ �� ���� ������ ������ ����
        Vector3 forward = -player.transform.forward; // �÷��̾��� ���� ������
        Vector3 right = player.transform.right; // �÷��̾��� ���� ������

        int characterRow = 0;
        int characterCol = 0;

        // �÷��̾��� ��ġ�� ã�� ����
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

        // ���� ������ �����ϰ� �ݶ��̴��� �����ϴ� ����
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    // �÷��̾� ��ġ�� �������� ������� ��ġ ���
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    // ���� ũ�⸦ ������ ������ ����
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

        Vector3 forward = -player.transform.forward; // �÷��̾��� ���� ������
        Vector3 right = player.transform.right; // �÷��̾��� ���� ������

        int characterRow = 0;
        int characterCol = 0;

        // �÷��̾��� ��ġ�� ã�� ����
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

        Gizmos.color = Color.red; // ���� ����

        // ���� ���� ������ ��Ÿ���� ���� �׸���
        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = player.transform.position + relativePosition;

                    Vector3 boxSize = new Vector3(1, 5, 1); // ���� ũ��

                    Gizmos.DrawWireCube(correctedPosition, boxSize);
                }
            }
        }
    }
}
