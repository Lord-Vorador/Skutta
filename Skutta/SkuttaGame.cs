using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Skutta.Common.ValueTypes;
using Skutta.Network;
using Skutta.Network.NetworkMessages.Client;
using System.Collections.Generic;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SkuttaClient _skuttaClient;

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _skuttaClient = new SkuttaClient();
        _skuttaClient.Connect("127.0.0.1", NetworkCommonConstants.GameServerPort);
        //_skuttaClient.SendMessage(new ClientConnectingMessage());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
