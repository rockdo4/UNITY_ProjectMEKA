using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/*
	public

    - ItemInfo GetItemData(int) : Dictionary<int, ItemInfo> ���� int Ű �� ã�Ƽ� ����
    - Dictionary<int, ItemInfo> GetOriginalTable() : Dictionary<int, ItemInfo> ���� �����ؼ� ����
*/

public class ItemInfoTable : DataTable
{
	//protected List<DropData> m_DropTableList = new List<DropData>();
	protected Dictionary<int, ItemInfo> itemDict = new Dictionary<int, ItemInfo>();

	public int Count
	{
		get
		{
			return itemDict.Count;
		}
	}

	public ItemInfoTable()
	{
		path = "Table/ItemInfoTable";
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
			var records = csv.GetRecords<ItemInfo>();

			foreach (var record in records)
			{
				ItemInfo temp = record;
				itemDict.Add(temp.ID, temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv �ε� ����");
		}
	}

	public ItemInfo GetItemData(int ID)
	{
		if (itemDict.ContainsKey(ID))
		{
			var data = itemDict[ID];
			return data;
		}
		return null;
	}

	public Dictionary<int, ItemInfo> GetOriginalTable()
	{
		return new Dictionary<int, ItemInfo>(itemDict);
	}
}
