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
	public Dictionary<int, List<CommunicationData>> idCommunicationList;
}

//public class AffectionCommunicationTable : DataTable
//{
//	//ĳ���� <ID, ĳ���� ��ȭ ����>
//	protected Dictionary<int, CommuinicationDictionary> characterCommunicationDict = new Dictionary<int, CommuinicationDictionary>();
//	public AffectionCommunicationTable()
//	{
//		path = "Table/AffectionCommunicationTable";
//		Load();
//	}

//	public override void Load()
//	{
//		var csvData = Resources.Load<TextAsset>(path);

//		TextReader reader = new StringReader(csvData.text);

//		var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
//		csvConfiguration.HasHeaderRecord = true;

//		var csv = new CsvReader(reader, csvConfiguration);

//		try
//		{
//			var records = csv.GetRecords<CommunicationData>();

//			foreach (var record in records)
//			{
//				CommunicationData temp = record;

//				if (!characterCommunicationDict.ContainsKey(temp.CharacterID))
//				{
//					var list = new CommuinicationDictionary();
//					characterCommunicationDict.Add(temp.CharacterID, list);
//				}

//				if (!characterCommunicationDict[temp.CharacterID].Exists(x => x.key == temp.ID))
//				{
//					var data = new CommuinicationDictionary();
//					data.key = temp.ID;
//					data.dataList = new List<CommunicationData>();
//					data.dataList.Add(temp);
//				}
//				else
//				{
//					characterCommunicationDict[temp.CharacterID].Find(x => x.key == temp.ID).dataList.Add(temp);
//				}
//				Debug.Log((temp.ID, temp.Script));
//				//Debug.Log(temp.Script);
//			}
//		}
//		catch (Exception ex)
//		{
//			Debug.Log(ex.Message);
//			Debug.LogError("csv �ε� ����");
//		}
//	}

//	public List<CommuinicationDictionary> GetAffectionData(int CharacterID)
//	{
//		if (characterCommunicationDict.ContainsKey(CharacterID))
//		{
//			Debug.LogWarning("ĳ���Ͱ� ������ ��ȭ ����");
//			return null;
//		}
//		return characterCommunicationDict[CharacterID];
//	}
//}
