using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MERIASkill : SkillBase
{
    private PlayerController player;
    private float timer;
    private float skillDuration;
    private bool isSkill;
    private float saveDelay;
    //50 20 버스트 사용 시, 12초동안 회복 속도가 1.5배 증가한다

    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        isSkill = false;
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
                player.ani.speed = 1;
                player.state.attackDelay = saveDelay;
            }
        }
    }

    public override void UseSkill()
    {
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime )
        {
            timer = 0;
            player.state.cost -= player.state.skillCost;
            var obj = ObjectPoolManager.instance.GetGo("MeriaSkillEffect");
            if (obj == null)
            {
                return;
            }
            obj.transform.position = player.transform.position;
            obj.SetActive(false);
            obj.SetActive(true);
            obj.GetComponent<PoolAble>().ReleaseObject(2f);
            player.ani.speed = 1.5f;
            saveDelay = player.state.attackDelay;
            player.state.attackDelay = saveDelay * 1.5f;
            isSkill = true;
        }

        
    }
}
