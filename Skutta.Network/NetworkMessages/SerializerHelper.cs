//using System;
//using System.Text.Json;

//public class SerializationHelper
//{
//    public static byte[] SerializeToBytes<T>(T obj)
//    {
//        if (obj == null)
//        {
//            throw new ArgumentNullException(nameof(obj));
//        }

//        string jsonString = JsonSerializer.Serialize(obj);
//        return System.Text.Encoding.UTF8.GetBytes(jsonString);
//    }

//    public static T DeserializeFromBytes<T>(byte[] data)
//    {
//        if (data == null)
//        {
//            throw new ArgumentNullException(nameof(data));
//        }

//        string jsonString = System.Text.Encoding.UTF8.GetString(data);
//        return JsonSerializer.Deserialize<T>(jsonString);
//    }
//}