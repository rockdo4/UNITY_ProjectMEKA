using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterLevelTable : DataTable
{
	protected Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();
	public CharacterLevelTable()
	{
		path = "Table/CharacterLevelTable";
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
			var records = csv.GetRecords<LevelData>();

			foreach (var record in records)
			{
				LevelData temp = record;
				levelDict.Add(temp.CharacterLevelID, temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public LevelData GetLevelData(int ID)
	{
		if(levelDict.ContainsKey(ID))
		{
			return levelDict[ID];
		}
		return null;
	}

	public Dictionary<int, LevelData> GetOriginalTable()
	{
		return new Dictionary<int, LevelData>(levelDict);
	}
}
