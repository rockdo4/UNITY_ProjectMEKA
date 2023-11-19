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

    //public int money { get; set; }
    //public bool isFirstGame { get; set; } = true;
    //public Dictionary<string, ReinforceData> reinforceDatas { get; set; } = new Dictionary<string, ReinforceData>();

    public override SaveData VersionUp()
    {
        return null;
    }
}

//public class SaveDataV2 : SaveData
//{
//    public SaveDataV2()
//    {
//        Version = 2;
//    }

//    public int money { get; set; }
//    public bool isFirstGame { get; set; } = true;
//    public float bgmVolume { get; set; }
//    public float effectVolume { get; set; }
//    public Dictionary<string, ReinforceData> reinforceDatas { get; set; } = new Dictionary<string, ReinforceData>();

//    public override SaveData VersionUp()
//    {
//        //var data = new SaveDataV3();
//        return null;
//    }
//}

//public class SaveDataV3 : SaveData
//{
//    public SaveDataV3()
//    {
//        Version = 3;

//    }

//    //public List<CubeInfo> cubeList { get; set; } = new List<CubeInfo>();

//    public override SaveData VersionUp()
//    {
//        return null;
//    }
//}