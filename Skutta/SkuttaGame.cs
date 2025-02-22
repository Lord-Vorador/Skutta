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
    private IController _playerController;
    private Player _player;
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

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _level = new Level();
        _audioDevice = new AudioDevice();
        _player = new();
        _playerController = new PlayerController(_player);
        _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
        _graphics.PreferredBackBufferHeight = 576; // Set to your default window height

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
        _skuttaClient.Connect("127.0.0.1", NetworkCommonConstants.GameServerPort);
        //_skuttaClient.SendMessage(new ClientConnectingMessage());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _audioDevice.LoadContent(Content);
        _audioDevice.PlayRandomSong();

        foreach (var pickuppable in _pickuppables)
        {
            pickuppable.Initialize(GraphicsDevice, _audioDevice);
        }

        _player.Initialize(GraphicsDevice, _audioDevice);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load your background image
        _backgroundTexture = Content.Load<Texture2D>("background");

        //var levelGround = Content.Load<Texture2D>("level_ground");
        //var levelPlatform = Content.Load<Texture2D>("level_platform");
        var levelGround = new Texture2D(GraphicsDevice, 1, 1);
        levelGround.SetData(new[] { Color.Green });

        var levelPlatform = new Texture2D(GraphicsDevice, 1, 1);
        levelPlatform.SetData(new[] { Color.Silver });


        _level.Initialize(GraphicsDevice, [levelGround, levelPlatform]);

        _graphics.PreferredBackBufferWidth = 1024; // Set to your default window width
        _graphics.PreferredBackBufferHeight = 576; // Set to your default window height
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
                pickuppable.Update(gameTime, _player);
            }
        }
        _playerController.Update(gameTime);
        _player.Update(gameTime);

        foreach (var pickuppable in _pickuppables)
        {
            if (pickuppable.IsPicked)
            {
                _pickuppables.Remove(pickuppable);
                break;
            }
            else
            {
                pickuppable.Update(gameTime, _player);
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

        _spriteBatch.Begin();

        _spriteBatch.Draw(
            _backgroundTexture,
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            Color.White
        );

        _spriteBatch.End();

        _level.Draw(_spriteBatch);

        foreach (var pickuppable in _pickuppables)
        {
            pickuppable.Draw(gameTime);
        }

        _player.Draw(gameTime);

        base.Draw(gameTime);
    }
}
