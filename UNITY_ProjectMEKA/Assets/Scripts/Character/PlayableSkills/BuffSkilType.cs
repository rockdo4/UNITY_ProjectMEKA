using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkilType : SkillBase
{
    
    //자신에게쓰는가
    //단일인가 광역인가
    //어떤 스텟을 변경하는가
    //이팩트는 어떤것을 쓰는가
    //선택형인가
    //지속시간
    //소모 코스트
    //쿨타임

    //-엠버:버스트 사용 시, 12초동안 공격력이 15% 증가한다.자신
    //-이사벨라:버스트 사용 시, 일정시간동안 범위 내에 지정한 아군의 공격력 상승
    //-쿠로카미: 공격속도 증가
    //-메리아: 버스트 사용 시, 12초동안 회복 속도가 1.5배 증가한다 자신
    public enum buffType
    {
        damage,
        attackSpeed,
    }
    //public enum IncrementalForm
    //{
    //    Percentage,
    //    Magnification,
    //}


    [SerializeField, Header("스킬 지속시간")]
    public float skillDuration;

    [SerializeField, Header("자신에게 쓰는가")]
    public bool isMe;

    [SerializeField, Header("단일인가")]
    public bool isSingle;

    [SerializeField, Header("이팩트 이름은 무엇인가")]
    public string effectName;

    [SerializeField, Header("선택해서 사용하는가")]
    public bool isChoice;

    [SerializeField, Header("어떤 능력치를 변경하는가")]
    public buffType type;

    [SerializeField, Header("증가 방식(퍼센트 or 배율)")]
    public Defines.IncrementalForm Inc;

    [SerializeField, Header("얼마나 증가(배율이면 그대로, 퍼세트면 1이 100%)")]
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
                obj.GetComponent<PoolAble>().ReleaseObject();
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
                    //즉발 시전->자신 || 주변 다른 캐릭터->어떤 능력치 수정-> 계산방법 % 배율 -> 이팩트 생성 -> 적용
                    InstantSkill();
                    break;
                case Defines.SkillType.SnipingSingle:
                    //선택된 놈이 넘어올것
                    break;
                case Defines.SkillType.SnipingArea:
                    //선택된 영역의 놈들이 넘어올것
                    break;
            }
        }
    }
    public void InstantSkill()
    {
        
        switch(type)
        {
            case buffType.attackSpeed:
                //공속
                //애니매이션 빠르게
                InstantSkillAttackSpeedBuff();
                break;
            case buffType.damage:
                InstantSkillAttackDamageBuff();
                //공격력만증가
                break;
        }
        
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

        //Vector3 rot = gameObject.transform.rotation.eulerAngles;
        //rot.x = -90;
        //obj.transform.rotation = Quaternion.Euler(rot);

        obj.SetActive(false);
        obj.SetActive(true);
    }
}
