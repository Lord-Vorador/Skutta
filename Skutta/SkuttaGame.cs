﻿using Microsoft.Xna.Framework;
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
using System.Numerics;
using System.Collections.Generic;
using System;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    //private SpriteBatch _spriteBatch;
    private List<IController> _playerControllers;
    private List<Player> _players;
    private List<Pickuppable> _pickuppables = new();
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;
    private AudioDevice _audioDevice = new();
    private bool _fullScreen = false;
    private KeyboardManager _keyboardManager;
    private SkuttaClient _skuttaClient;
    //andre gör saker
    private Random _random = new();

    private Level _level;

    private RenderTarget2D _renderTarget;
    private int _gameWidth = 512;
    private int _gameHeight = 288;

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _level = new Level();
        _audioDevice = new AudioDevice();
        _graphics.PreferredBackBufferWidth = _gameWidth * 2; // Set to your default window width
        _graphics.PreferredBackBufferHeight = _gameHeight * 2; // Set to your default window height

        GenerateRandomPickuppables(10); // Generate 10 random pickuppables
    }

    private void GenerateRandomPickuppables(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int xPos = _random.Next(0, 1024);
            int yPos = _random.Next(0, 576);
            _pickuppables.Add(new Pickuppable(xPos, yPos));
        }
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _keyboardManager = new KeyboardManager();
        _skuttaClient = new SkuttaClient();
        _skuttaClient.Connect("192.168.1.102", NetworkCommonConstants.GameServerPort);
        //_skuttaClient.SendMessage(new ClientConnectingMessage());

        _players = new List<Player>();
        _playerControllers = new List<IController>();

        _graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

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

        player2.SetPosition(new Microsoft.Xna.Framework.Vector2(250, 250));
        _playerControllers.Add(new NetworkController(player2));

        var jumpPickupTexture = Content.Load<Texture2D>("jump-powerup");
        foreach (var pickuppable in _pickuppables)
        {
            pickuppable.Initialize(GraphicsDevice, [jumpPickupTexture], _audioDevice);
        }

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load your background image
        _backgroundTexture = Content.Load<Texture2D>("background2");
        _renderTarget = new RenderTarget2D(GraphicsDevice, _gameWidth, _gameHeight);

        //var levelGround = Content.Load<Texture2D>("level_ground");
        var levelPlatform = Content.Load<Texture2D>("brick");
        var levelGround = new Texture2D(GraphicsDevice, 1, 1);
        levelGround.SetData(new[] { Color.Green });
        


        //var levelPlatform = new Texture2D(GraphicsDevice, 1, 1);
        //levelPlatform.SetData(new[] { Color.Silver });


        _level.Initialize(GraphicsDevice, [levelPlatform, levelGround]);

        _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
        _graphics.PreferredBackBufferHeight = 576; // Set to your default window height
    }

    private Player CreateNewPlayer()
    {
        var player = new Player();
        player.Initialize(GraphicsDevice, _audioDevice, Content);
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

        foreach (var pickuppable in _pickuppables)
        {
            if (pickuppable.IsPicked)
            {
                _pickuppables.Remove(pickuppable);
                break;
            }
            else
            {
                foreach (var player in _players)
                {
                    pickuppable.Update(gameTime, player);
                }
            }
        }

        //_level.Update(gameTime);

        foreach (var player in _players)
        {
            player.Update(gameTime, _level);
        }

        foreach (var player in _players)
        {
            foreach (var p in _players)
            {
                if (player.onTopOf(p))
                {
                    p.smash();
                }
            }

        }

        foreach (var controller in _playerControllers)
        {
            controller.Update(gameTime);
        }

        foreach (var pickuppable in _pickuppables)
        {
            if (pickuppable.IsPicked)
            {
                _pickuppables.Remove(pickuppable);
                break;
            }
            else
            {
                foreach (var player in _players)
                {
                    pickuppable.Update(gameTime, player);
                }
            }
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

        // Draw to render target
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _gameWidth, _gameHeight), Color.Gray);
        // Draw other game elements here
        _spriteBatch.End();

        // Draw render target to screen
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);

        _level.Draw(_spriteBatch);

        foreach (var player in _players)
        {
            player.Draw(gameTime);
        }

        foreach (var pickuppable in _pickuppables)
        {
            pickuppable.Draw(gameTime);
        }

        base.Draw(gameTime);
    }
}
