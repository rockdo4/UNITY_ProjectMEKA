using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBossCollider : MonoBehaviour
{
    EnemyController enemy;
    [SerializeField, Header("행")]//p,e
    public int hang;
    [SerializeField, Header("열")]//p,e
    public int yal;
    [SerializeField, Header("공격범위설정")]//p,e
    public int[] rangeAttack;
    [HideInInspector]
    public int[,] AttackRange;


    //[SerializeField, Header("적 공중형, 지상형 선택(언체크 지상형)")]//e
    //public bool isFly;


    public void ConvertTo2DArray()
    {
        // 1차원 배열의 길이가 행과 열의 곱과 일치하는지 확인
        if (rangeAttack.Length != hang * yal)
        {
            Debug.LogError("1차원 배열의 길이가 행과 열의 곱과 일치하지 않습니다.");
            return;
        }

        // 새 2차원 배열 생성
        AttackRange = new int[hang, yal];

        // 1차원 배열의 데이터를 2차원 배열로 변환
        for (int i = 0; i < hang; i++)
        {
            for (int j = 0; j < yal; j++)
            {
                AttackRange[i, j] = rangeAttack[i * yal + j];
            }
        }

        //return AttackRange; // 변환된 2차원 배열 반환
    }
    private void Awake()
    {
        enemy = transform.parent.GetComponent<EnemyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ConvertTo2DArray();
        CreateColliders();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            //Debug.Log(other, other);
            if (!enemy.rangeInSecondPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                //other�� enemy�� ����ĭ�� ������ ����Ʈ�� �����ʾƾ���
                if (enemy.CurrentGridPos != other.GetComponentInParent<PlayerController>().CurrentGridPos)
                {
                    enemy.rangeInSecondPlayers.Add(other.GetComponentInParent<Transform>().gameObject);
                    var obj = other.GetComponentInParent<CanDie>();
                    obj.action.AddListener(() =>
                    {
                        enemy.rangeInSecondPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);
                    });
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if (enemy.rangeInSecondPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                enemy.rangeInSecondPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);
                var obj = other.GetComponentInParent<CanDie>();
                obj.action.RemoveListener(() =>
                {
                    enemy.rangeInSecondPlayers.Remove(other.GetComponentInParent<GameObject>().gameObject);
                });
            }
        }
    }
    void CreateColliders()
    {
        if (enemy.state == null || AttackRange == null || transform == null)
        {
            return;
        }

        Vector3 forward = -enemy.transform.forward;
        Vector3 right = enemy.transform.right;
        Vector3 parentScale = enemy.transform.localScale;
        int characterRow = 0;
        int characterCol = 0;

        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        for (int i = 0; i < AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < AttackRange.GetLength(1); j++)
            {
                if (AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = new Vector3(relativePosition.x / parentScale.x, relativePosition.y / parentScale.y, relativePosition.z / parentScale.z);

                    BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                    collider.size = new Vector3(1 / parentScale.x, 3 / parentScale.y, 1 / parentScale.z);
                    collider.center = correctedPosition;
                    collider.isTrigger = true;
                }
            }
        }


    }
}
