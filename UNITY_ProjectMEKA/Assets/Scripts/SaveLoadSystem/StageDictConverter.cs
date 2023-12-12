using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDictConverter : JsonConverter<Dictionary<int, StageSaveData>>
{
    public override Dictionary<int, StageSaveData> ReadJson(JsonReader reader, Type objectType, Dictionary<int, StageSaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Dictionary<int, StageSaveData> result = new Dictionary<int, StageSaveData>();
        var jobj = JObject.Load(reader);

        foreach(var item in jobj)
        {
            var stageSaveData = new StageSaveData();
            stageSaveData.stageID = (int)item.Value["stageID"];
            stageSaveData.isUnlocked = (bool)item.Value["isUnlocked"];
            stageSaveData.isCleared = (bool)item.Value["isCleared"];
            stageSaveData.clearScore = (int)item.Value["clearScore"];
            result.Add(int.Parse(item.Key), stageSaveData);
        }
        return result;
    }

    public override void WriteJson(JsonWriter writer, Dictionary<int, StageSaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        foreach(var info in value)
        {
            writer.WritePropertyName(info.Key.ToString());
            writer.WriteStartObject();
            writer.WritePropertyName("stageID");
            writer.WriteValue(info.Value.stageID);
            writer.WritePropertyName("isUnlocked");
            writer.WriteValue(info.Value.isUnlocked);
            writer.WritePropertyName("isCleared");
            writer.WriteValue(info.Value.isCleared);
            writer.WritePropertyName("clearScore");
            writer.WriteValue(info.Value.clearScore);
            writer.WriteEndObject();
        }
        writer.WriteEndObject();
    }
}
