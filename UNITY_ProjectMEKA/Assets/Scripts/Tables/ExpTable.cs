using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

/*
	public

    - ExpData GetExpData(int) : Dictionary<int, ExpData> 에서 int 키 값 찾아서 리턴
    - Dictionary<int, ExpData> GetOriginalTable() : Dictionary<int, ExpData> 복사 생성해서 리턴
*/

public class ExpTable : DataTable
{
	protected List<ExpData> expList = new List<ExpData>();
	public ExpTable()
	{
		path = "Table/ExpTable";
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
			var records = csv.GetRecords<ExpData>();

			foreach (var record in records)
			{
				ExpData temp = record;
				expList.Add(temp);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public ExpData GetExpData(int level)
	{
		level--;
		if (level < 0 || level > expList.Count)
		{
			Debug.LogError("레벨 테이블 범위 초과");
			return null;
		}
		return expList[level];
	}

	public List<ExpData> GetOriginalTable()
	{
		return new List<ExpData>(expList);
	}
}
