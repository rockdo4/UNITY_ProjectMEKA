//using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        data.formationList = new List<int[]>
        {
            new int[8],
            new int[8],
            new int[8],
            new int[8],
        };
        data.characterStorage = new Dictionary<int, Character>();

        return null;
    }
}

public class SaveDataV2 : SaveDataV1
{
	public SaveDataV2()
    {
		Version = 2;
        formationList = new List<int[]>();
		characterStorage = new Dictionary<int, Character>();
	}

    public List<int[]> formationList;
    public Dictionary<int, Character> characterStorage;

	public override SaveData VersionUp()
    {
		var data = new SaveDataV3()
        {
			IsFirstGame = IsFirstGame,
			BGMVolume = BGMVolume,
			SEVolume = SEVolume,
			MasterVolume = MasterVolume,
			IsMasterVolumMute = IsMasterVolumMute,
			IsBGMVolumMute = IsBGMVolumMute,
			IsSEVolumMute = IsSEVolumMute
		};
		data.formationList = formationList;
		data.characterStorage = characterStorage;

		//�߰�

		data.itemStorage = new List<Item>();

		return data;
	}
}

public class SaveDataV3 : SaveDataV2
{
	public SaveDataV3()
    {
		Version = 3;
		itemStorage = new List<Item>();
	}

	public List<Item> itemStorage;

	public override SaveData VersionUp()
    {
        var data = new SaveDataV4()
        {
            IsFirstGame = IsFirstGame,
            BGMVolume = BGMVolume,
            SEVolume = SEVolume,
            MasterVolume = MasterVolume,
            IsMasterVolumMute = IsMasterVolumMute,
            IsBGMVolumMute = IsBGMVolumMute,
            IsSEVolumMute = IsSEVolumMute
        };
        data.formationList = formationList;
        data.characterStorage = characterStorage;
        data.itemStorage = itemStorage;

        // add
        data.storyStageDatas = new Dictionary<int, StageSaveData>();
        data.assignmentStageDatas = new Dictionary<int, StageSaveData>();
        data.challengeStageDatas = new Dictionary<int, StageSaveData>();

        return data;
    }
}

public class SaveDataV4 : SaveDataV3
{
	public SaveDataV4()
	{
		Version = 4;
		storyStageDatas = new Dictionary<int, StageSaveData>();
		assignmentStageDatas = new Dictionary<int, StageSaveData>();
		challengeStageDatas = new Dictionary<int, StageSaveData>();
    }

	public Dictionary<int,StageSaveData> storyStageDatas;
	public Dictionary<int, StageSaveData> assignmentStageDatas;
	public Dictionary<int, StageSaveData> challengeStageDatas;

    public override SaveData VersionUp()
    {
        var data = new SaveDataV5()
        {
            deviceStorage = new Dictionary<int, Device>()
		};

        return data;
    }
}

public class SaveDataV5 : SaveDataV4
{
    public SaveDataV5()
    {
        Version = 5;
		deviceStorage = new Dictionary<int, Device>();
	}

    public Dictionary<int, Device> deviceStorage;

	public override SaveData VersionUp()
    {
		return null;
	}
}