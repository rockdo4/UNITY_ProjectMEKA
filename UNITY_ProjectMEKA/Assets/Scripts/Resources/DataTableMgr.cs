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

        var testCharTable = new TestCharacterTable();
        tables.Add(typeof(TestCharacterTable) , testCharTable);

        //var itemTable = new ItemTable();
        //tables.Add(typeof(ItemTable), itemTable);
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
