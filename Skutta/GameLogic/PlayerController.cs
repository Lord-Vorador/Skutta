using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.GameLogic
{
    internal class PlayerController : IController
    {
        private Player _player;

        public PlayerController(Player player)
        {
            _player = player;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
                _player.SetJumping();

            if (keyboardState.IsKeyDown(Keys.Right))
                _player.SetMovingRight();
            if (keyboardState.IsKeyDown(Keys.Left))
                _player.SetMovingLeft();

        }
    }
}
