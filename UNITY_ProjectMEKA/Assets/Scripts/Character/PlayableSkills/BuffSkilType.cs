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

    private PlayerController player;
    private float timer;
    private float duration;


    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillDuration;
        duration = 0f;
    }
    private void Update()
    {
        
    }
    public override void UseSkill()
    {
    }
}
