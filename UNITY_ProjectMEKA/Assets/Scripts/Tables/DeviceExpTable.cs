using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DeviceExpTable : DataTable
{
	protected List<DeviceExpData> expList = new List<DeviceExpData>();
	public DeviceExpTable()
	{
		path = "Table/DeviceExpTable";
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
			var records = csv.GetRecords<DeviceExpData>();

			foreach (var record in records)
			{
				DeviceExpData temp = record;
				expList.Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public DeviceExpData GetExpData(int level)
	{
		level--;
		if (level < 0 || level > expList.Count)
		{
			Debug.LogError("레벨 테이블 범위 초과");
			return null;
		}
		return expList[level];
	}

	public List<DeviceExpData> GetOriginalTable()
	{
		return new List<DeviceExpData>(expList);
	}
}
