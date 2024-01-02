using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillScript : MonoBehaviour
{
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillDescription;
    private SkillInfoTable skillInfoTable;

    private void Start()
    {
        skillInfoTable = DataTableMgr.GetTable<SkillInfoTable>();
    }
    public void SetSkill(int id)
    {
        
    }
}
