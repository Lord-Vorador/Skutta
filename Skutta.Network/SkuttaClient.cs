using Lidgren.Network;
using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network
{
    public class SkuttaClient
    {
        private NetClient _client;

        public SkuttaClient() { }

        public void Connect(string ip, int port)
        {
            var config = new NetPeerConfiguration(NetworkCommonConstants.AppIdentifier);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            _client = new NetClient(config);

            _client.Start();
            _client.Connect(ip, port);
        }

        public void Disconnect() { }

        public void SendMessage(INetworkMessage msg)
        {
            NetOutgoingMessage outgoingMsg = _client.CreateMessage();
            outgoingMsg.Write(SerializationHelper.SerializeToBytes(msg));
            _client.SendMessage(outgoingMsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
