using Lidgren.Network;
using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network.NetworkMessages
{
    internal class MessageCreator
    {
        public static INetworkMessage CreateMessage(NetIncomingMessage msg)
        {
            var messageType = msg.ReadByte();
            switch (messageType)
            {
                case SkuttaMessageTypes.:
            }
        }
    }
}
