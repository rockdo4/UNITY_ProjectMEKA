using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SnipingSkillType : SkillBase
{
    private PlayerController player;
    [SerializeField, Header("���ۼ�Ʈ ������")]
    public float figure;
    [SerializeField, Header("����� ����Ʈ �̸�")]
    public string effectName;
    private bool isSniping;
    private float timer;
    private void Start()
    {
        player = GetComponent<PlayerController>();
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
        if (player.state.cost >= player.state.skillCost && timer >= player.state.skillCoolTime)
        {
            timer = 0;
            player.state.cost -= skillCost;
            isSkillUsing = true;
            switch (skillType)
            {
                case Defines.SkillType.SnipingSingle:
                    SnipingSingle();
                    break;
                case Defines.SkillType.SnipingArea:

                    break;

            }
        }
    }
    public void SnipingSingle()
    {
        if (targetList.Any())
        {
            var s = FindObjectWithHighestHpRatio(targetList);

            //TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE 
            var e = s.GetComponent<EnemyController>();//TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE 
            e.stunTime = 3f;//TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE
            e.SetState(NPCStates.Stun);//TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE 
            //TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE TEST CODE 

            var obj = ObjectPoolManager.instance.GetGo(effectName);
            StartCoroutine(AttackDelay(s));
            Vector3 pos = s.gameObject.transform.position;
            pos.y += 0.5f;
            obj.transform.position = pos;
            obj.SetActive(false);
            obj.SetActive(true);
            obj.GetComponent<PoolAble>().ReleaseObject(3f);
            
        }
        Debug.Log("���̹� �̰� ��ų����");
    }
    IEnumerator AttackDelay(GameObject s)
    {
        yield return new WaitForSeconds(2f);
        s.GetComponent<IAttackable>().OnAttack(player.state.damage * figure);
        targetList.Clear();
    }
    public void SnipingArea()
    {

    }
    GameObject FindObjectWithHighestHpRatio(List<GameObject> list)
    {
        if (list == null || list.Count == 0)
            return null;

        GameObject highestHpRatioObject = list[0];
        float highestHpRatio = GetHpRatio(highestHpRatioObject.GetComponent<EnemyController>());

        foreach (var obj in list)
        {
            EnemyController health = obj.GetComponent<EnemyController>();
            if (health != null)
            {
                float hpRatio = GetHpRatio(health);
                if (hpRatio > highestHpRatio)
                {
                    highestHpRatio = hpRatio;
                    highestHpRatioObject = obj;
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
