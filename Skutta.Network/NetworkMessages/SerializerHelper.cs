using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SerializationHelper
{
    static JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters =
            {
                new JsonStringEnumConverter()
            }
    };

    public static byte[] SerializeToBytes<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }


        string jsonString = JsonSerializer.Serialize(obj, options);
        return System.Text.Encoding.UTF8.GetBytes(jsonString);
    }

    public static T DeserializeFromBytes<T>(byte[] data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        string jsonString = System.Text.Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<T>(jsonString, options);
    }
}