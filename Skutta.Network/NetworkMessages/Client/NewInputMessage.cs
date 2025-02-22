using Skutta.Common.ValueTypes;
using Skutta.Network.NetworkMessages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.Network.NetworkMessages.Client;

public class NewInputMessage : INetworkMessage
{
    public List<SkuttaInput> _keysPressed = new List<SkuttaInput>();

    public NewInputMessage(List<SkuttaInput> keysPressed)
    {
        _keysPressed = keysPressed;
    }

    public static NewInputMessage FromBytes()
    {
        
    }
}
