using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RYUSIENSkill : SkillBase
{
    private PlayerController player;
    public bool isSkill = false;
    private Dictionary<GameObject, float> distancePlayer;

    //�ñ׸� �������� ���� ä������, ���� ���� ��ġ�Ͽ� ���� Ÿ�Ͽ� ��ġ�� ĳ���� �� �� ���� �����ϰ� ���� �ο��Ѵ�
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
            //����
        }
    }
    public override void UseSkill()
    {
        isSkill = true;
        //Time.timeScale = 0.2f;
    }

    
}
