using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RYUSIENSkill : SkillBase
{
    private PlayerController player;
    public bool isSkill = false;
    private Dictionary<GameObject, float> distancePlayer;

    //시그마 게이지가 전부 채워지면, 리우 셴을 터치하여 현재 타일에 배치된 캐릭터 중 한 명을 선택하고 방어막을 부여한다
    public void Start()
    {
        player = GetComponent<PlayerController>();
        isSkill = false;
        distancePlayer = new Dictionary<GameObject, float>();   
    }
    public void Update()
    {
        if (isSkill)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    int layerMask = LayerMask.GetMask("PlayerCollider");


            //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            //    {
            //        Debug.Log("Hit " + hit.transform.name);
            //        isSkill = false;
            //        Time.timeScale = 1.0f;
            //    }

            //}
            //보류
        }
    }
    public override void UseSkill()
    {
        isSkill = true;
        //Time.timeScale = 0.2f;
    }

    
}
