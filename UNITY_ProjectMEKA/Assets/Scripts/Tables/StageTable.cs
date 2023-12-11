using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StageTable : DataTable
{
    protected Dictionary<int, StageData> stageDict = new Dictionary<int, StageData>();
    public int Count
    {
        get
        {
            return stageDict.Count;
        }
    }
    public StageTable()
    {
        path = "Table/StageTable";
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
            var records = csv.GetRecords<StageData>();
            foreach (var record in records)
            {
                StageData temp = record;
                stageDict.Add(temp.StageID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public StageData GetStageData(int id)
    {
        if(stageDict.ContainsKey(id))
        {
            var data = stageDict[id];
            return data;
        }
        return null;
    }

    public Dictionary<int, StageData> GetOriginalTable()
    {
        return new Dictionary<int, StageData>(stageDict);
    }
}
