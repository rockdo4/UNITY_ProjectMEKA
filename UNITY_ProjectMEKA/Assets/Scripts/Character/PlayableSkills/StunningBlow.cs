using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunningBlow : SkillBase
{
    
    private float timer;
    private PlayerController player;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public override void UseSkill()
    {
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
        {
            timer = 0;
            player.state.cost -= player.state.skillCost;
            player.ani.SetTrigger("Skill");
        }
        else
        {
            Debug.Log("마나부족");
        }
    }
    public void Stunnig()
    {
        Debug.Log("Stun");
        var obj = ObjectPoolManager.instance.GetGo("JestSkillEffect");
        
        Vector3 pos = player.transform.position + player.transform.forward * 1f;
        pos.y += 0.5f; // y축 위치 조정

        obj.transform.position = pos;
        Quaternion rot = Quaternion.Euler(-90,0,0);
        obj.transform.rotation = rot;
        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(2f);
        foreach (var p in player.rangeInEnemys) 
        {
            p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
        }

    }
}
