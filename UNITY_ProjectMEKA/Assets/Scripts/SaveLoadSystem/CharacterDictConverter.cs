using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDictConverter : JsonConverter<Dictionary<int, Character>>
{
	public override Dictionary<int, Character> ReadJson(JsonReader reader, Type objectType, Dictionary<int, Character> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		Dictionary<int, Character> result = new Dictionary<int, Character>();
		var jObj = JObject.Load(reader);

		foreach(var item in jObj)
		{
			var character = new Character();
			character.CharacterID = (int)item.Value["CharacterID"];
			character.CharacterLevel = (int)item.Value["CharacterLevel"];
			character.CurrentExp = (int)item.Value["CurrentExp"];
			character.CharacterGrade = (int)item.Value["CharacterGrade"];
			character.IsUnlock = (bool)item.Value["IsUnlock"];
			result.Add(int.Parse(item.Key), character);
		}

		return result;
	}

	public override void WriteJson(JsonWriter writer, Dictionary<int, Character> value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		foreach(var info in value)
		{	
			writer.WritePropertyName(info.Key.ToString());
			writer.WriteStartObject();
			writer.WritePropertyName("CharacterID");
			writer.WriteValue(info.Value.CharacterID);
			writer.WritePropertyName("CharacterLevel");
			writer.WriteValue(info.Value.CharacterLevel);
			writer.WritePropertyName("CurrentExp");
			writer.WriteValue(info.Value.CurrentExp);
			writer.WritePropertyName("CharacterGrade");
			writer.WriteValue(info.Value.CharacterGrade);
			writer.WritePropertyName("IsUnlock");
			writer.WriteValue(info.Value.IsUnlock);
			writer.WriteEndObject();
		}
		writer.WriteEndObject();
	}
}
