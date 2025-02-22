using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.GameLogic
{
    public class Player
    {
        SpriteBatch spriteBatch;
        // A texture for the box and its rectangle
        Texture2D playerTexture;
        Rectangle body;
        int playerVelocity = 5; // Movement speed for the box

        // Jumping related fields.
        bool isJumping = false;
        float jumpVelocity = 0f;
        float jumpImpulse = 10f;
        float gravity = 0.5f;
        int groundLevel; // Y position where the box rests.

        int screenWidth;
        int screenHeight;

        private AudioDevice _audioDevice;

        public Player()
        {
            body = new Rectangle(100, 100, 50, 50);
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

            groundLevel = screenHeight - body.Height;

            body.Y = groundLevel;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Horizontal movement with arrow keys.
            if (keyboardState.IsKeyDown(Keys.Right))
                body.X += playerVelocity;
            if (keyboardState.IsKeyDown(Keys.Left))
                body.X -= playerVelocity;

            // Initiate a jump when space is pressed, but only if the box is on the ground.
            if (keyboardState.IsKeyDown(Keys.Space) && body.Y >= groundLevel)
            {
                jumpVelocity = -jumpImpulse; // Negative velocity gives an upward impulse.
                _audioDevice.PlaySoundEffect("jump");
            }

            // Apply gravity continuously if the box is in the air or in upward motion.
            if (body.Y < groundLevel || jumpVelocity < 0)
            {
                body.Y += (int)jumpVelocity;
                jumpVelocity += gravity;

                // If the box reaches or falls below ground level, snap it to the ground.
                if (body.Y >= groundLevel)
                {
                    body.Y = groundLevel;
                    jumpVelocity = 0f;
                }
            }

            // Collision detection against the screen boundaries.

            // Prevent moving off the left edge.
            if (body.X < 0)
                body.X = 0;
            // Prevent moving off the right edge.
            if (body.X + body.Width > screenWidth)
                body.X = screenWidth - body.Width;
            // Prevent moving above the top edge.
            if (body.Y < 0)
                body.Y = 0;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            // Draw the box texture, stretching it to the rectangle's size and tinting it red
            spriteBatch.Draw(playerTexture, body, Color.Red);
            spriteBatch.End();
        }

        public Rectangle GetBody()
        {
            return body;
        }

        public void AddEffect(string name)
        {
            jumpImpulse += 5f;
        }
    }
}
