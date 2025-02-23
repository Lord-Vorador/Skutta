using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skutta.GameLogic;
using System.Reflection.Metadata;

namespace Skutta
{
    public class Pickuppable
    {
        private SpriteBatch spriteBatch;
        private Texture2D _texture;
        private Rectangle _body;
        private AudioDevice _audioDevice;
        
        public bool IsPicked { get; set; }
        
        public Pickuppable(int xPos = 500, int yPos = 400)
        {
            IsPicked = false;
            _body = new Rectangle(xPos, yPos, 30, 30);
        }

        public void Initialize(GraphicsDevice graphics, Texture2D[] sprites, AudioDevice audioDevice)
        {
            _audioDevice = audioDevice;
            spriteBatch = new SpriteBatch(graphics);

            // Create a 1x1 white texture.

            _texture = sprites[0];
            //_texture.SetData(new[] { Color.White });
        }

        private bool IsColliding(Player player)
        {
            // Assuming PlayerIndex is an enum and we have a way to get the player's body rectangle
            Rectangle playerBody = player.GetPlayerBoundingBox();

            // Calculate the centers of both rectangles
            Vector2 pickuppableCenter = new Vector2(_body.X + _body.Width / 2, _body.Y + _body.Height / 2);
            Vector2 playerCenter = new Vector2(playerBody.X + playerBody.Width / 2, playerBody.Y + playerBody.Height / 2);

            // Calculate the distance between the centers
            float distance = Vector2.Distance(pickuppableCenter, playerCenter);

            // Calculate the sum of the radii
            float pickuppableRadius = Math.Min(_body.Width, _body.Height) / 2;
            float playerRadius = Math.Min(playerBody.Width, playerBody.Height) / 2;
            float sumOfRadiuses = pickuppableRadius + playerRadius;

            // Check for collision
            return distance < sumOfRadiuses;
        }

        private Rectangle GetPlayerBody(PlayerIndex player)
        {
            // Placeholder method to get the player's body rectangle
            // This should be replaced with the actual implementation
            return new Rectangle(120, 120, 30, 30); // Example values
        }

        public void Update(GameTime gameTime, Player player)
        {
            //move upp and down maby rotate 

            if (IsColliding(player))
            {
                // Play a sound effect when the player collides with the pickuppable
                _audioDevice.PlaySoundEffect("coin-pickup");
                // Do something when the player collides with the pickuppable
                // For example, increase the player's score
                player.AddEffect("increase jump");
                
                IsPicked = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!IsPicked)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                spriteBatch.Draw(_texture, _body, Color.AliceBlue);
                spriteBatch.End();
            }
        }
    }
}
