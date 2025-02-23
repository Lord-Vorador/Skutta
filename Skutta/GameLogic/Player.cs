using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;

namespace Skutta.GameLogic
{
    public class Player
    {
        GraphicsDevice _graphicsDevice;
        SpriteEffects _spriteEffects;
        Rectangle _body;
        Texture2D _playerTexture;
        Vector2 _position = new Vector2(400, 200);
        Vector2 _velocity = Vector2.Zero;
        float jumpImpulse = 10f;
        float mooveSpeed = 10f;
        int groundLevel; // Y position where the box rests.

        int screenWidth;
        int screenHeight;

        private AudioDevice _audioDevice;
        private float _gravity = 0.5f;

        public bool onGround = false;
        public int height;
        private bool isSmashed;

        public Player()
        {
        }

        public void Initialize(GraphicsDevice graphics, AudioDevice audioDevice, ContentManager content)
        {
            _audioDevice = audioDevice;

            // Create a 1x1 white texture.
            //_playerTexture = new Texture2D(graphics, 1, 1);
            _playerTexture = content.Load<Texture2D>("player");
            height = _playerTexture.Height;

            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;

            groundLevel = screenHeight - SkuttaGame._tileSize;

            _velocity = new Vector2(0, 0);
        }

        // Resolves collisions by moving the player out of any colliding tile.
        private bool IsCollidingRect(Rectangle rect, int[] map, int mapWidth, int mapHeight, int tileSize)
        {
            // Determine which tiles the rectangle covers.
            int startX = Math.Max(0, rect.Left / tileSize);
            int endX = Math.Min(mapWidth - 1, rect.Right / tileSize);
            int startY = Math.Max(0, rect.Top / tileSize);
            int endY = Math.Min(mapHeight - 1, rect.Bottom / tileSize);

            // Check each overlapping tile.
            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    int tileIndex = y * mapWidth + x;
                    if (tileIndex < map.Length && map[tileIndex] > 0)
                    {
                        Rectangle tileRect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                        if (rect.Intersects(tileRect))
                        {
                            // Collision found.
                            onGround = tileRect.Bottom > rect.Bottom;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Update(GameTime gameTime, Level level)
        {
            Vector2 newPos = _position;

            // --- Horizontal Movement ---
            newPos.X += _velocity.X;
            Rectangle horizontalRect = new Rectangle(
                (int)newPos.X,
                (int)_position.Y,
                (int)SkuttaGame._tileSize,
                (int)SkuttaGame._tileSize);

            // Check horizontal collisions.
            if (!IsCollidingRect(horizontalRect, level.Map, 32, 18, SkuttaGame._tileSize))
            {
                // No collision horizontally: update X position.
                _position.X = newPos.X;
            }
            else
            {
                // Horizontal collision: cancel horizontal velocity.
                _velocity.X = 0;
                //onGround = true;
            }

            // --- Vertical Movement ---
            newPos = _position;
            newPos.Y += _velocity.Y;
            Rectangle verticalRect = new Rectangle(
                (int)_position.X,
                (int)newPos.Y,
                (int)SkuttaGame._tileSize,
                (int)SkuttaGame._tileSize);

            // Check vertical collisions.
            if (!IsCollidingRect(verticalRect, level.Map, 32, 18, SkuttaGame._tileSize))
            {
                // No collision vertically: update Y position.
                _position.Y = newPos.Y;
                _velocity.Y += _gravity;
            }
            else
            {
                // Vertical collision: cancel vertical velocity.
                // Additionally, if you're falling, you might want to snap the player's position to the top of the colliding tile.
                _velocity.Y = 0;
                //onGround = true;
            }

            // Apply gravity for the next frame.          
            _velocity.X = 0;

            // Collision detection against the screen boundaries.
            if (_position.X < 0)
                _position.X = 0;
            // Prevent moving off the right edge.
            if (_position.X + SkuttaGame._tileSize > screenWidth)
                _position.X = screenWidth - SkuttaGame._tileSize;
            // Prevent moving above the top edge.
            if (_position.Y < 0)
                _position.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var rectangle = new Microsoft.Xna.Framework.Rectangle(new Point((int)(_position.X), (int)(_position.Y)),
                new Point((int)(SkuttaGame._tileSize), (int)(SkuttaGame._tileSize)));

            Vector2 centerPosition = new Vector2(rectangle.Center.X, rectangle.Center.Y);
            spriteBatch.Draw(_playerTexture, centerPosition, null, Color.White, 0f, new Vector2(8, 8),
                1f, _spriteEffects, 0f);
        }

        public void SetJumping()
        {
            if (_velocity.Y == 0 && onGround)
            { 
                _audioDevice.PlaySoundEffect("jump");
                _velocity.Y = -jumpImpulse;
            }
        }

        internal void SetMovingRight(int speed = 10)
        {
            _velocity.X = mooveSpeed;
            _spriteEffects = SpriteEffects.FlipHorizontally;
        }

        internal void SetMovingLeft(int speed = 10)
        {
            _velocity.X = -mooveSpeed;
            _spriteEffects = SpriteEffects.None;
        }

        public void JumpPowerup()
        {
            jumpImpulse += 5f;
        }

        public void MovePowerup()
        {
            mooveSpeed += 5f;
        }

        internal void SetPosition(Vector2 position)
        {
            _position = position;
        }

        internal Rectangle GetPlayerBoundingBox()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, SkuttaGame._tileSize, SkuttaGame._tileSize);
        }

        internal bool onTopOf(Player p)
        {
            if (this == p || p.isSmashed || isSmashed)
                return false;

            var pbox = p.GetPlayerBoundingBox();
            var box = GetPlayerBoundingBox();

            if (pbox.Intersects(box))
            {
                if (box.Bottom > pbox.Top && box.Top < pbox.Top)
                    return true;
            }

            return false;

        }

        internal void smash()
        {
            isSmashed = true;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        internal bool GetDirection()
        {
            return _spriteEffects == SpriteEffects.FlipHorizontally;
        }
    }
}
