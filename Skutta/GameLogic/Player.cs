using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection;

namespace Skutta.GameLogic
{
    public class Player
    {
        SpriteBatch spriteBatch;
        Texture2D playerTexture;
        Vector2 _position = new Vector2(400, 400);
        Vector2 _velocity = Vector2.Zero;
        Point _playerSize = new Point(50, 50);
        float jumpImpulse = 10f;
        int groundLevel; // Y position where the box rests.

        int screenWidth;
        int screenHeight;

        private AudioDevice _audioDevice;
        private float _gravity = 0.5f;

        public Player()
        {
        }

        public void Initialize(GraphicsDevice graphics, AudioDevice audioDevice)
        {
            _audioDevice = audioDevice;
            spriteBatch = new SpriteBatch(graphics);

            // Create a 1x1 white texture.
            playerTexture = new Texture2D(graphics, 1, 1);
            playerTexture.SetData(new[] { Color.White });

            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;

            groundLevel = screenHeight - _playerSize.X;

            _velocity = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime)
        {
            _position += _velocity;
            _velocity.Y += _gravity;

            if (_position.Y >= groundLevel)
            {
                _velocity = Vector2.Zero;
            }

            // Collision detection against the screen boundaries.
            if (_position.X < 0)
                _position.X = 0;
            // Prevent moving off the right edge.
            if (_position.X + _playerSize.X > screenWidth)
                _position.X = screenWidth - _playerSize.X;
            // Prevent moving above the top edge.
            if (_position.Y < 0)
                _position.Y = 0;
        }

        public void Draw(GameTime gameTime)
        {
            var rectangle = new Microsoft.Xna.Framework.Rectangle(new Point((int)_position.X, (int)_position.Y), _playerSize);

            spriteBatch.Begin();
            // Draw the box texture, stretching it to the rectangle's size and tinting it red
            spriteBatch.Draw(playerTexture, rectangle, Color.Red);
            spriteBatch.End();
        }

        public void SetJumping()
        {
            _audioDevice.PlaySoundEffect("jump");
            _velocity.Y = -jumpImpulse;
        }

        internal void SetMovingRight()
        {
            _velocity.X = 10;
        }

        internal void SetMovingLeft()
        {
            _velocity.X = -10;
        }

        public Rectangle GetBody()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, _playerSize.X, _playerSize.Y);
        }

        public void AddEffect(string name)
        {
            jumpImpulse += 5f;
        }
    }
}
