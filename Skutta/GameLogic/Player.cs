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
    class Player
    {
        AudioDevice _audioDevice;
        // A texture for the box and its rectangle
        Texture2D playerTexture;
        Model playerModel;

        // The player's position in world space.
        Vector3 playerPosition = new Vector3(100, 300, 0);

        Rectangle body;
        //int playerVelocity = 5; // Movement speed for the box

        float moveSpeed = 5f;
        bool isJumping = false;
        float jumpVelocity = 0f;
        float gravity = 0.5f;
        float groundLevel = 0f; // Y position where the box rests.

        // Locked screen camera parameters.
        // For an 800x600 screen, we'll lock the camera's target to the center (400,300)
        // and place the camera at a fixed distance along Z.
        Vector3 lockedScreenCenter;
        float cameraDistance = 800f;

        int screenWidth;
        int screenHeight;

        // Camera matrices.
        Matrix view;
        Matrix projection;

        // Define the half-size of the player's bounding box.
        // (For example, if the player is 50x50 in size, half-size is 25.)
        Vector3 boundingBoxHalfExtents = new Vector3(6.5f, 6.5f, 0);


        public Player()
        {
            //body = new Rectangle(100, 100, 50, 50);
        }

        public void Initialize(GraphicsDevice graphics, Model model, AudioDevice audioDevice)
        {
            _audioDevice = audioDevice;
            playerModel = model;
            // Create a 1x1 white texture.
            //playerTexture = new Texture2D(graphics, 1, 1);
            //playerTexture.SetData(new[] { Color.White });

            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;

            lockedScreenCenter = new Vector3(screenWidth / 2, screenHeight / 2, 0);

            //cameraDistance = ;

            //body.Y = groundLevel;
            // Place the player on the ground.
            playerPosition.Y = groundLevel;

            // Set up the locked camera.
            // Position the camera directly above the locked screen center at a fixed Z distance.
            view = Matrix.CreateLookAt(
                new Vector3(lockedScreenCenter.X, lockedScreenCenter.Y, cameraDistance), // Camera position.
                lockedScreenCenter,                                                     // Camera target.
                Vector3.Up);                                                            // Up vector.

            // Create a perspective projection matrix.
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                graphics.Viewport.AspectRatio,
                1.0f,
                1000.0f);
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            // Horizontal movement.
            if (keyboard.IsKeyDown(Keys.Left))
                playerPosition.X -= moveSpeed;
            if (keyboard.IsKeyDown(Keys.Right))
                playerPosition.X += moveSpeed;

            // Jumping logic.
            if (keyboard.IsKeyDown(Keys.Space) && !isJumping)
            {
                _audioDevice.PlaySoundEffect("jump");
                isJumping = true;
                jumpVelocity = 10f; // Impulse upward.
            }
            if (isJumping)
            {
                playerPosition.Y += jumpVelocity;
                jumpVelocity -= gravity;

                if (playerPosition.Y <= groundLevel)
                {
                    playerPosition.Y = groundLevel;
                    isJumping = false;
                    jumpVelocity = 0f;
                }
            }

            // Create the player's BoundingBox in world space.
            // For a 2D game, we keep Z constant.
            BoundingBox playerBox = new BoundingBox(
                playerPosition - boundingBoxHalfExtents,
                playerPosition + boundingBoxHalfExtents);

            // Create a BoundingBox representing the screen boundaries.
            // Here we assume the screen's origin (0,0) maps to the bottom-left/top-left,
            // depending on your coordinate system. Adjust as needed.
            BoundingBox screenBox = new BoundingBox(
                new Vector3(0, 0, 0),
                new Vector3(screenWidth, screenHeight, 0));

            // Check for collision: if the player's bounding box extends beyond the screen bounds,
            // reposition the player accordingly.
            if (!playerBox.Intersects(screenBox))
            {
                // Left boundary.
                if (playerBox.Min.X < screenBox.Min.X)
                    playerPosition.X = screenBox.Min.X + boundingBoxHalfExtents.X;
                // Right boundary.
                if (playerBox.Max.X > screenBox.Max.X)
                    playerPosition.X = screenBox.Max.X - boundingBoxHalfExtents.X;
                // Top boundary.
                if (playerBox.Min.Y < screenBox.Min.Y)
                    playerPosition.Y = screenBox.Min.Y + boundingBoxHalfExtents.Y;
                // Bottom boundary.
                if (playerBox.Max.Y > screenBox.Max.Y)
                    playerPosition.Y = screenBox.Max.Y - boundingBoxHalfExtents.Y;
            }
        }

        public void Draw(GameTime gameTime)
        {
            // Create the world matrix for the player model.
            // Since this is a 2D platformer, we only modify X and Y (leaving Z constant).
            Matrix world = Matrix.CreateScale(3.0f) *
                           Matrix.CreateTranslation(playerPosition);
            // Create the world matrix for the player model.
           // Matrix world = Matrix.CreateTranslation(playerPosition);

            // Draw the 3D player model.
            foreach (ModelMesh mesh in playerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                    //Optionally, tweak light parameters:
                    //effect.DiffuseColor = new Vector3(1, 1, 1);
                    //effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.TextureEnabled = true; // Set to true if you want to use textures.
                    effect.PreferPerPixelLighting = true;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.DiffuseColor = new Vector3(1,1,1);
                }
                mesh.Draw();
            }
            //spriteBatch.Begin();
            //// Draw the box texture, stretching it to the rectangle's size and tinting it red
            //spriteBatch.Draw(playerTexture, body, Color.Red);
            //spriteBatch.End();
        }
    }
}
