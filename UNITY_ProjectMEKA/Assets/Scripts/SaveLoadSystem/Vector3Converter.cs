using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // 제이슨 오브젝트에서 읽기

        var jObj = JObject.Load(reader); // 스트림X=>순서 상관X, 객체로 읽어오는 거
        var x = (float)jObj["x"];
        var y = (float)jObj["y"];
        var z = (float)jObj["z"];
        return new Vector3(x, y, z);

        //throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        // 제이슨 라이터로 쓰기
        /*
        {
            "x" : ???,
            "y" : ???,
            "z" : ???
        }
        */
        writer.WriteStartObject(); // {
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject(); // }

        //throw new NotImplementedException();
    }
}