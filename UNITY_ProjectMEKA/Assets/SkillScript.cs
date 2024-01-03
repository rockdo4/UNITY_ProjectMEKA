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
    public void SetSkill(int id, int level)
    {
        if (skillInfoTable == null)
            skillInfoTable = DataTableMgr.GetTable<SkillInfoTable>();

        var stringTable = DataTableMgr.GetTable<StringTable>();
        var datas = skillInfoTable.GetSkillDatas(id);

        var levelID = datas[level - 1].SkillLevelID;

        skillLevel.SetText(level.ToString());
        skillDescription.SetText(stringTable.GetString($"{levelID}_skillInfo"));
    }
}
