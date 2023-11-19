//using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SaveDataVC = SaveDataV1;
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

    public int Money { get; set; }
    public bool IsFirstGame { get; set; } = true;
    public float BGMVolume { get; set; }
    public float SEVolume { get; set; }
    public float MasterVolume { get; set; }
    public bool IsMasterVolumMute { get; set; }
    public bool IsBGMVolumMute { get; set; }
    public bool IsSEVolumMute { get; set; }

    public override SaveData VersionUp()
    {
        return null;
    }
}