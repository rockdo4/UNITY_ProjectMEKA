using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSkillType : SkillBase
{
    public enum BattingEffect
    {
        Stun,
        Damage,
        DamageAndAttackDamageUp,

    }
    [SerializeField, Header("����Ʈ �̸�")]
    public string effectName;

    [SerializeField, Header("Ÿ�� ȿ��")]
    public BattingEffect effect;

    [SerializeField, Header("Ÿ���� ���ݷ� ������ �ִ°�")]
    public bool isDamageUp;

    [SerializeField, Header("���� ��� �����ΰ�")]
    public bool isSingle;

    [SerializeField, Header("��ų ���ӽð�")]
    public float skillDuration;

    [SerializeField, Header("���� ���(�ۼ�Ʈ or ����)")]
    public Defines.IncrementalForm inc;

    [SerializeField, Header("�󸶳� ����(�����̸� �״��, �ۼ�Ʈ�� 1�� 100%)")]
    public float figure;


    //���� ȿ�� ��������, �ܼ� ����������
    //���ϰ��� �ΰ�
    private float saveDamage;
    private float saveAttackDelay;
    private float timer;
    private float duration;
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        saveDamage = player.state.damage;
        saveAttackDelay = player.state.attackDelay;
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
                player.state.damage = saveDamage;
                player.state.attackDelay = saveAttackDelay;
            }
        }
    }
    public override void UseSkill()
    {
        if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.Auto:
                    
                    break;
                case Defines.SkillType.Instant:
                    player.ani.SetTrigger("Skill");
                    if(isDamageUp)
                    {
                        DamageUp();
                    }
                    break;
                case Defines.SkillType.SnipingSingle:
                    //���õ� ���� �Ѿ�ð�
                    break;
                case Defines.SkillType.SnipingArea:
                    //���õ� ������ ����� �Ѿ�ð�
                    break;
            }
        }
        
    }
    public void DamageUp()
    {
        switch(inc)
        {
            case Defines.IncrementalForm.Magnification:
                player.state.damage *= figure;
                break;
            case Defines.IncrementalForm.Percentage:
                player.state.damage = saveDamage + (saveDamage * figure);
                break;
        }
        
    }
    public void Stunnig()
    {
        var obj = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pos = player.transform.position + player.transform.forward * 1f;
        pos.y += 0.5f; // y�� ��ġ ����

        obj.transform.position = pos;
        
        obj.SetActive(false);
        obj.SetActive(true);
        obj.GetComponent<PoolAble>().ReleaseObject(2f);

        switch(effect)
        {
            case BattingEffect.Damage:

                break;
            case BattingEffect.Stun:
                StunAttack();
                break;
        }

        foreach (var p in player.rangeInEnemys)
        {
            p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
        }

    }
    public void PalaSkillAttack()
    {
        var t = player.target.GetComponentInParent<IAttackable>();
        t.OnAttack(saveDamage * 1.2f);
    }
    public void StunAttack()
    {
        if(isSingle)
        {
            foreach (var p in player.rangeInEnemys)
            {
                p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
                return;
            }
        }
        else
        {
            foreach (var p in player.rangeInEnemys)
            {
                p.GetComponentInParent<EnemyController>().SetState(NPCStates.Stun);
            }
        }
       
    }

}
