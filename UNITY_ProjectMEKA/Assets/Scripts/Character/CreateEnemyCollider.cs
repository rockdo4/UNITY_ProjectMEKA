using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateEnemyCollider : MonoBehaviour
{
    EnemyController enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<EnemyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateColliders();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            //Debug.Log(other, other);
            if (!enemy.rangeInPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                //other�� enemy�� ����ĭ�� ������ ����Ʈ�� �����ʾƾ���
                if (enemy.CurrentGridPos != other.GetComponentInParent<PlayerController>().CurrentGridPos)
                {
                    enemy.rangeInPlayers.Add(other.GetComponentInParent<Transform>().gameObject);
                    var obj = other.GetComponentInParent<CanDie>();
                    obj.action?.AddListener(() =>
                    {
                        //if (other.GetComponentInParent<Transform>().gameObject.activeInHierarchy)//34번째줄
                        if (other != null && other.GetComponentInParent<Transform>() != null)
                        {
                            enemy.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);
                        }
                        //enemy.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);
                    });
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if (enemy.rangeInPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                enemy.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);
                var obj = other.GetComponentInParent<CanDie>();
                obj.action.RemoveListener(() =>
                {
                    enemy.rangeInPlayers.Remove(other.GetComponentInParent<GameObject>().gameObject);
                });
            }
        }
    }
    void CreateColliders()
    {
        if (enemy.state == null || enemy.state.AttackRange == null || transform == null)
        {
            return;
        }


        
        Vector3 forward = -enemy.transform.forward;
        Vector3 right = enemy.transform.right;
        Vector3 parentScale = enemy.transform.localScale;
        int characterRow = 0;
        int characterCol = 0;

        for (int i = 0; i < enemy.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < enemy.state.AttackRange.GetLength(1); j++)
            {
                if (enemy.state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        for (int i = 0; i < enemy.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < enemy.state.AttackRange.GetLength(1); j++)
            {
                if (enemy.state.AttackRange[i, j] == 1)
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
