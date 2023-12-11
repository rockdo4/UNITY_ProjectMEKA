using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CreateCollider : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateColliders();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyCollider") && player.state.occupation != Defines.Occupation.Supporters)
        {
            //Debug.Log(other, other);
            if (!player.rangeInEnemys.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                player.rangeInEnemys.Add(other.GetComponentInParent<Transform>().gameObject);
                if (other.GetComponentInParent<EnemyController>().state.enemyType == Defines.EnemyType.OhYaBung)
                {
                    player.enemyBlockCount.Add(1);
                    player.enemyBlockCount.Add(1);
                }
                else
                {
                    player.enemyBlockCount.Add(1);
                }

                var obj = other.GetComponentInParent<CanDie>();
                obj.action.AddListener(() =>
                {
                    player.rangeInEnemys.Remove(other.GetComponentInParent<Transform>().gameObject);
                    if (other.GetComponentInParent<EnemyController>().state.enemyType == Defines.EnemyType.OhYaBung)
                    {
                        player.enemyBlockCount.Remove(1);
                        player.enemyBlockCount.Remove(1);
                    }
                    else
                    {
                        player.enemyBlockCount.Remove(1);
                    }
                });
            }
        }
        if (other.CompareTag("PlayerCollider") && player.state.occupation == Defines.Occupation.Supporters)
        {
            if (!player.rangeInPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                player.rangeInPlayers.Add(other.GetComponentInParent<Transform>().gameObject);


                var obj = other.GetComponentInParent<CanDie>();
                obj.action.AddListener(() =>
                {
                    player.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);

                });
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyCollider") && player.state.occupation != Defines.Occupation.Supporters)
        {
            if (player.rangeInEnemys.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                player.rangeInEnemys.Remove(other.GetComponentInParent<Transform>().gameObject);
                if (other.GetComponentInParent<EnemyController>().state.enemyType == Defines.EnemyType.OhYaBung)
                {
                    player.enemyBlockCount.Remove(1);
                    player.enemyBlockCount.Remove(1);
                }
                else
                {
                    player.enemyBlockCount.Remove(1);
                }
                var obj = other.GetComponentInParent<CanDie>();
                obj.action.RemoveListener(() =>
                {
                    player.rangeInEnemys.Remove(other.GetComponentInParent<Transform>().gameObject);
                    if (other.GetComponentInParent<EnemyController>().state.enemyType == Defines.EnemyType.OhYaBung)
                    {
                        player.enemyBlockCount.Remove(1);
                        player.enemyBlockCount.Remove(1);
                    }
                    else
                    {
                        player.enemyBlockCount.Remove(1);
                    }
                });
            }
        }
        /*Debug.Log(other.tag);*/
        if (other.CompareTag("PlayerCollider") && player.state.occupation == Defines.Occupation.Supporters)
        {
            if (player.rangeInPlayers.Contains(other.GetComponentInParent<Transform>().gameObject))
            {
                player.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);

                var obj = other.GetComponentInParent<CanDie>();
                obj.action.RemoveListener(() =>
                {
                    player.rangeInPlayers.Remove(other.GetComponentInParent<Transform>().gameObject);

                });
            }
        }
    }

    void CreateColliders()
    {
        if (player.state == null || player.state.AttackRange == null || transform == null)
        {
            return;
        }
        player.state.ConvertTo2DArray();

        Vector3 forward = -transform.parent.forward;
        Vector3 right = transform.parent.right;
        Vector3 parentScale = transform.parent.localScale;

        int characterRow = 0;
        int characterCol = 0;

        for (int i = 0; i < player.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < player.state.AttackRange.GetLength(1); j++)
            {
                if (player.state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        for (int i = 0; i < player.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < player.state.AttackRange.GetLength(1); j++)
            {
                if (player.state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 correctedPosition = new Vector3(relativePosition.x / parentScale.x, relativePosition.y / parentScale.y, relativePosition.z / parentScale.z);

                    BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                    collider.size = new Vector3(1 / parentScale.x, 5 / parentScale.y, 1 / parentScale.z);
                    collider.center = correctedPosition;
                    collider.isTrigger = true;
                }
            }
        }
    }
}
