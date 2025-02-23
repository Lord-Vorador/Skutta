using Lidgren.Network;
using Skutta.Network.NetworkMessages.Client;
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
        private Thread _fetchThread;
        private NetClient _client;
        private bool _running = true;

        public SkuttaClient() { }

        public event Action<NetIncomingMessage> MessageReceived2;

        public void Connect(string ip, int port)
        {
            var config = new NetPeerConfiguration(NetworkCommonConstants.AppIdentifier);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            _client = new NetClient(config);

            _client.Start();
            _client.Connect(ip, port);

            _fetchThread = new Thread(FetchingLogic);
            _fetchThread.Start();
        }

        public bool IsConnected()
        {
            return _client.ConnectionStatus == NetConnectionStatus.Connected;
        }

        private void FetchingLogic()
        {
            while (_running)
            {
                NetIncomingMessage message;
                while ((message = _client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            //string receivedMessage = message.ReadString();
                            this.MessageReceived2.Invoke(message);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus)message.ReadByte();
                            Console.WriteLine($"Status changed: {status}");
                            break;

                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            string logMessage = message.ReadString();
                            Console.WriteLine(logMessage);
                            break;

                        default:
                            Console.WriteLine($"Unhandled type: {message.MessageType}");
                            break;
                    }

                    _client.Recycle(message);
                    
                    // Sleep briefly to avoid busy-waiting
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        public void Disconnect() { }

        public void SendMessage(PlayerPositionMessage msg)
        {
            NetOutgoingMessage outgoingMsg = _client.CreateMessage();
            outgoingMsg.Write((byte)SkuttaMessageTypes.PlayerPosition);
            outgoingMsg.Write(msg.Position.X);
            outgoingMsg.Write(msg.Position.Y);
            _client.SendMessage(outgoingMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMessage(PlayerConnectingMessage msg)
        {
            NetOutgoingMessage outgoingMsg = _client.CreateMessage();
            outgoingMsg.Write((byte)SkuttaMessageTypes.ClientConnecting);
            outgoingMsg.Write(msg.Name);
            _client.SendMessage(outgoingMsg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
