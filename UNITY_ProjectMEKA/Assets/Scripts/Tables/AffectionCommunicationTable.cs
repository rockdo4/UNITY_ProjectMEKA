using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;

public class AffectionCommunicationTable : DataTable
{
	protected Dictionary<int, List<CommunicationData>> communicationDictList = new Dictionary<int, List<CommunicationData>>();
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
				
				if(!communicationDictList.ContainsKey(temp.CharacterID))
				{
					var list = new List<CommunicationData>();

					communicationDictList.Add(temp.CharacterID, list);
					communicationDictList[temp.CharacterID].Add(temp);
				}
				else
				{
					communicationDictList[temp.CharacterID].Add(temp);
				}

				//Debug.Log(temp.Script);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public List<CommunicationData> GetAffectionData(int CharacterID)
	{
		if (communicationDictList.ContainsKey(CharacterID))
		{
			Debug.LogWarning("캐릭터가 보유한 대화 없음");
			return null;
		}
		return communicationDictList[CharacterID];
	}
}
