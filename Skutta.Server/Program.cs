using Lidgren.Network;
using Skutta.Network;
using System.Net;

namespace MasterServer;

public class Program
{
    private static SkuttaServer _server;

    static void Main(string[] args)
    {
        _server = new SkuttaServer();
        _server.Run();
    }
}