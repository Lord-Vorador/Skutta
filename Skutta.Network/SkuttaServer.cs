using Lidgren.Network;
using Microsoft.Xna.Framework;
using Skutta.Network.NetworkMessages;
using Skutta.Network.NetworkMessages.Client;
using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network;

public class SkuttaServer
{
    private NetServer _server;
    private Dictionary<long, ConnectedPlayer> registeredClients = new Dictionary<long, ConnectedPlayer>();

    public void Run()
    {
        

        NetPeerConfiguration config = new NetPeerConfiguration(NetworkCommonConstants.AppIdentifier);
        config.SetMessageTypeEnabled(NetIncomingMessageType.UnconnectedData, true);
        config.Port = NetworkCommonConstants.GameServerPort;

        _server = new NetServer(config);
        _server.Start();

        Console.WriteLine("Press ESC to quit");
        while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.UnconnectedData:
                        //
                        // We've received a message from a client or a host
                        //

                        // by design, the first byte always indicates action
                        //switch ((SkuttaMessageTypes)msg.ReadByte())
                        //{
                        //    case SkuttaMessageTypes.ClientConnecting:
                        //
                        //        // It's a host wanting to register its presence
                        //        //var id = msg.ReadInt64(); // server unique identifier
                        //
                        //        //Console.WriteLine("New client connecting " + id);
                        //        //registeredClients.Add(msg.SenderConnection.RemoteUniqueIdentifier, new IPEndPoint[]
                        //        //{
                        //        //    msg.ReadIPEndPoint(), // internal
						//		//	msg.SenderEndPoint // external
						//		//});
                        //        //break;
                        //
                        //        //case GameServerMessageType.RequestHostList:
                        //        //    // It's a client wanting a list of registered hosts
                        //        //    Console.WriteLine("Sending list of " + registeredClients.Count + " hosts to client " + msg.SenderEndPoint);
                        //        //    foreach (var kvp in registeredClients)
                        //        //    {
                        //        //        // send registered host to client
                        //        //        NetOutgoingMessage om = peer.CreateMessage();
                        //        //        om.Write(kvp.Key);
                        //        //        om.Write(kvp.Value[0]);
                        //        //        om.Write(kvp.Value[1]);
                        //        //        peer.SendUnconnectedMessage(om, msg.SenderEndPoint);
                        //        //    }
                        //        //
                        //        //    break;
                        //        //case GameServerMessageType.RequestIntroduction:
                        //        //    // It's a client wanting to connect to a specific (external) host
                        //        //    IPEndPoint clientInternal = msg.ReadIPEndPoint();
                        //        //    long hostId = msg.ReadInt64();
                        //        //    string token = msg.ReadString();
                        //        //
                        //        //    Console.WriteLine(msg.SenderEndPoint + " requesting introduction to " + hostId + " (token " + token + ")");
                        //        //
                        //        //    // find in list
                        //        //    IPEndPoint[] elist;
                        //        //    if (registeredClients.TryGetValue(hostId, out elist))
                        //        //    {
                        //        //        // found in list - introduce client and host to eachother
                        //        //        Console.WriteLine("Sending introduction...");
                        //        //        peer.Introduce(
                        //        //            elist[0], // host internal
                        //        //            elist[1], // host external
                        //        //            clientInternal, // client internal
                        //        //            msg.SenderEndPoint, // client external
                        //        //            token // request token
                        //        //        );
                        //        //    }
                        //        //    else
                        //        //    {
                        //        //        Console.WriteLine("Client requested introduction to nonlisted host!");
                        //        //    }
                        //        //    break;
                        //}
                        //break;

                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        // print diagnostics message
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        HandleGameMessage(msg);
                    break;
                    default:
                            //Console.WriteLine(msg.ReadString());
                            break;
                }
            }
        }

        _server.Shutdown("shutting down");
    }

    private void HandleGameMessage(NetIncomingMessage msg)
    {
        var gameMsg = GetGameMessage(msg);

        var playerConnectingMsg = gameMsg as PlayerConnectingMessage;
        if (playerConnectingMsg != null)
        {
            var connectedPlayer = new ConnectedPlayer()
            {
                SenderConnection = msg.SenderConnection,
                Name = playerConnectingMsg.Name
            };

            registeredClients.Add(msg.SenderConnection.RemoteUniqueIdentifier, connectedPlayer);
            Console.WriteLine($"{playerConnectingMsg.Name} has joined the server!");
        }

        var playerPositionMsg = gameMsg as PlayerPositionMessage;
        if (playerPositionMsg != null)
        {
            //registeredClients.Add(msg.SenderConnection.RemoteUniqueIdentifier, msg.SenderConnection.RemoteEndPoint);
            if (registeredClients.TryGetValue(msg.SenderConnection.RemoteUniqueIdentifier, out ConnectedPlayer connectedPlayer))
            {
                Console.WriteLine($"{connectedPlayer.Name}: {playerPositionMsg.Position.X},{playerPositionMsg.Position.Y}");

                foreach (var client in registeredClients)
                {
                    var outgoingMsg = _server.CreateMessage();
                    outgoingMsg.Write((byte)SkuttaMessageTypes.BroadcastPosition);
                    outgoingMsg.Write(connectedPlayer.Name);
                    outgoingMsg.Write(playerPositionMsg.Position.X);
                    outgoingMsg.Write(playerPositionMsg.Position.Y);
                    outgoingMsg.Write((bool)playerPositionMsg.Direction);

                    _server.SendMessage(outgoingMsg, client.Value.SenderConnection, NetDeliveryMethod.UnreliableSequenced, 1);
                }
            }
        }
    }

    private INetworkMessage GetGameMessage(NetIncomingMessage msg)
    {
        var msgType = (SkuttaMessageTypes)msg.ReadByte();
        switch (msgType)
        {
            case SkuttaMessageTypes.ClientConnecting:
                var clientConnectingMsg = new PlayerConnectingMessage();
                clientConnectingMsg.Name = msg.ReadString();
                return clientConnectingMsg;
                break;
            case SkuttaMessageTypes.PlayerPosition:
                var playerPosMsg = new PlayerPositionMessage();
                float x = msg.ReadFloat();
                float y = msg.ReadFloat();
                playerPosMsg.Position = new Vector2(x, y);
                return playerPosMsg;
                break;
        }

        return null;
    }
}
