using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableMgr
{
    private static Dictionary<System.Type, DataTable> tables = new Dictionary<System.Type, DataTable>();

    static DataTableMgr()
    {
        tables.Clear();

        //var testCharTable = new TestCharacterTable();
        //tables.Add(typeof(TestCharacterTable), testCharTable);

        var charTable = new CharacterTable();
        tables.Add(typeof(CharacterTable), charTable);

        if(charTable == null)
        {
			Debug.Log("null");
		}

        var expTable = new ExpTable();
        tables.Add(typeof(ExpTable), expTable);

        var characterLevelTable = new CharacterLevelTable();
        tables.Add(typeof(CharacterLevelTable), characterLevelTable);

        var itemTable = new ItemInfoTable();
        tables.Add(typeof(ItemInfoTable), itemTable);

        var synchroTable = new SynchroTable();
        tables.Add(typeof(SynchroTable), synchroTable);

        var stageTable = new StageTable();
        tables.Add(typeof(StageTable), stageTable);

        var rewarTable = new RewardTable();
        tables.Add(typeof(RewardTable), rewarTable);

        var deviceOptionTable = new DeviceOptionTable();
        tables.Add(typeof(DeviceOptionTable), deviceOptionTable);

        var deviceValueTable = new DeviceValueTable();
        tables.Add(typeof(DeviceValueTable), deviceValueTable);

        var deviceExpTable = new DeviceExpTable();
        tables.Add(typeof(DeviceExpTable), deviceExpTable);

        var skillTable = new SkillTable();
        tables.Add(typeof(SkillTable), skillTable);

		CharacterManager.Instance.InitCharacterStorage(charTable, characterLevelTable);
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
        {
            return null;
        }
        return tables[id] as T;
    }
}
