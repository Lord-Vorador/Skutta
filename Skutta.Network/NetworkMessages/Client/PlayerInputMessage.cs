﻿//using Lidgren.Network;
//using Skutta.Common.ValueTypes;
//using Skutta.Network.NetworkMessages.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace Skutta.Network.NetworkMessages.Client;

//public class PlayerInputMessage : INetworkMessage
//{
//    public SkuttaInput[] _keysPressed { get; set; }

//    public PlayerInputMessage()
//    {
//    }

//    public PlayerInputMessage(SkuttaInput[] keysPressed)
//    {
//        _keysPressed = keysPressed;
//    }

//    public byte GetMessageType()
//    {
//        return (byte)SkuttaMessageTypes.PlayerInput;
//    }

//    public byte[] GetBytes()
//    {
//        return SerializationHelper.SerializeToBytes<PlayerInputMessage>(this);
//    }

//    //public static INetworkMessage CreateFromMessage(NetIncomingMessage msg)
//    //{
//    //    var msgType = (SkuttaMessageTypes)msg.ReadByte();
//    //    if (msgType != SkuttaMessageTypes.PlayerMovement)
//    //    {
//    //        return null;
//    //    }
//    //
//    //    //var skuttaMsg = new InputMessage();
//    //    //int numberOfKeysPressed = msg.ReadByte();
//    //    //var keysPressedAsBytes = msg.ReadBytes(numberOfKeysPressed);
//    //    //skuttaMsg._keysPressed.AddRange(keysPressedAsBytes.Select(x => (SkuttaInput)x));
//    //
//    //    return skuttaMsg;
//    //}
//}
