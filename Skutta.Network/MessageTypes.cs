using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network;

public enum SkuttaMessageTypes : byte
{
    ClientConnecting,
    PlayerPosition = 100,
    BroadcastPosition = 101,
}
