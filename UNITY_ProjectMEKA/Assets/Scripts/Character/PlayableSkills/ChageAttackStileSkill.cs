using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageAttackStileSkill : SkillBase
{
    //��ų �������� 100%�� �� �� 10�� ���� �ڵ����� �����Ⱑ 2�� �������� �ٲ��, ���� ���� �������� ������.
    [SerializeField, Header("��ų ���� ����� �ִϸ��̼� ��Ʈ�ѷ�")]
    public RuntimeAnimatorController newAnimationController;

    [SerializeField, Header("��ų ���ӽð�")]
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
            //�ڵ�->��Ÿ��&&�ڽ�Ʈ->����
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
                    //��� ����->�ڽ� || �ֺ� �ٸ� ĳ����->� �ɷ�ġ ����-> ����� % ���� -> ����Ʈ ���� -> ����
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
