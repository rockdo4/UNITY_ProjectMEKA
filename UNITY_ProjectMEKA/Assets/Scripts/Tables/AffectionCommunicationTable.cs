using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CommuinicationDictionary
{
	//ĳ���� ��ȭ ���� ID, �� ID�� �ش��ϴ� ����Ʈ
	public Dictionary<int, List<CommunicationData>> idCommunicationList = new Dictionary<int, List<CommunicationData>>();
}

public class AffectionCommunicationTable : DataTable
{
	//ĳ���� <ID, ĳ���� ��ȭ ����>
	protected Dictionary<int, CommuinicationDictionary> characterCommunicationDict = new Dictionary<int, CommuinicationDictionary>();
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

				if (!characterCommunicationDict.ContainsKey(temp.CharacterID))
				{
					var dict = new CommuinicationDictionary();
					characterCommunicationDict.Add(temp.CharacterID, dict);
				}

				var charDict = characterCommunicationDict[temp.CharacterID];

				if (!charDict.idCommunicationList.ContainsKey(temp.ID)) 
				{
					var list = new List<CommunicationData>();
					charDict.idCommunicationList.Add(temp.ID, list);
                }

                charDict.idCommunicationList[temp.ID].Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv �ε� ����");
		}

		if(characterCommunicationDict == null)
		{
			Debug.Log("123");
		}
		else
		{
			Debug.Log("123");
		}

	}

	public CommuinicationDictionary GetAffectionData(int CharacterID)
	{
		if (!characterCommunicationDict.ContainsKey(CharacterID))
		{
			Debug.LogWarning("ĳ���Ͱ� ������ ��ȭ ����");
			return null;
		}
		return characterCommunicationDict[CharacterID];
	}
}
