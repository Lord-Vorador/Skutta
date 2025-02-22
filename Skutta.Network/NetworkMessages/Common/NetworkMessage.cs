//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Skutta.Network.NetworkMessages.Common;

//public class NetworkMessage : INetworkMessage
//{
//    public byte[] ReadBytes<T>()
//    {
//        string jsonString = JsonSerializer.Serialize(this);
//        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
//        return bytes;
//    }

//    public T GetMessageFromBytes<T>(byte[] data)
//    {
//        string jsonString = System.Text.Encoding.UTF8.GetString(data);
//        T message = JsonSerializer.Deserialize<T>(jsonString);
//        return message;
//    }
//}
