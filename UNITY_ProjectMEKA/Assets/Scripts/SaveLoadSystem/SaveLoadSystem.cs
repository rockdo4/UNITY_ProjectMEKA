using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaveDataVC = SaveDataV2; // 교체

public static class SaveLoadSystem
{
    public enum Modes
    {
        Json,
        Binary,
        EncryptedBinary
    }
    public static Modes FileMode { get; } = Modes.Json;
    public static int SaveDataVersion { get; } = 1; // 버전
    private static string[] SaveSlotFileNames =
    {
        "Save0.json",
        "Save1.json",
        "Save2.json"
    };
    private static string AutoSaveFileName { get; } = "AutoSave";
    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static void AutoSave(SaveData data)
    {
        Save(data, AutoSaveFileName);
    }

    public static SaveData AutoLoad()
    {
        return Load(AutoSaveFileName);
    }

    public static void Save(SaveData data, int slot)
    {
        Save(data, SaveSlotFileNames[slot]);
    }

    public static SaveData Load(int slot)
    {
        return Load(SaveSlotFileNames[slot]);
    }

    public static void Save(SaveData data, string fileName)
    {
        if(!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        var path = Path.Combine(SaveDirectory, fileName);

        Debug.Log((path, "savefile.json"));

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var Serialize = new JsonSerializer();
            Serialize.Converters.Add(new Vector3Converter());
            Serialize.Converters.Add(new QuaternionConverter());
            Serialize.Converters.Add(new CharacterDictConverter());
            Serialize.Serialize(writer, data);
        }
    }

    public static SaveData Load(string fileName)
    {
        var path = Path.Combine(SaveDirectory, fileName);
        if (!File.Exists(path))
        {
            return null;
        }

        SaveData data = null;
        int version = 0;

        var json = File.ReadAllText(path);
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var jObj = JObject.Load(reader);
            version = jObj["Version"].Value<int>();
        }
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            serialize.Converters.Add(new Vector3Converter());
            serialize.Converters.Add(new QuaternionConverter());
            //serialize.Converters.Add(new 

            switch (version) // �߰������ ��
            {
                case 1:
                    data = serialize.Deserialize<SaveDataV1>(reader);
                    break;
                case 2:
                    data = serialize.Deserialize<SaveDataV2>(reader);
                    break;
                case 3:
                    //data = serialize.Deserialize<SaveDataV3>(reader);
                    break;
            }

            while (data.Version < SaveDataVersion)
            {
                data = data.VersionUp();
            }
        }

        return data;
    }
}