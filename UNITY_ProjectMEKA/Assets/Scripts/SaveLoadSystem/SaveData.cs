//using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public abstract class SaveData
{
    public int Version { get; set; }
	public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;
    }
    public bool IsFirstGame { get; set; } = true;
    public float BGMVolume { get; set; }
    public float SEVolume { get; set; }
    public float MasterVolume { get; set; }
    public bool IsMasterVolumMute { get; set; }
    public bool IsBGMVolumMute { get; set; }
    public bool IsSEVolumMute { get; set; }

	public override SaveData VersionUp()
    {
        var data = new SaveDataV2()
        {
			IsFirstGame = IsFirstGame,
			BGMVolume = BGMVolume,
			SEVolume = SEVolume,
			MasterVolume = MasterVolume,
			IsMasterVolumMute = IsMasterVolumMute,
			IsBGMVolumMute = IsBGMVolumMute,
			IsSEVolumMute = IsSEVolumMute
		};
        //data.formationList = new List<int[]>
        //{
        //    new int[8],
        //    new int[8],
        //    new int[8],
        //    new int[8],
        //};

        return null;
    }
}

public class SaveDataV2 : SaveDataV1
{
	public SaveDataV2()
    {
		Version = 2;
	}

    public List<int[]> formationList { get; set; } = new List<int[]> 
    { 
        new int[8],
		new int[8],
		new int[8],
		new int[8],
    };

	public override SaveData VersionUp()
    {
        return null;
	}
}