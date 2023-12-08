using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using System.Linq;

public class SynchroTable : DataTable
{
	//protected List<DropData> m_DropTableList = new List<DropData>();
	protected List<SynchroData> synchroDataList = new List<SynchroData>();

	public SynchroTable()
	{
		path = "Table/SynchroTable";
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
			var records = csv.GetRecords<SynchroData>();

			foreach (var record in records)
			{
				SynchroData temp = new SynchroData();

				temp = record;

				synchroDataList.Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public SynchroData GetSynchroData(int grade, int occupation)
	{
		var data = synchroDataList.Find(x => x.Grade == grade && x.Occupation == occupation);
		if(data != default || data != null)
		{
			return data;
		}
		return null;
	}

	public List<SynchroData> GetOriginalTable()
	{
		return new List<SynchroData>(synchroDataList);
	}
}
