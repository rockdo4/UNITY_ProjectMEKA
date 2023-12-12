using System.Diagnostics;
using UnityEngine;
using SaveDataVC = SaveDataV4;

public class PlayDataManager
{
    public static SaveDataVC data;

    public static void Init()
    {
        data = SaveLoadSystem.Load("savefile.json") as SaveDataVC;

        if (data == null)
        {
            Reset();
        }
    }

    public static void Save()
    {
        SaveLoadSystem.Save(data, "savefile.json");
    }

    public static void Reset()
    {
        data = new SaveDataVC();
        FirstGameSet();
        Save();
    }

    private static void FirstGameSet()
    {
        data.IsFirstGame = true;
        data.BGMVolume = 0.5f;
        data.SEVolume = 0.5f;
        data.MasterVolume = 0.5f;
        data.IsMasterVolumMute = false;
        data.IsBGMVolumMute = false;
        data.IsSEVolumMute = false;
        FirstGameStageSaveDataSet();
    }

    private static void FirstGameStageSaveDataSet()
    {
        var stageTable = DataTableMgr.GetTable<StageTable>().GetOriginalTable();

        foreach(var stage in stageTable)
        {
            var saveData = new StageSaveData();
            saveData.stageID = stage.Key;
            if (stage.Value.Index == 1)
            {
                saveData.isUnlocked = true;
            }
            if (stage.Value.Class == 1)
            {
                data.storyStageDatas.AddLast(saveData);
            }
            else if(stage.Value.Class == 2)
            {
                data.assignmentStageDatas.AddLast(saveData);
            }
            else if(stage.Value.Class == 3)
            {
                data.challengeStageDatas.AddLast(saveData);
            }
        }
    }
}