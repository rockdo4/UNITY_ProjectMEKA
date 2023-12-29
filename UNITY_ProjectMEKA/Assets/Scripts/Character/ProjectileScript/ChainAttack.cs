using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Defines;

public class ChainAttack : MonoBehaviour
{
    public Transform target;
    public float damage;
    public bool isReleased = false;
    private GameObject[] enemys;
    private float timer;
    private float sTimer;
    private bool isOne = false;
    public DamageDeleteTime ddt;
    public PlayerController player;
    private Tile tile;
    private StageManager stageManager;

    public void Start()
    {
        ddt = GetComponent<DamageDeleteTime>();
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }
    public void Update()
    {
        
        timer += Time.deltaTime;
        if(timer > ddt.DamageTime && !isOne)
        {
            isOne = true;
            Explosion();
        }
        else
        {
            if(transform != null || target != null)
            {
                transform.position = target.position;
            }
        }
        sTimer += Time.deltaTime;
        if(sTimer > ddt.deleteTime)
        {
            sTimer = 0;
            timer = 0;
            isOne = false;
            ReleaseObject();
        }

    }

    public void Explosion()
    {
        player.NormalAttackSound();
        Vector3 os = transform.position;
        Vector3Int CurrentGridPos = new Vector3Int(Mathf.RoundToInt(os.x), Mathf.RoundToInt(os.y), Mathf.RoundToInt(os.z));

        tile = stageManager.tileManager.GetCurrentTile(CurrentGridPos);
        if(player.target != null)
        {
            IAttackable t = player.target.GetComponentInParent<IAttackable>();
            t.OnAttack(player.state.damage);
        }
        
        foreach (var en in tile.objectsOnTile)
        {
            if (en.tag == "Enemy")
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

    }

    private void ReleaseObject()
    {
        if (isReleased)
        {
            return;
        }
        isReleased = true;
        GetComponent<PoolAble>().ReleaseObject();
    }
    public void ResetState()
    {
        isReleased = false;
    }
}
