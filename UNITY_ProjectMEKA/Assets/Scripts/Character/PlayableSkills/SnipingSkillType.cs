using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Defines;
using System;
using UnityEngine.Rendering;

public class SnipingSkillType : SkillBase
{

    public enum SkillType
    {
        Attack,
        PlayerDamageUp,
        Shield,
    }

    private PlayerController player;
    [SerializeField, Header("몇퍼센트 데미지")]
    public float figure;
    [SerializeField, Header("사용할 이팩트 이름")]
    public string effectName;
    [SerializeField, Header("스킬 지속시간")]
    public float duration;
    
    [SerializeField, Header("어떤 스킬(공격, 버프 등등")]
    public SkillType skillT;
    private bool isSniping;
    private float timer;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillCoolTime;
        ConvertTo2DArray();
    }
    private void OnEnable()
    {
        isSniping = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        
    }
    public override void UseSkill()
    {
        
        switch (skillType)
        {
            case Defines.SkillType.SnipingSingle:
                SnipingSingle();
                break;
            case Defines.SkillType.SnipingArea:
                SnipingArea();
                break;

        }
        
    }
    public void SnipingArea()
    {
        if (targetList.Any())
        {
            if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
            {
                timer = 0;
                player.state.cost -= skillCost;
                isSkillUsing = true;
                switch (skillT)
                {
                    case SkillType.Attack:
                        AreaSnipingAttack();
                        break;
                    case SkillType.PlayerDamageUp:
                        break;
                    case SkillType.Shield:
                        break;
                }
            }
        }
    }
    public void AreaSnipingAttack()
    {
        attackableTiles = player.attackableSkillTiles;
        var obj = ObjectPoolManager.instance.GetGo(effectName);
        var pos = attackableTiles[4].transform.position;
        pos.x += 0.5f;
        pos.z += 0.5f;
        
        obj.transform.position = pos;
        obj.SetActive(false);
        obj.SetActive(true);
        
    }
    IEnumerator Damage5Second()
    {
        while(true) 
        {
            yield return null;

        }
    }
    public void SnipingSingle()
    {
        if (targetList.Any())
        {
            if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
            {
                timer = 0;
                player.state.cost -= skillCost;
                isSkillUsing = true;
                switch(skillT)
                {
                    case SkillType.Attack:
                        SingleSnipingAttack();
                        break;
                    case SkillType.PlayerDamageUp:
                        SingleSnipingPlayerDamageUp();
                        break;
                    case SkillType.Shield:
                        SingleSnipingPlayerShield();
                        break;
                }
                
            }
        }
        Debug.Log("쒸이벌 이게 스킬이지");
    }
    
    public void ShieldSkill()
    {
        foreach (var a in targetList)
        {
            if (a.tag == "Player")
            {
                var c = a.GetComponent<PlayerController>();
                c.state.shield = figure;
                var obj = ObjectPoolManager.instance.GetGo(effectName);
                obj.transform.position = c.gameObject.transform.position;
                obj.GetComponent<Shield>().player = c;
                obj.SetActive(false);
                obj.SetActive(true);
                isSkillUsing = false;
            }
        }
    }
    public void SingleSnipingPlayerShield()
    {
        player.ani.SetTrigger("Skill");
    }
    public void SingleSnipingPlayerDamageUp()
    {
        foreach(var a in targetList)
        {
            if(a.tag == "Player")
            {
                var pl = a.GetComponent<PlayerController>();
                StartCoroutine(UpState(pl.state.damage, pl));
                pl.state.damage *= figure;
                var ob = ObjectPoolManager.instance.GetGo(effectName);
                ob.transform.position = pl.gameObject.transform.position;
                ob.SetActive(false);
                ob.SetActive(true);
                ob.GetComponent<PoolAble>().ReleaseObject(duration);
                return;
            }
        }
    }
    IEnumerator UpState(float originState,PlayerController player)
    {
        yield return new WaitForSeconds(duration);
        player.state.damage = originState;
        isSkillUsing = false;
    }
    public void SingleSnipingAttack()
    {
        var s = FindObjectWithHighestHpRatio(targetList);
        var e = s.GetComponent<EnemyController>();
        var obj = ObjectPoolManager.instance.GetGo(effectName);
        obj.GetComponent<FallowTarget>().target = e.gameObject.transform;
        StartCoroutine(AttackDelay(s));
        Vector3 pos = s.gameObject.transform.position;
        pos.y += 0.5f;
        obj.transform.position = pos;
        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(3f);
    }
    IEnumerator AttackDelay(GameObject s)
    {
        yield return new WaitForSeconds(2f);
        s.GetComponent<IAttackable>().OnAttack(player.state.damage * figure);
        targetList.Clear();
        isSkillUsing = false;
    }
    
    GameObject FindObjectWithHighestHpRatio(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }

        GameObject highestHpRatioObject = null;
        float highestHpRatio = 0;

        foreach (var obj in list)
        {
            if (obj.tag == "Enemy")
            {
                EnemyController health = obj.GetComponent<EnemyController>();
                if (health != null)
                {
                    if (highestHpRatioObject == null)
                    {
                        highestHpRatioObject = obj;
                        highestHpRatio = GetHpRatio(health);
                    }
                    else
                    {
                        float hpRatio = GetHpRatio(health);
                        if (hpRatio > highestHpRatio)
                        {
                            highestHpRatio = hpRatio;
                            highestHpRatioObject = obj;
                        }
                    }
                }
            }
        }

        return highestHpRatioObject;
    }

    float GetHpRatio(EnemyController health)
    {
        if (health.state.maxHp > 0)
            return (float)health.state.Hp / health.state.maxHp;
        return 0;
    }
}
