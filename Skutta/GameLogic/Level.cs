using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta.GameLogic
{
    public class Level
    {
        private int mapWidth = 32;
        private int[] map = [
            1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,
            1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,
            0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,2,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1,1,0,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,
            0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
            1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,
            1,2,1,2,1,2,1,2,1,2,1,0,0,0,0,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,
            1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,
            0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,2,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1,1,0,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,1,
            0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
        ];

        public int[] Map { get { return map; } }

        //16x9

        private Texture2D[] mapSprites;

        int screenWidth;
        int screenHeight;

        int tileWidth = 32;
        int tileHeight = 32;

        public Level()
        {
            // Load level data
        }

        public void Initialize(GraphicsDevice graphics, Texture2D[] sprites)
        {
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            mapSprites = sprites;
        }

        public void Update(GameTime gameTime)
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw level
            spriteBatch.Begin();

            for (int i = 0; i < map.Length; ++i)
            {
                // Draw tile
                if (map[i] > 0)
                {
                    // Draw sprite
                    int row = i / mapWidth;
                    int column = i % mapWidth;
                    int x = (column * tileWidth);
                    int y = (row * tileHeight);

                    spriteBatch.Draw(mapSprites[map[i]-1], new Rectangle(x, y, tileWidth, tileHeight), Color.White);
                }
            }
            

            spriteBatch.End();
        }
    }
}
