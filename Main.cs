using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Cursor _cursor;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        IsFixedTimeStep = false;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _cursor = new Cursor();
        FullScreen(_graphics);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cursor.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _cursor.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        _cursor.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    void FullScreen(GraphicsDeviceManager graphics)
    {
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

        graphics.PreferredBackBufferWidth = displayMode.Width;
        graphics.PreferredBackBufferHeight = displayMode.Height;

        graphics.IsFullScreen = true;
        graphics.ApplyChanges();
    }
}
