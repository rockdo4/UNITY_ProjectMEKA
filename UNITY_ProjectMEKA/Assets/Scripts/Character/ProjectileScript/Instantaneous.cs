using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class Instantaneous : MonoBehaviour
{
    private PlayerController player;
    private GameObject[] enemys;
    private StageManager stageManager;
    public Tile tile;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }
    public void Shoot()
    {
        player.NormalAttackSound();
        Vector3 os = player.target.transform.position;
        Vector3Int CurrentGridPos = new Vector3Int(Mathf.RoundToInt(os.x), Mathf.RoundToInt(os.y), Mathf.RoundToInt(os.z));
        tile = stageManager.tileManager.GetCurrentTile(CurrentGridPos);
        IAttackable t = player.target.GetComponentInParent<IAttackable>();
        t.OnAttack(player.state.damage);
        foreach (var en in tile.objectsOnTile) 
        {
            if(en.tag == "Enemy")
            {
                if (Random.Range(0f, 1f) >= player.state.critChance)
                {
                    en.GetComponent<IAttackable>().OnAttack(player.state.damage * player.state.fatalDamage);
                }
                else
                {
                    en.GetComponent<IAttackable>().OnAttack(player.state.damage);
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
