using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ISABELLASkill : SkillBase
{
    private PlayerController player;
    private float timer;
    private float skillDuration;
    private bool isSkill;
    private List<GameObject> buffPlayers;
    private Dictionary<string, float> savePlayersDamage;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        isSkill = false;
        savePlayersDamage = new Dictionary<string, float>();
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
                foreach (var player in savePlayersDamage.Keys)
                {
                    var pl = GameObject.Find(player).GetComponent<PlayerController>();
                    if (pl != null)
                    {
                        pl.state.damage = savePlayersDamage[player];
                    }
                }
            }
        }
    }

    public override void UseSkill()
    {
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
        {
            //����Ʈ ��� ��, 20�� ���� ���� ���� ������ �Ʊ��� ���ݷ� 1.5�� ���

            timer = 0;
            isSkill = true;
            foreach(var a in player.rangeInPlayers)
            {
                if(a.activeInHierarchy && a!=null)
                {
                    var pl = a.GetComponentInParent<PlayerController>();
                    savePlayersDamage.Add(pl.name, pl.state.damage);
                    pl.state.damage *= 1.5f;
                    //����Ʈ ���� IsabellaSkillEffect
                    var par = ObjectPoolManager.instance.GetGo("IsabellaSkillEffect");

                    Vector3 pos = pl.transform.position;
                    pos.y += 0.5f;
                    par.transform.position = pos;

                    Vector3 rot = pl.transform.rotation.eulerAngles;
                    rot.x = -90;
                    par.transform.rotation = Quaternion.Euler(rot);;

                    par.SetActive(false);
                    par.SetActive(true);
                    par.GetComponent<PoolAble>().ReleaseObject(player.state.skillDuration);
                }
            }
        }
    }

    
}
