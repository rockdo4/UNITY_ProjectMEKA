using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using SaveDataVC = SaveDataV1;

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
        data.Money = 0;
        data.BGMVolume = 0.5f;
        data.SEVolume = 0.5f;
        data.MasterVolume = 0.5f;
        data.IsMasterVolumMute = false;
        data.IsBGMVolumMute = false;
        data.IsSEVolumMute = false;
    }
}