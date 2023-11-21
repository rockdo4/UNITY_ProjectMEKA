using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour//레벨업 가능
{
    CharacterState state;//이거 사용하면 능력치 정보 전부 접근가능
    private void Start()
    {
        state = GetComponent<CharacterState>();
    }
    private void Update()
    {
        
    }
    public void AddExp(int xp)
    {
       
    }

}
