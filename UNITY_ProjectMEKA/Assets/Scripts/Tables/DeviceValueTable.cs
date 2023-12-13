using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DeviceValueTable : DataTable
{
	protected Dictionary<int, DeviceOption> deviceValueDict = new Dictionary<int, DeviceOption>();

	public DeviceValueTable()
	{
		path = "Table/DeviceValueTable";
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
			var records = csv.GetRecords<DeviceOption>();

			foreach (var record in records)
			{
				DeviceOption temp = record;

				deviceValueDict.Add(temp.ID, temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public DeviceOption GetDeviceValueData(int id)
	{
		if (deviceValueDict.ContainsKey(id))
		{
			return deviceValueDict[id];
		}
		else
		{
			Debug.LogError("잘못된 옵션 아이디");
			return null;
		}
	}

	public Dictionary<int, DeviceOption> GetOrigianlValueTable()
	{
		return new Dictionary<int, DeviceOption>(deviceValueDict);
	}
}
