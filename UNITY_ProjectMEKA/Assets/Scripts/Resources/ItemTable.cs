using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/*
	public

    - Item GetItemData(int) : Dictionary<int, Item> ���� int Ű �� ã�Ƽ� ����
    - Dictionary<int, Item> GetOriginalTable() : Dictionary<int, Item> ���� �����ؼ� ����
*/

public class ItemTable : DataTable
{
	//protected List<DropData> m_DropTableList = new List<DropData>();
	protected Dictionary<int, Item> itemDict = new Dictionary<int, Item>();

	public ItemTable()
	{
		path = "Table/ItemTable";
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
			var records = csv.GetRecords<Item>();

			foreach (var record in records)
			{
				Item temp = new Item();
				temp = record;
				itemDict.Add(temp.ID, temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv �ε� ����");
		}
	}

	public Item GetItemData(int ID)
	{
		if (itemDict.ContainsKey(ID))
		{
			var data = itemDict[ID];
			return data;
		}
		return null;
	}

	public Dictionary<int, Item> GetOriginalTable()
	{
		return new Dictionary<int, Item>(itemDict);
	}
}
