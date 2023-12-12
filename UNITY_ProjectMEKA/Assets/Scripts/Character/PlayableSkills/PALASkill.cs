using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PALASkill : SkillBase
{
    //����Ʈ ��� ��, ������ ���� 3ȸ 120%�� ����, ���� ���ݷ� 10�ʵ��� 20% ���� 
    private PlayerController player;
    private float saveDamage;
    private bool isSkill;
    private float timer;
    private float skillDuration;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkill = false;
        timer = player.state.skillCoolTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkill)
        {
            skillDuration += Time.deltaTime;
            if(skillDuration >= player.state.skillDuration)
            {
                isSkill = false;
                skillDuration = 0;
                player.state.damage = saveDamage;
            }
        }
    }
    public override void UseSkill()
    {
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime) 
        {
            player.ani.SetTrigger("Skill");
            saveDamage = player.state.damage;
            player.state.damage = saveDamage + (saveDamage * 0.2f);
            isSkill = true;
        }
       
    }
    public void PalaSkillAttack()
    {
        var t = player.target.GetComponentInParent<IAttackable>();
        t.OnAttack(saveDamage * 1.2f);
    }
}
