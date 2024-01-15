using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class TutorialTable : DataTable
{
	protected List<List<TutorialData>> tutorialList = new List<List<TutorialData>>();

	public TutorialTable()
	{
		path = "Table/TutorialTable";
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
			var records = csv.GetRecords<TutorialData>();

			var list = new List<TutorialData>();
			int page = 0;

			foreach ( var record in records )
			{
				if(record.Page != page)
				{
					page = record.Page;
					list = new List<TutorialData>();
					tutorialList.Add(list);
				}
				list.Add(record);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.LogError("csv 로드 에러");
		}
	}

	public List<List<TutorialData>> GetOriginalTable()
	{
		return new List<List<TutorialData>>(tutorialList);
	}
}
