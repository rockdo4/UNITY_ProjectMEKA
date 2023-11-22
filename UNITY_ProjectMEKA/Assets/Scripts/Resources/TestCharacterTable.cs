using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/*
	public

    - TestCharacterInfo GetCharacterData(int) : Dictionary<int, TestCharacterInfo> 에서 int 키 값 찾아서 리턴
    - Dictionary<int, TestCharacterInfo> GetOriginalTable() : Dictionary<int, TestCharacterInfo> 복사 생성해서 리턴
*/

public class TestCharacterTable : DataTable
{
	//protected List<DropData> m_DropTableList = new List<DropData>();
	protected Dictionary<int, TestCharacterInfo> testCharDict = new Dictionary<int, TestCharacterInfo>();

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
				TestCharacter temp = new TestCharacterInfo();

				temp.ID = record.ID;
				temp.Level = record.Level;
				temp.Rare = record.Rare;
				temp.Name = record.Name;
				temp.Weight	= record.Weight;

				testCharDict.Add(temp.ID, temp as TestCharacterInfo);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public TestCharacterInfo GetCharacterData(int ID)
	{
		var data = testCharDict[ID];
		if (data == null)
		{
			return null;
		}
		return data;
	}

	public Dictionary<int, TestCharacterInfo> GetOriginalTable()
	{
		return new Dictionary<int, TestCharacterInfo>(testCharDict);
	}
}
