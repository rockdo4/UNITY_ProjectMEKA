using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IYRASkill : SkillBase
{
    //�ñ׸� �������� �������� �ڵ����
    //��Ÿ��10��
    //�Ҹ� �ñ׸�50
    //���������� ������� Ÿ��
    private PlayerController player;
    private float timer;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = player.state.skillCoolTime;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime && player.currentState != PlayerController.CharacterStates.Arrange)
        {
            timer = 0;
            player.state.cost -= player.state.skillCost;
            UseSkill();
        }
    }

    public void SkillUse()
    {
        var s = ObjectPoolManager.instance.GetGo("Slash");
        if (s == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        pos.y += 0.5f;
        s.transform.position = pos;
        s.transform.rotation = transform.rotation;
        s.GetComponent<EnergySlash>().player = player.gameObject;
        s.SetActive(false);
        s.SetActive(true);
        
    }

    public override void UseSkill()
    {
        player.ani.SetTrigger("Skill");
    }
}
