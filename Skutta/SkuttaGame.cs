using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Skutta.GameLogic;
using Skutta.Engine;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    //private SpriteBatch _spriteBatch;
    private Player _player = new();
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;
    private AudioDevice _audioDevice;
    private bool _fullScreen = false;
    private KeyboardManager _keyboardManager;

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _audioDevice = new AudioDevice();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _keyboardManager = new KeyboardManager();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _audioDevice.LoadContent(Content);
        _audioDevice.PlayRandomSong();

        //_player.Initialize(GraphicsDevice, _audioDevice);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load your background image
        _backgroundTexture = Content.Load<Texture2D>("background");

        _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
        _graphics.PreferredBackBufferHeight = 576; // Set to your default window height
        //_spriteBatch = new SpriteBatch(GraphicsDevice);
        var playerModel = Content.Load<Model>("Models/poop");
        //Texture2D texture = Content.Load<Texture2D>("Images/poop");
        //playerModel.Meshes[0].Effects[0].Texture = texture;
        _player.Initialize(GraphicsDevice, playerModel, _audioDevice);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _player.Update(gameTime);

        if (_keyboardManager.IsKeyPressedOnce(Keys.F11))
        {
            ToggleFullScreen();
        }

        _keyboardManager.Update();
        base.Update(gameTime);
    }
    private void ToggleFullScreen()
    {
        _fullScreen = !_fullScreen;
        if (_fullScreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.IsBorderless = true;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
            _graphics.PreferredBackBufferHeight = 576; // Set to your default window height
            Window.IsBorderless = false;
        }
        _graphics.ApplyChanges();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(
            _backgroundTexture,
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            Color.White
        );

        _spriteBatch.End();

        _player.Draw(gameTime);

        base.Draw(gameTime);
    }
}
