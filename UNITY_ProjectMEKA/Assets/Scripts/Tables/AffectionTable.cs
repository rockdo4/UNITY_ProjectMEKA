using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class AffectionTable : DataTable
{
	protected List<AffectionData> affectionList = new List<AffectionData>();
	public AffectionTable()
	{
		path = "Table/AffectionTable";
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
			var records = csv.GetRecords<AffectionData>();

			foreach (var record in records)
			{
				AffectionData temp = record;
				affectionList.Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public AffectionData GetAffectionData(int level)
	{
		level--;
		if (level < 0 || level >= affectionList.Count)
		{
			Debug.LogWarning("호감도 테이블 범위 초과");
			return null;
		}
		return affectionList[level];
	}

	public List<AffectionData> GetOriginalTable()
	{
		return new List<AffectionData>(affectionList);
	}
}
