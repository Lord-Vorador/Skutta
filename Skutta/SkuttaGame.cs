using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Skutta.GameLogic;

namespace Skutta;

public class SkuttaGame : Game
{
    private GraphicsDeviceManager _graphics;
    //private SpriteBatch _spriteBatch;
    private Player _player = new();
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;

    Song _song;
    SoundEffect _soundEffect;

    public SkuttaGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        //_spriteBatch = new SpriteBatch(GraphicsDevice);

        _soundEffect = Content.Load<SoundEffect>("Audio/Effects/jump");
        _player.Initialize(GraphicsDevice, _soundEffect);
        // TODO: use this.Content to load your game content here

        _song = Content.Load<Song>("Audio/Music/pattaya-by-scandinavianz");
        


        MediaPlayer.Play(_song);
        MediaPlayer.Volume = 0.5f;
        _song = Content.Load<Song>("Audio/Music/pattaya-by-scandinavianz");
        _soundEffect = Content.Load<SoundEffect>("Audio/Effects/jump");

        MediaPlayer.Play(_song);
        MediaPlayer.Volume = 0.5f;

        _player.Initialize(GraphicsDevice, _soundEffect);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load your background image
        _backgroundTexture = Content.Load<Texture2D>("background");
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
