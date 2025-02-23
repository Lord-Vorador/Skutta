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
        protected string _soundEffectName;

        public bool IsPicked { get; set; }
        
        public Pickuppable(int xPos = 500, int yPos = 400)
        {
            IsPicked = false;
            _body = new Rectangle(xPos, yPos, 30, 30);
        }

        public void Initialize(GraphicsDevice graphics, Texture2D sprite, AudioDevice audioDevice)
        {
            _audioDevice = audioDevice;
            spriteBatch = new SpriteBatch(graphics);

            _soundEffectName = "coin-pickup";
            _texture = sprite;
        }

        private bool IsColliding(Player player)
        {
            Rectangle playerBody = player.GetPlayerBoundingBox();

            Vector2 pickuppableCenter = new Vector2(_body.X + _body.Width / 2, _body.Y + _body.Height / 2);
            Vector2 playerCenter = new Vector2(playerBody.X + playerBody.Width / 2, playerBody.Y + playerBody.Height / 2);

            float distance = Vector2.Distance(pickuppableCenter, playerCenter);

            float pickuppableRadius = Math.Min(_body.Width, _body.Height) / 2;
            float playerRadius = Math.Min(playerBody.Width, playerBody.Height) / 2;
            float sumOfRadiuses = pickuppableRadius + playerRadius;

            return distance < sumOfRadiuses;
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (IsColliding(player))
            {
                OnCollictionOccured(player);
                _audioDevice.PlaySoundEffect(_soundEffectName);
                IsPicked = true;
            }
        }

        protected virtual void OnCollictionOccured(Player player)
        {
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
