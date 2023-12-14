using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkilType : SkillBase
{
    
    //�ڽſ��Ծ��°�
    //�����ΰ� �����ΰ�
    //� ������ �����ϴ°�
    //����Ʈ�� ����� ���°�
    //�������ΰ�
    //���ӽð�
    //�Ҹ� �ڽ�Ʈ
    //��Ÿ��

    //-����:����Ʈ ��� ��, 12�ʵ��� ���ݷ��� 15% �����Ѵ�.�ڽ�
    //-�̻级��:����Ʈ ��� ��, �����ð����� ���� ���� ������ �Ʊ��� ���ݷ� ���
    //-���ī��: ���ݼӵ� ����
    //-�޸���: ����Ʈ ��� ��, 12�ʵ��� ȸ�� �ӵ��� 1.5�� �����Ѵ� �ڽ�
    public enum buffType
    {
        damage,
        attackSpeed,
        AddCost,
    }
    //public enum IncrementalForm
    //{
    //    Percentage,
    //    Magnification,
    //}


    [SerializeField, Header("��ų ���ӽð�")]
    public float skillDuration;

    [SerializeField, Header("�ڽſ��� ���°�")]
    public bool isMe;

    [SerializeField, Header("�����ΰ�")]
    public bool isSingle;

    [SerializeField, Header("����Ʈ �̸��� �����ΰ�")]
    public string effectName;

    [SerializeField, Header("�����ؼ� ����ϴ°�")]
    public bool isChoice;

    [SerializeField, Header("� �ɷ�ġ�� �����ϴ°�")]
    public buffType type;

    [SerializeField, Header("���� ���(�ۼ�Ʈ or ����)")]
    public Defines.IncrementalForm Inc;

    [SerializeField, Header("�󸶳� ����(�����̸� �״��, �ۼ�Ʈ�� 1�� 100%)")]
    public float figure;

    private PlayerController player;
    private float timer;
    private float duration;
    private float saveDamage;
    private float saveSpeed;
    private GameObject obj;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillCoolTime;
        duration = 0f;
        isSkillUsing = false;
        saveSpeed = player.state.attackDelay;
        saveDamage = player.state.damage;

    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration) 
            {
                duration = 0f;
                isSkillUsing = false;
                player.state.attackDelay = saveSpeed;
                player.ani.speed = 1;
                player.state.damage = saveDamage;
                if(obj != null)
                {
                    obj.GetComponent<PoolAble>().ReleaseObject();

                }
            }

        }

    }
    public override void UseSkill()
    {
        if(player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch(skillType)
            {
                case Defines.SkillType.Auto:

                    break;
                case Defines.SkillType.Instant:
                    //��� ����->�ڽ� || �ֺ� �ٸ� ĳ����->� �ɷ�ġ ����-> ����� % ���� -> ����Ʈ ���� -> ����
                    InstantSkill();
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
    public void InstantSkill()
    {
        
        switch(type)
        {
            case buffType.attackSpeed:
                //����
                //�ִϸ��̼� ������
                InstantSkillAttackSpeedBuff();
                break;
            case buffType.damage:
                InstantSkillAttackDamageBuff();
                //���ݷ¸�����
                break;
            case buffType.AddCost:
                InstantSkillAddCost();
                break;
        }
        
    }

    public void InstantSkillAddCost()
    {

        PoolBuffEffactAll();
        var cos = GameObject.FindGameObjectWithTag("StageManager");
        cos.GetComponent<StageManager>().currentCost += player.rangeInPlayers.Count;
    }

    public void InstantSkillAttackSpeedBuff()
    {
        if(isMe)
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    player.state.attackDelay -= saveSpeed * figure;
                    player.ani.speed += 1 * figure;
                    PoolBuffEffact();
                    break;
                case Defines.IncrementalForm.Magnification:
                    player.state.attackDelay *= figure;
                    player.ani.speed = figure;
                    PoolBuffEffact();
                    break;
            }
        }
        else
        {

        }
    }
    public void InstantSkillAttackDamageBuff()
    {
        if (isMe)
        {
            switch (Inc)
            {
                case Defines.IncrementalForm.Percentage:
                    player.state.damage += saveDamage * figure;
                    PoolBuffEffact();
                    break;
                case Defines.IncrementalForm.Magnification:
                    player.state.damage *= figure;
                    PoolBuffEffact();
                    break;
            }
        }
        else
        {

        }
    }
    public void PoolBuffEffact()
    {
        obj = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pos = gameObject.transform.position;
        pos.y += 0.5f;
        obj.transform.position = pos;

        obj.SetActive(false);
        obj.SetActive(true);
    }
    public void PoolBuffEffactAll()
    {
        var obs = ObjectPoolManager.instance.GetGo(effectName);

        Vector3 pobs = gameObject.transform.position;
        pobs.y += 0.5f;
        obs.transform.position = pobs;

        obs.GetComponent<PoolAble>().ReleaseObject(3f);

        obs.SetActive(false);
        obs.SetActive(true);
        foreach (var a in player.rangeInPlayers)
        {
            var oh = ObjectPoolManager.instance.GetGo(effectName);

            Vector3 pos = a.GetComponentInParent<PlayerController>().gameObject.transform.position;
            pos.y += 0.5f;
            oh.transform.position = pos;

            oh.GetComponent<PoolAble>().ReleaseObject(3f);

            oh.SetActive(false);
            oh.SetActive(true);
        }
    }
}
