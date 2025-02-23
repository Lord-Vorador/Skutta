using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Skutta.Network;
using Skutta.Network.NetworkMessages.Client;
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
        private readonly SkuttaClient skuttaClient;

        public string Name { get; set; }

        public PlayerController(Player player, SkuttaClient skuttaClient)
        {
            _player = player;
            this.skuttaClient = skuttaClient;
            Name = "Björn";
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

            skuttaClient.SendMessage(new PlayerPositionMessage() { Position = _player.GetPosition() });
        }

        public void SetPosition(Vector2 pos)
        {
            // Do nuffin'
        }
    }
}
