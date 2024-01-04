using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class HitScan : MonoBehaviour
{
    public PlayerController player;
    public LayerMask enemyLayer; // 'Enemy' 레이어에 대한 레이어 마스크
    public GameObject hitEffectPrefab;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        enemyLayer = 1 << enemyLayerIndex;
    }

    public void Shoot()
    {
        Vector3 direction = player.target.transform.position - player.FirePosition.transform.position;
        RaycastHit[] hits = Physics.RaycastAll(player.FirePosition.transform.position, direction, Mathf.Infinity, enemyLayer);

        var FlashInstance = ObjectPoolManager.instance.GetGo(player.state.flashName);
        FlashInstance.transform.position = player.FirePosition.transform.position;
        //FlashInstance.transform.rotation = Quaternion.LookRotation(player.target.transform.position);
        Vector3 targetDirection = player.target.transform.position - player.FirePosition.transform.position;
        FlashInstance.transform.rotation = Quaternion.LookRotation(targetDirection);
        FlashInstance.SetActive(false);
        FlashInstance.SetActive(true);
        FlashInstance.GetComponent<PoolAble>().ReleaseObject(2f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("EnemyCollider"))
            {
                //Debug.Log("Enemy collider on Enemy layer hit: " + hit.collider.name);
                if(Random.Range(0f,1f) <= player.state.critChance)
                {
                    hit.collider.gameObject.GetComponentInParent<IAttackable>().OnAttack((player.state.damage * player.state.fatalDamage) + player.Rockpaperscissors());

                }
                else
                {
                    hit.collider.gameObject.GetComponentInParent<IAttackable>().OnAttack(player.state.damage + player.Rockpaperscissors());
                }

                var hitInstance = ObjectPoolManager.instance.GetGo(player.state.hitName);
                hitInstance.transform.position = hit.point;
                hitInstance.transform.rotation = Quaternion.LookRotation(hit.normal);
                hitInstance.SetActive(false);
                hitInstance.SetActive(true);

                hitInstance.GetComponent<PoolAble>().ReleaseObject(2f);
                //GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(hitEffect, 2f); 
            }
            
        }
    }

}
