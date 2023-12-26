using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class AffectionCommunicationTable : DataTable
{
	protected List<CommunicationData> communicationList = new List<CommunicationData>();
	public AffectionCommunicationTable()
	{
		path = "Table/AffectionCommunicationTable";
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
			var records = csv.GetRecords<CommunicationData>();

			foreach (var record in records)
			{
				CommunicationData temp = record;
				communicationList.Add(temp);
				Debug.Log(temp.Script);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public CommunicationData GetAffectionData(int list)
	{
		list--;
		if (list < 0 || list >= communicationList.Count)
		{
			Debug.LogWarning("대화 테이블 범위 초과");
			return null;
		}
		return communicationList[list];
	}

	public List<CommunicationData> GetOriginalTable()
	{
		return new List<CommunicationData>(communicationList);
	}
}
