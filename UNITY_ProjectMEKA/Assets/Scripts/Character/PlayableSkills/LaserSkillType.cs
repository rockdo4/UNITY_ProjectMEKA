using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkillType : SkillBase
{
    [SerializeField, Header("레이저 생명주기")]
    public float skillDuration;
    [SerializeField, Header("이팩트 이름")]
    public string effectName;
    private PlayerController player;
    private float timer;
    private float duration;
    private GameObject obj;
    
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkillUsing = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration)
            {
                duration = 0;
                isSkillUsing = false;
                player.ani.speed = 1;
                obj.GetComponent<PoolAble>().ReleaseObject();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }
    }

    public override void UseSkill()
    {
        if (player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.Auto:

                    break;
                case Defines.SkillType.Instant:
                    InstantSkill();
                    break;
                case Defines.SkillType.SnipingSingle:

                    break;
                case Defines.SkillType.SnipingArea:

                    break;
            }
        }
    }
    public void InstantSkill()
    {
        player.ani.SetTrigger("Skill");
        
    }
    public void LaserBeamPoolEffect()
    {
        player.ani.speed = 0;
        obj = ObjectPoolManager.instance.GetGo(effectName);
        var laser = obj.GetComponent<Laser>();
        laser.player = player;
        obj.SetActive(false);
        obj.SetActive(true);
    }
}
