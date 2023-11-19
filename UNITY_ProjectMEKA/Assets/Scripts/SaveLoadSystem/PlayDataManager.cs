using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public class PlayDataManager
{
    public static SaveDataVC data;
    private static int reinforceCount = 6;

    public static void Init()
    {
        data = SaveLoadSystem.Load("savefile.json") as SaveDataVC;
        if (data == null)
        {
            data = new SaveDataVC();
            FirstGameSet();
            //data.isFirstGame = true;
            SaveLoadSystem.Save(data, "savefile.json");
        }
    }

    public static void Save()
    {
        SaveLoadSystem.Save(data, "savefile.json");
    }

    public static void Reset()
    {
        data = new SaveDataVC();
        Save();
    }

    private static void FirstGameSet()
    {

    }
}