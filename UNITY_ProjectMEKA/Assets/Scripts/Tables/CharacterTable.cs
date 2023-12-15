using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

/*
	public

    - CharacterData GetCharacterData(int) : Dictionary<int, CharacterData> 에서 int 키 값 찾아서 리턴
    - Dictionary<int, CharacterData> GetOriginalTable() : Dictionary<int, CharacterData> 복사 생성해서 리턴
*/

public class CharacterTable : DataTable
{
	protected Dictionary<int, CharacterData> characterDict = new Dictionary<int, CharacterData>();

	public int Count
	{
		get
		{
			return characterDict.Count;
		}
	}

	public CharacterTable()
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
			var records = csv.GetRecords<CharacterData>();

			foreach (var record in records)
			{
				CharacterData temp = record;
				characterDict.Add(temp.CharacterID, temp);

				Debug.Log((temp.CharacterID, temp.CharacterName));
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public CharacterData GetCharacterData(int ID)
	{
		if (characterDict.ContainsKey(ID))
		{
			var data = characterDict[ID];
			return data;
		}
		return null;
	}

	public Dictionary<int, CharacterData> GetOriginalTable()
	{
		return new Dictionary<int, CharacterData>(characterDict);
	}
}
