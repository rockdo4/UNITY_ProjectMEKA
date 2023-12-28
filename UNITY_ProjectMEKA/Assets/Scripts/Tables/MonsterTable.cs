using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MonsterTable : DataTable
{
    protected Dictionary<int, MonsterData> monsterDict = new Dictionary<int, MonsterData>();

    public int Count
    {
        get
        {
            return monsterDict.Count;
        }
    }

    public MonsterTable()
    {
        path = "Table/MonsterTable";
        Load();
    }

    public override void Load()
    {
        var csvData = Resources.Load<TextAsset>(path);

        TextReader reader = new StringReader(csvData.text);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.HasHeaderRecord = true;

        var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            var records = csv.GetRecords<MonsterData>();

            foreach (var record in records)
            {
                MonsterData temp = record;
                monsterDict.Add(temp.MonsterID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public MonsterData GetMonsterData(int ID)
    {
        if (monsterDict.ContainsKey(ID))
        {
            var data = monsterDict[ID];
            return data;
        }
        return null;
    }

    public Dictionary<int, MonsterData> GetOriginalTable()
    {
        return new Dictionary<int, MonsterData>(monsterDict);
    }

}
