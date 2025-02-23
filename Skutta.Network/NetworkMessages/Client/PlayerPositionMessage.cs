
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Skutta.Network.NetworkMessages.Common;
using System.Text;
using System;

namespace Skutta.Network.NetworkMessages.Client;
public class PlayerPositionMessage : INetworkMessage
{
    public Vector2 Position { get; set; }
    public bool Direction { get; set; }

    public byte[] SerializeToByteArray()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.UTF8))
            {
                writer.Write(Position.X);
                writer.Write(Position.Y);
            }

            return memoryStream.ToArray();
        }
    }
}