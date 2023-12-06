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
    }
    private void Update()
    {
        
    }
    public override void UseSkill()
    {
        if(player.state.cost >= player.state.skillCost)
        {
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
        foreach(var p in player.rangeInEnemys) 
        {
            p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
        }

    }
}
