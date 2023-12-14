using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class RewardTable : DataTable
{
    protected Dictionary<int, RewardData> rewardDict = new Dictionary<int, RewardData>();
    public int Count
    {
        get
        {
            return rewardDict.Count;
        }
    }
    public RewardTable()
    {
        path = "Table/RewardTable";
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
            var records = csv.GetRecords<RewardData>();
            foreach (var record in records)
            {
                RewardData temp = record;
                rewardDict.Add(temp.RewardID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public RewardData GetStageData(int id)
    {
        if (rewardDict.ContainsKey(id))
        {
            var data = rewardDict[id];
            return data;
        }
        return null;
    }

    public Dictionary<int, RewardData> GetOriginalTable()
    {
        return new Dictionary<int, RewardData>(rewardDict);
    }
}
