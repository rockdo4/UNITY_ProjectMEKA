using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ProjectileTypeSkill : SkillBase
{
    [SerializeField, Header("�߻�ü �̸�")]
    public string projectileName;
    private PlayerController player;
    private float timer;
    private float duration;
    private GameObject obj;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillCoolTime;
        duration = 0;
        isSkillUsing = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        currentSkillTimer = timer;
        if (skillType == Defines.SkillType.Auto && player.currentState != PlayerController.CharacterStates.Arrange)
        {
            UseSkill();
            //�ڵ�->��Ÿ��&&�ڽ�Ʈ->����
        }
    }

    public override void UseSkill()
    {
        if (player.state.cost >= skillCost && timer >= skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            if(skillSoundName != null)
            {
                SoundManager.instance.PlayerSFXAudio(skillSoundName);
            }
            switch (skillType)
            {
                case Defines.SkillType.Auto:
                    player.ani.SetTrigger("Skill");
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
    public void SkillUse()
    {
        var s = ObjectPoolManager.instance.GetGo(projectileName);
        if (s == null)
        {
            isSkillUsing = false;
            return;
        }
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        s.transform.position = pos;
        s.transform.rotation = transform.rotation;
        s.GetComponent<EnergySlash>().player = player.gameObject;
        s.SetActive(false);
        s.SetActive(true);
        isSkillUsing = false;

    }

}
