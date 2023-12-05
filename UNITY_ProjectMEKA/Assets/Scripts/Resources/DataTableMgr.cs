using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableMgr
{
    private static Dictionary<System.Type, DataTable> tables = new Dictionary<System.Type, DataTable>();

    // 스태틱 생성자
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

        var levelTable = new LevelTable();
        tables.Add(typeof(LevelTable), levelTable);

        var itemTable = new ItemInfoTable();
        tables.Add(typeof(ItemInfoTable), itemTable);

		CharacterManager.Instance.InitCharacterStorage(charTable, levelTable);
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
