using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MonsterLevelTable : DataTable
{
    protected Dictionary<int, MonsterLevelData> monsterDict = new Dictionary<int, MonsterLevelData>();

    public int Count
    {
        get
        {
            return monsterDict.Count;
        }
    }

    public MonsterLevelTable()
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
            var records = csv.GetRecords<MonsterLevelData>();

            foreach (var record in records)
            {
                MonsterLevelData temp = record;
                monsterDict.Add(temp.MonsterLevelID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public MonsterLevelData GetMonsterData(int ID)
    {
        if(monsterDict.ContainsKey(ID))
        {
            var data = monsterDict[ID];
            return data;
        }
        return null;
    }

    public Dictionary<int, MonsterLevelData> GetOriginalTable()
    {
        return new Dictionary<int, MonsterLevelData>(monsterDict);
    }
}
