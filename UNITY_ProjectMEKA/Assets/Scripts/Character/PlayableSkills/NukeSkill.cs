using UnityEngine;

public class NukeSkill : SkillBase
{
    public GameObject pre;
    private PlayerController player;
    private float timer;
    private Vector3 pos;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        isSkillUsing = false;
        player.ani.speed = 1;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
            
        }
    }
    private void OnEnable()
    {
        isSkillUsing = false;
        if(player != null) 
        {
            player.ani.speed = 1;

        }

    }
    private void Update()
    {
        if (isSkillUsing)
        {
            timer += Time.deltaTime;
            if(timer >= 4f)
            {
                player.ani.speed = 1;
            }
            if (timer >= 5f)
            {
                GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var ob in obj)
                {
                    if(ob.activeInHierarchy)
                    {
                        ob.GetComponent<IAttackable>().OnAttack(player.state.damage * 1.5f);
                    }
                }
                player.state.Hp = 0;
                isSkillUsing=false;
            }
        }
       
    }
    public override void UseSkill()
    {

        player.ani.SetTrigger("Skill");
        
        var nuke = Instantiate(pre, new Vector3(pos.x, 0.25f, pos.z+2f), Quaternion.identity);
        isSkillUsing = true;
        Destroy(nuke, 15f);
    }
    public void NUKE()
    {
        player.ani.speed = 0;
        
    }


}
