using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageAttackStileSkill : SkillBase
{
    //스킬 게이지가 100%가 될 때 10초 동안 자동으로 고유기가 2번 공격으로 바뀌며, 방어력 무시 데미지를 입힌다.
    [SerializeField, Header("스킬 사용시 변경될 애니매이션 컨트롤러")]
    public RuntimeAnimatorController newAnimationController;

    [SerializeField, Header("스킬 지속시간")]
    public float skillDuration;


    private RuntimeAnimatorController saveAnimationController;

    private PlayerController player;
    private float timer;
    private float duration;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
        duration = 0;
        isSkillUsing = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (skillType == Defines.SkillType.Auto && player.currentState != PlayerController.CharacterStates.Arrange)
        {
            UseSkill();
            //자동->쿨타임&&코스트->실행
        }
        if(isSkillUsing)
        {
            duration += Time.deltaTime;
            if(duration >= skillDuration)
            {
                duration = 0;
                isSkillUsing = false;
                player.ani.runtimeAnimatorController = saveAnimationController;

            }
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
                    saveAnimationController = player.ani.runtimeAnimatorController;
                    player.ani.runtimeAnimatorController = newAnimationController;
                    break;
                case Defines.SkillType.Instant:
                    //즉발 시전->자신 || 주변 다른 캐릭터->어떤 능력치 수정-> 계산방법 % 배율 -> 이팩트 생성 -> 적용
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

    public void SkillAttack()
    {
        var p = player.target.GetComponentInParent<IAttackable>();
        p.OnAttack(player.state.damage);
        Vector3 enemyPos = player.target.GetComponentInParent<EnemyController>().gameObject.transform.position;
        enemyPos.y += 0.5f;
        var obbj = ObjectPoolManager.instance.GetGo("EnemyHitEffect");
        obbj.transform.position = enemyPos;
        obbj.SetActive(false);
        obbj.SetActive(true);
        obbj.GetComponent<PoolAble>().ReleaseObject(1f);
    }
    public void NextAttack()
    {
        player.ani.SetTrigger("NextAttack");
    }
}
