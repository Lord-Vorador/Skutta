using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace Skutta.GameLogic
{
    public class Player
    {
        SpriteBatch spriteBatch;
        GraphicsDevice _graphicsDevice;
        SpriteEffects _spriteEffects;
        Rectangle _body;
        Texture2D _playerTexture;
        Vector2 _position = new Vector2(400, 200);
        Vector2 _velocity = Vector2.Zero;
        Point _playerSize = new Point(32, 32);
        float jumpImpulse = 10f;
        int groundLevel; // Y position where the box rests.

        int screenWidth;
        int screenHeight;

        private AudioDevice _audioDevice;
        private float _gravity = 0.5f;


        public Player()
        {
        }

        public void Initialize(GraphicsDevice graphics, AudioDevice audioDevice, ContentManager content)
        {
            _audioDevice = audioDevice;
            spriteBatch = new SpriteBatch(graphics);

            // Create a 1x1 white texture.
            _playerTexture = new Texture2D(graphics, 1, 1);
            _playerTexture = content.Load<Texture2D>("player2");

            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;

            groundLevel = screenHeight - _playerSize.Y;

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
                            return true;
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
                (int)_playerSize.X,
                (int)_playerSize.Y);

            // Check horizontal collisions.
            if (!IsCollidingRect(horizontalRect, level.Map, 32, 32, 32))
            {
                // No collision horizontally: update X position.
                _position.X = newPos.X;
            }
            else
            {
                // Horizontal collision: cancel horizontal velocity.
                _velocity.X = 0;
            }

            // --- Vertical Movement ---
            newPos = _position;
            newPos.Y += _velocity.Y;
            Rectangle verticalRect = new Rectangle(
                (int)_position.X,
                (int)newPos.Y,
                (int)_playerSize.X,
                (int)_playerSize.Y);

            // Check vertical collisions.
            if (!IsCollidingRect(verticalRect, level.Map, 32, 32, 32))
            {
                // No collision vertically: update Y position.
                _position.Y = newPos.Y;
            }
            else
            {
                // Vertical collision: cancel vertical velocity.
                // Additionally, if you're falling, you might want to snap the player's position to the top of the colliding tile.
                _velocity.Y = 0;
            }

            // Apply gravity for the next frame.
            _velocity.Y += _gravity;

            // Check for collision

            //var newPos = _position + _velocity;
            //if (!IsColliding(newPos, level.Map, 32,32,32))
            //{
            //    _position = newPos;
            //    _velocity.Y += _gravity;
            //}
            //else
            //    _velocity = Vector2.Zero;



            //float playerCenterX = _position.X + (_playerSize.X / 2);
            //float playerCenterY = _position.Y + (_playerSize.Y / 2);

            //int playerTileX = playerX / 32;
            //int playerTileY = playerY / 32;

            //int playerTileIndex = playerTileY * 32 + playerTileX;

            //if (!(level.Map[playerTileIndex] > 0))
            //{
            //    var newPos = _position + _velocity;
            //    _position += _velocity;
            //    _velocity.Y += _gravity;
            //    //p.Position = new Vector2(playerTileX * tileWidth, playerTileY * tileHeight);
            //}
            //else
            //{
            //    // collitions

            //}

            // First, copy the current position.
            //Vector2 newPos = _position;

            //// --- Horizontal Movement ---
            //newPos.X += _velocity.X;
            //if (IsColliding(newPos, level.Map, 32, 32, 32))
            //{
            //    // If a collision occurs horizontally, don't move in X
            //    // Optionally, you could try to slide (or adjust position to the nearest non-colliding point)
            //    newPos.X = _position.X;
            //    _velocity.X = 0;
            //}

            //// --- Vertical Movement ---
            //newPos.Y += _velocity.Y;
            //if (IsColliding(newPos, level.Map, 32, 32, 32))
            //{
            //    // If a vertical collision occurs, don't move in Y
            //    newPos.Y = _position.Y;
            //    _velocity.Y = 0;
            //}
            //else
            //{
            //    // Only apply gravity if there's no collision vertically.
            //    _velocity.Y += _gravity;
            //}

            //// Finally, update the position.
            //_position = newPos;



            //if (_position.Y >= groundLevel)
            //{
            //    _velocity = Vector2.Zero;
            //}



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

        private bool IsColliding(Vector2 newPos, int[] map, int mapWidth, int mapHeight, int tileSize)
        {
            // Create the player's bounding rectangle based on the new position.
            Rectangle playerRect = new Rectangle(
                (int)newPos.X,
                (int)newPos.Y,
                (int)_playerSize.X,
                (int)_playerSize.Y);

            // Calculate which tiles the player's rectangle overlaps.
            int startX = Math.Max(0, playerRect.Left / tileSize);
            int endX = Math.Min(mapWidth - 1, playerRect.Right / tileSize);
            int startY = Math.Max(0, playerRect.Top / tileSize);
            int endY = Math.Min(mapHeight - 1, playerRect.Bottom / tileSize);

            // Iterate through each tile cell within the player's bounds.
            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    int tileIndex = y * mapWidth + x;
                    if (map[tileIndex] > 0)
                    {
                        // A solid tile was found.
                        return true;
                    }
                }
            }

            // No collisions detected.
            return false;
        }

        private bool isColliding(Vector2 newPos, int[] map)
        {
            int playerTileX = (int)newPos.X / 32;
            int playerTileY = (int)newPos.Y / 32;
            int playerTileIndex = playerTileY * 32 + playerTileX;

            if (map[playerTileIndex] > 0)
                return true;

            playerTileX = (int)(newPos.X+_playerSize.X) / 32;
            playerTileY = (int)newPos.Y / 32;
            playerTileIndex = playerTileY * 32 + playerTileX;

            if (map[playerTileIndex] > 0)
                return true;

            playerTileX = (int)(newPos.X + _playerSize.X) / 32;
            playerTileY = (int)(newPos.Y + _playerSize.Y) / 32;
            playerTileIndex = playerTileY * 32 + playerTileX;

            if (map[playerTileIndex] > 0)
                return true;

            playerTileX = (int)(newPos.X) / 32;
            playerTileY = (int)(newPos.Y + _playerSize.Y) / 32;
            playerTileIndex = playerTileY * 32 + playerTileX;

            if (map[playerTileIndex] > 0)
                return true;

            return false;
        }

        public void Draw(GameTime gameTime)
        {
            var rectangle = new Microsoft.Xna.Framework.Rectangle(new Point((int)_position.X, (int)_position.Y), _playerSize);

            spriteBatch.Begin();
            Vector2 centerPosition = new Vector2(rectangle.Center.X, rectangle.Center.Y);
            spriteBatch.Draw(_playerTexture, centerPosition, null, Color.White, 0f, new Vector2(_playerTexture.Width / 2f, _playerTexture.Height / 2f),
                1f, _spriteEffects, 0f);
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
            _spriteEffects = SpriteEffects.FlipHorizontally;
        }

        internal void SetMovingLeft()
        {
            _velocity.X = -10;
            _spriteEffects = SpriteEffects.None;
        }

        public void AddEffect(string name)
        {
            jumpImpulse += 5f;
        }

        internal void SetPosition(Vector2 position)
        {
            _position = position;
        }

        internal Rectangle GetPlayerBoundingBox()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, _playerSize.X, _playerSize.Y);
        }
    }
}
