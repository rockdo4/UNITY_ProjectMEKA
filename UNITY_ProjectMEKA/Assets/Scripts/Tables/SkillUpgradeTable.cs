using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillUpgradeTable : DataTable
{
	protected List<SkillUpgradeData> skillUpgradeTable = new List<SkillUpgradeData>();
	public SkillUpgradeTable()
	{
		path = "Table/SkillUpgradeTable";
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
			var records = csv.GetRecords<SkillUpgradeData>();

			foreach (var record in records)
			{
				SkillUpgradeData temp = record;
				skillUpgradeTable.Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public SkillUpgradeData GetUpgradeData(int level)
	{
		if (level < 0 || level >= skillUpgradeTable.Count)
		{
			Debug.LogError("레벨 테이블 범위 초과");
			return null;
		}
		return skillUpgradeTable[level - 1];
	}

	public List<SkillUpgradeData> GetOriginalTable()
	{
		return new List<SkillUpgradeData>(skillUpgradeTable);
	}
}
