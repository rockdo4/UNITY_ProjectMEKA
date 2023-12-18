using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillTable : DataTable
{
	
	protected Dictionary<int, SkillData[]> skillDictArr = new Dictionary<int, SkillData[]>();

	public SkillTable()
	{
		path = "Table/SkillTable";
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
			var records = csv.GetRecords<SkillData>();

			SkillData[] skillDatas = null;

			foreach (var record in records)
			{
				SkillData temp = record;
			
				if(temp.SkillLevel == 1)
				{
					skillDatas = new SkillData[temp.SkillMaxLevel];
				}

				skillDatas[temp.SkillLevel - 1] = temp;

				if(temp.SkillLevel == 6)
				{
					skillDictArr.Add(temp.SkillID, skillDatas);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public SkillData[] GetSkillData(int skillID)
	{
		if(skillDictArr.ContainsKey(skillID))
		{
			return skillDictArr[skillID];
		}
		else
		{
			Debug.LogError("스킬 아이디 없음");
			return null;
		}
	}

	public Dictionary<int, SkillData[]> GetOriginalTable()
	{
		return new Dictionary<int, SkillData[]>(skillDictArr);
	}
}
