using System.Net;
using Lidgren.Network;

namespace Skutta.Network;

internal class ConnectedPlayer
{
    public NetConnection SenderConnection { get; set; }
    public string Name { get; set; }
    public ConnectedPlayer()
    {
    }

}