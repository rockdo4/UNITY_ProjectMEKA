using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberSkill : SkillBase
{
    private float skillTimer;
    private float timer;
    private bool isUsingSkill = false;
    private PlayerController player;
    private GameObject obj;
    private float saveDamage;

    public void Start()
    {
        player = GetComponent<PlayerController>();
        //timer = player.state.skillCoolTime;
        skillTimer = player.state.skillCoolTime;
    }

    public void Update()
    {
        if (isUsingSkill)
        {
            timer += Time.deltaTime;
            if (timer >= player.state.skillDuration)
            {
                timer = 0;
                skillTimer = 0;
                isUsingSkill = false;
                obj.GetComponent<PoolAble>().ReleaseObject();
                player.ani.speed = 1;
                player.state.damage = saveDamage;

            }
        }
        skillTimer += Time.deltaTime;
        //지속시간동안만 공속이 빨라져야한다
        //지속시간이 끝나면 원래대로 돌아가야함

    }
    //12초동안 공격력15%증가
    public override void UseSkill()
    {
        if (player.state.cost >= player.state.skillCost && skillTimer >= player.state.skillCoolTime)
        {
            isUsingSkill = true;
            player.state.cost -= player.state.skillCost;
            obj = ObjectPoolManager.instance.GetGo("AmberSkillEffect");

            Vector3 pos = gameObject.transform.position;
            pos.y += 0.5f;
            obj.transform.position = pos;

            Vector3 rot = gameObject.transform.rotation.eulerAngles;
            rot.x = -90;
            obj.transform.rotation = Quaternion.Euler(rot);

            obj.SetActive(false);
            obj.SetActive(true);
            saveDamage = player.state.damage;
            player.state.damage = (player.state.damage + (player.state.damage * 0.15f));

        }
        else
        {
            Debug.Log("코스트(마나) 부족");
        }

    }
}
