using System.Diagnostics;
using UnityEngine;
using SaveDataVC = SaveDataV6;
using static Defines;

public class PlayDataManager
{
    public static SaveDataVC data;

    public static void Init()
    {
        if (GameManager.IsLoaded) return;

        GameManager.IsLoaded = true;

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
        FirstGameCharacterSaveDataSet();
    }

    private static void FirstGameStageSaveDataSet()
    {
        var stageTable = DataTableMgr.GetTable<StageTable>().GetOriginalTable();

        foreach(var stage in stageTable)
        {
            var saveData = new StageSaveData();
            saveData.stageID = stage.Key;
            if (stage.Value.Index == 0)
            {
                saveData.isUnlocked = true;
            }
            if (stage.Value.Class == (int)StageClass.Story)
            {
                data.storyStageDatas.Add(stage.Key,saveData);
            }
            else if(stage.Value.Class == (int)StageClass.Assignment)
            {
                data.assignmentStageDatas.Add(stage.Key, saveData);
            }
            else if(stage.Value.Class == (int)StageClass.Challenge)
            {
                data.challengeStageDatas.Add(stage.Key, saveData);
            }
        }

        for(int i = 0; i < 7; ++i)
        {
            data.systemUnlockData.Add(i + 1, false);
        }
    }

    private static void FirstGameCharacterSaveDataSet()
    {

        var charTable = DataTableMgr.GetTable<CharacterTable>().GetOriginalTable();

        var storage = CharacterManager.Instance.m_CharacterStorage;

        foreach (var character in charTable)
        {
            var chara = new Character();
            chara.CharacterID = character.Value.CharacterID;
            chara.CharacterLevel = 1;
            chara.CurrentExp = 0;
            chara.CharacterGrade = character.Value.InitialGrade;
            chara.SkillLevel = 1;
            chara.IsUnlock = false;

            chara.affection = new CharacterAffection();
            chara.affection.AffectionLevel = 1;
            chara.affection.AffectionPoint = 0;
            chara.affection.LastTime = default;

            if (!storage.ContainsKey(chara.CharacterID))
            {
                storage.Add(chara.CharacterID, chara);
            }
        }

        Save();
        CharacterManager.Instance.CheckPlayData();
    }
}