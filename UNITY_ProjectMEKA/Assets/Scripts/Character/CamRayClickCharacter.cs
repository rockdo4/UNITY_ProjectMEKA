using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRayClickCharacter : MonoBehaviour
{
    
    
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("PlayerCollider"))
                {
                    SkillBase usk = hit.collider.gameObject.GetComponentInParent<SkillBase>();
                    if(usk == null)
                    {
                        return;
                    }
                    PlayerController pl = hit.collider.gameObject.GetComponentInParent<PlayerController>();
                    if(pl != null)
                    {
                        if(pl.state.cost < 0)
                        {
                            pl.state.cost = 0;
                        }
                        else if(pl.state.cost >= pl.skillCost)
                        {
                            pl.state.cost -= pl.skillCost;

                            usk.UseSkill();

                        }

                    }
                }
            }
        }
       
        
    }
}
