using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StringTable : DataTable
{
    protected Dictionary<string, StringData> stringDict = new Dictionary<string, StringData>();

    public int Count
    {
        get
        {
            return stringDict.Count;
        }
    }

    public StringTable()
    {
        path = "Table/StringTable";
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
            var records = csv.GetRecords<StringData>();
            foreach (var record in records)
            {
                StringData temp = record;
                stringDict.Add(temp.ID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv 로드 에러");
        }
    }

    public string GetString(string key)
    {
        if(stringDict.ContainsKey(key))
        {
            var data = stringDict[key];
            if(StageDataManager.Instance.language == Defines.Language.Kor)
            {
                return data.KOR;
            }
            else
            {
                return data.ENG;
            }
        }
        return null;
    }

    public Dictionary<string, StringData> GetOriginalTable()
    {
        return new Dictionary<string, StringData>(stringDict);
    }
}
