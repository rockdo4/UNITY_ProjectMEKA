using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KALEASkill : SkillBase
{
    private PlayerController player;
    private float timer;
    private float skillDuration;
    private bool isSkill;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        isSkill = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime&&!isSkill)
        {
            timer = 0;
            player.state.cost -= player.state.skillCost;
            UseSkill();
        }
        else if(isSkill)
        {
            skillDuration += Time.deltaTime;
            if(skillDuration >= player.state.skillDuration)
            {
                skillDuration = 0;
                isSkill = false;
                player.ani.runtimeAnimatorController = player.currnetAnimationController;
            }
        }
    }

    public override void UseSkill()
    {
        player.currnetAnimationController = player.ani.runtimeAnimatorController;
        player.ani.runtimeAnimatorController = player.animationController;
        isSkill = true;
    }

    public void SkillAttack()
    {
        var p = player.target.GetComponentInParent<IAttackable>();
        p.OnAttack(player.state.damage);
    }
    public void NextAttack()
    {
        player.ani.SetTrigger("NextAttack");
    }
}
