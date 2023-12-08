using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantaneous : MonoBehaviour
{
    private PlayerController player;
    private GameObject[] enemys;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    public void Shoot()
    {
        Vector3 os = player.target.transform.position;
        Vector3Int CurrentGridPos = new Vector3Int(Mathf.FloorToInt(os.x), Mathf.FloorToInt(os.y), Mathf.FloorToInt(os.z));
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        IAttackable t = player.target.GetComponentInParent<IAttackable>();
        t.OnAttack(player.state.damage);
        foreach (var enemy in enemys)
        {
            if (CurrentGridPos == enemy.GetComponent<EnemyController>().CurrentGridPos)
            {
                IAttackable aoeDamage = enemy.GetComponent<IAttackable>();
                if (aoeDamage != null && enemy.gameObject != player.target.gameObject)
                {
                    aoeDamage.OnAttack(player.state.damage);//추후 주변 몬스터의 개수만큼 배율변경되도록 수정
                }
            }
        }
        var hitInstance = ObjectPoolManager.instance.GetGo(player.state.hitName);
        hitInstance.transform.position = player.target.transform.position;
        hitInstance.transform.rotation = player.target.transform.rotation;
        hitInstance.SetActive(false);
        hitInstance.SetActive(true);
    }
}
