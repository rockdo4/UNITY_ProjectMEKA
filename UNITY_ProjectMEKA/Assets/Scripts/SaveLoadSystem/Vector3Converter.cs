using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // ���̽� ������Ʈ���� �б�

        var jObj = JObject.Load(reader); // ��Ʈ��X=>���� ���X, ��ü�� �о���� ��
        var x = (float)jObj["x"];
        var y = (float)jObj["y"];
        var z = (float)jObj["z"];
        return new Vector3(x, y, z);

        //throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        // ���̽� �����ͷ� ����
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