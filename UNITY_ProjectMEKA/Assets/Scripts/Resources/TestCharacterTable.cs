using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class TestCharacter
{
	public int _ID { get; set; }
	public int _Level { get; set; }
	public int _Rare { get; set; }
	public string _Name { get; set; }
	public int _Weight { get; set; }
}

public class TestCharacterTable : DataTable
{
	//protected List<DropData> m_DropTableList = new List<DropData>();
	protected Dictionary<int, TestCharacter> testCharDict = new Dictionary<int, TestCharacter>();

	public TestCharacterTable()
	{
		path = "Table/CharacterTable";
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
			var records = csv.GetRecords<TestCharacter>();

			foreach (var record in records)
			{
				var temp = new TestCharacter();
				temp = record;
				testCharDict.Add(temp._ID, temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public TestCharacter GetMonsterData(int ID)
	{
		var data = testCharDict[ID];
		if (data == null)
		{
			return null;
		}
		return data;
	}
}
