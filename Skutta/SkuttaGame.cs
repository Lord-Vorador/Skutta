using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Skutta.Common.ValueTypes;
using Skutta.Network;
using Skutta.Network.NetworkMessages.Client;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Skutta.GameLogic;
using Skutta.Engine;
using System;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    //private SpriteBatch _spriteBatch;
    private List<IController> _playerControllers;
    private List<Player> _players;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;
    private AudioDevice _audioDevice;
    private bool _fullScreen = false;
    private KeyboardManager _keyboardManager;
    private SkuttaClient _skuttaClient;

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
        _skuttaClient = new SkuttaClient();
        _skuttaClient.Connect("127.0.0.1", NetworkCommonConstants.GameServerPort);
        //_skuttaClient.SendMessage(new ClientConnectingMessage());

        _players = new List<Player>();
        _playerControllers = new List<IController>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _audioDevice.LoadContent(Content);
        _audioDevice.PlayRandomSong();

        var player = CreateNewPlayer();
        _players.Add(player);
        _playerControllers.Add(new PlayerController(player));

        var player2 = CreateNewPlayer();
        _players.Add(player2);
        _playerControllers.Add(new NetworkController(player2));

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load your background image
        _backgroundTexture = Content.Load<Texture2D>("background");

        _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
        _graphics.PreferredBackBufferHeight = 576; // Set to your default window height
    }

    private Player CreateNewPlayer()
    {
        var player = new Player();
        player.Initialize(GraphicsDevice, _audioDevice);
        return player;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var input = new SkuttaInput[]
        {
            SkuttaInput.MoveRight,
            SkuttaInput.MoveLeft
        };
        
        _skuttaClient.SendMessage(new InputMessage(input));

        foreach (var player in _players)
        {
            player.Update(gameTime);
        }

        foreach (var controller in _playerControllers)
        {
            controller.Update(gameTime);
        }

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

        foreach (var player in _players)
        {
            player.Draw(gameTime);
        }

        base.Draw(gameTime);
    }
}
