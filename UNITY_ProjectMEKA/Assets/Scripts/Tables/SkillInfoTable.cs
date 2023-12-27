using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillInfoTable : DataTable
{
	protected Dictionary<int, SkillInfo[]> skillDictArr = new Dictionary<int, SkillInfo[]>();

	public SkillInfoTable()
	{
		path = "Table/SkillInfoTable";
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
			var records = csv.GetRecords<SkillInfo>();

			SkillInfo[] skillDatas = null;

			foreach (var record in records)
			{
				SkillInfo temp = record;

				if (temp.SkillLevel == 1)
				{
					skillDatas = new SkillInfo[temp.SkillMaxLevel];
				}

				skillDatas[temp.SkillLevel - 1] = temp;

				if (temp.SkillLevel == 6)
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

	public SkillInfo[] GetSkillDatas(int skillID)
	{
		if (skillDictArr.ContainsKey(skillID))
		{
			return skillDictArr[skillID];
		}
		else
		{
			Debug.LogError("스킬 아이디 없음");
			return null;
		}
	}

	public Dictionary<int, SkillInfo[]> GetOriginalTable()
	{
		return new Dictionary<int, SkillInfo[]>(skillDictArr);
	}
}
