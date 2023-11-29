using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummySkill : SkillBase
{
    public int cost = 10;

    public override void UseSkill()
    {
        Debug.Log("This is skill!");
    }
}
