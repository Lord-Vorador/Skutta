using Lidgren.Network;
using Skutta.Network.NetworkMessages.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network;

public class SkuttaServer
{
    public void Run()
    {
        Dictionary<long, IPEndPoint[]> registeredClients = new Dictionary<long, IPEndPoint[]>();

        NetPeerConfiguration config = new NetPeerConfiguration(NetworkCommonConstants.AppIdentifier);
        config.SetMessageTypeEnabled(NetIncomingMessageType.UnconnectedData, true);
        config.Port = NetworkCommonConstants.GameServerPort;

        NetServer server = new NetServer(config);
        server.Start();

        Console.WriteLine("Press ESC to quit");
        while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.UnconnectedData:
                        //
                        // We've received a message from a client or a host
                        //

                        // by design, the first byte always indicates action
                        switch ((MessageTypes)msg.ReadByte())
                        {
                            case MessageTypes.ClientConnecting:

                                // It's a host wanting to register its presence
                                //var id = msg.ReadInt64(); // server unique identifier

                                //Console.WriteLine("New client connecting " + id);
                                registeredClients.Add(msg.SenderConnection.RemoteUniqueIdentifier, new IPEndPoint[]
                                {
                                    msg.ReadIPEndPoint(), // internal
									msg.SenderEndPoint // external
								});
                                break;

                                //case GameServerMessageType.RequestHostList:
                                //    // It's a client wanting a list of registered hosts
                                //    Console.WriteLine("Sending list of " + registeredClients.Count + " hosts to client " + msg.SenderEndPoint);
                                //    foreach (var kvp in registeredClients)
                                //    {
                                //        // send registered host to client
                                //        NetOutgoingMessage om = peer.CreateMessage();
                                //        om.Write(kvp.Key);
                                //        om.Write(kvp.Value[0]);
                                //        om.Write(kvp.Value[1]);
                                //        peer.SendUnconnectedMessage(om, msg.SenderEndPoint);
                                //    }
                                //
                                //    break;
                                //case GameServerMessageType.RequestIntroduction:
                                //    // It's a client wanting to connect to a specific (external) host
                                //    IPEndPoint clientInternal = msg.ReadIPEndPoint();
                                //    long hostId = msg.ReadInt64();
                                //    string token = msg.ReadString();
                                //
                                //    Console.WriteLine(msg.SenderEndPoint + " requesting introduction to " + hostId + " (token " + token + ")");
                                //
                                //    // find in list
                                //    IPEndPoint[] elist;
                                //    if (registeredClients.TryGetValue(hostId, out elist))
                                //    {
                                //        // found in list - introduce client and host to eachother
                                //        Console.WriteLine("Sending introduction...");
                                //        peer.Introduce(
                                //            elist[0], // host internal
                                //            elist[1], // host external
                                //            clientInternal, // client internal
                                //            msg.SenderEndPoint, // client external
                                //            token // request token
                                //        );
                                //    }
                                //    else
                                //    {
                                //        Console.WriteLine("Client requested introduction to nonlisted host!");
                                //    }
                                //    break;
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        // print diagnostics message
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        HandleMessage(msg);
                    break;
                    default:
                            //Console.WriteLine(msg.ReadString());
                            break;
                }
            }
        }

        server.Shutdown("shutting down");
    }

    private void HandleMessage(NetIncomingMessage msg)
    {
        var message = MessageCreator.Create(msg);

        //switch ((GameServerMessageType)msg.ReadByte())
        //{
        //Console.WriteLine(msg.ReadByte());
        //Console.WriteLine(msg.ReadString());
        //}
    }
}
