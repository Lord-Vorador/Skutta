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

    public string Name { get; set; }

    public NetworkController(Player player, string name)
    {
        _player = player;
        Name = name;
    }

    public void Update(GameTime gameTime)
    {
        //Random random = new Random();
        //_player.SetPosition(new Vector2(random.Next(500), random.Next(500)));
    }

    public void SetPosition(Vector2 pos)
    {
        _player.SetPosition(pos);
    }

    public void SetDirection(bool direction)
    {
        if (direction)
        {
            _player.SetMovingLeft();
        }
        else
        {
            _player.SetMovingRight();
        }
    }
}
