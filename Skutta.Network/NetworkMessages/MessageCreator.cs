//using Lidgren.Network;
//using Skutta.Network.NetworkMessages.Client;
//using Skutta.Network.NetworkMessages.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Skutta.Network.NetworkMessages;

//internal class MessageCreator
//{
//    public static INetworkMessage? ParseMessage(NetIncomingMessage msg)
//    {
//        var messageType = msg.PeekByte();
//        switch ((SkuttaMessageTypes)messageType)
//        {
//            case SkuttaMessageTypes.PlayerMovement:
//                return InputMessage.CreateFromMessage(msg);
//            default:
//                Console.WriteLine($"unknown message: {msg.PeekByte()}");
//                break;
//        }

//        return null;
//    }
//}
