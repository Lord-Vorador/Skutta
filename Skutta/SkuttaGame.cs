using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Skutta.GameLogic;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    //private SpriteBatch _spriteBatch;
    private Player _player = new();

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        //_spriteBatch = new SpriteBatch(GraphicsDevice);
        var playerModel = Content.Load<Model>("Models/poop");
        //Texture2D texture = Content.Load<Texture2D>("Images/poop");
        //playerModel.Meshes[0].Effects[0].Texture = texture;
        _player.Initialize(GraphicsDevice, playerModel);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _player.Draw(gameTime);

        base.Draw(gameTime);
    }
}
