using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.GameLogic;

internal class NetworkController : IController
{
    private Player _player;

    public NetworkController(Player player)
    {
        _player = player;
    }

    public void Update(GameTime gameTime)
    {
        //Random random = new Random();
        //_player.SetPosition(new Vector2(random.Next(500), random.Next(500)));
    }
}
