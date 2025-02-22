using Lidgren.Network;
using Skutta.Common.ValueTypes;
using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Skutta.Network.NetworkMessages.Client;

public class InputMessage : INetworkMessage
{
    public SkuttaInput[] _keysPressed { get; set; }

    public InputMessage()
    {
    }

    public InputMessage(SkuttaInput[] keysPressed)
    {
        _keysPressed = keysPressed;
    }

    //public static INetworkMessage CreateFromMessage(NetIncomingMessage msg)
    //{
    //    var msgType = (SkuttaMessageTypes)msg.ReadByte();
    //    if (msgType != SkuttaMessageTypes.PlayerMovement)
    //    {
    //        return null;
    //    }
    //
    //    //var skuttaMsg = new InputMessage();
    //    //int numberOfKeysPressed = msg.ReadByte();
    //    //var keysPressedAsBytes = msg.ReadBytes(numberOfKeysPressed);
    //    //skuttaMsg._keysPressed.AddRange(keysPressedAsBytes.Select(x => (SkuttaInput)x));
    //
    //    return skuttaMsg;
    //}
}
