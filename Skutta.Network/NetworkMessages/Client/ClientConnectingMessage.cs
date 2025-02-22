using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network.NetworkMessages.Client;

public class ClientConnectingMessage : INetworkMessage
{
    public string Name { get; set; }
}
