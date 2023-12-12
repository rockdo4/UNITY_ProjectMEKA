using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DeviceOptionTable : DataTable
{
	protected Dictionary<int, DeviceOption> deviceCoreOptionDict = new Dictionary<int, DeviceOption>();
	protected Dictionary<int, DeviceOption> deviceEngineOptionDict = new Dictionary<int, DeviceOption>();
	protected Dictionary<int, DeviceOption> deviceSubOptionDict = new Dictionary<int, DeviceOption>();

	public DeviceOptionTable()
	{
		path = "Table/DeviceOptionTable";
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
				
				if(temp.PartType == 0)
				{
					deviceSubOptionDict.Add(temp.ID, temp);
				}
				else if(temp.StatType == 1)
				{
					deviceCoreOptionDict.Add(temp.ID, temp);
				}
				else if(temp.StatType == 2)
				{
					deviceEngineOptionDict.Add(temp.ID, temp);
				}
				else
				{
					Debug.LogError("잘못된 옵션 타입");
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public DeviceOption GetDeviceOptionData(int id)
	{
		if(deviceCoreOptionDict.ContainsKey(id))
		{
			return deviceCoreOptionDict[id];
		}
		else if(deviceEngineOptionDict.ContainsKey(id))
		{
			return deviceEngineOptionDict[id];
		}
		else if(deviceSubOptionDict.ContainsKey(id))
		{
			return deviceSubOptionDict[id];
		}
		else
		{
			Debug.LogError("잘못된 옵션 아이디");
			return null;
		}
	}

	public Dictionary<int, DeviceOption> GetOrigianlCoreTable()
	{
		return new Dictionary<int, DeviceOption>(deviceCoreOptionDict);
	}
	public Dictionary<int, DeviceOption> GetOrigianlEngineTable()
	{
		return new Dictionary<int, DeviceOption>(deviceEngineOptionDict);
	}
	public Dictionary<int, DeviceOption> GetOrigianlSubTable()
	{
		return new Dictionary<int, DeviceOption>(deviceSubOptionDict);
	}
}
