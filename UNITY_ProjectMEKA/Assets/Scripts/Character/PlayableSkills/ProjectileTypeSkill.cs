using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ProjectileTypeSkill : SkillBase
{
    [SerializeField, Header("발사체 이름")]
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
            //자동->쿨타임&&코스트->실행
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
