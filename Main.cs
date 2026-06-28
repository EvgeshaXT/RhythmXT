using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

internal class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Cursor _cursor;
    MainMenu _mainMenu;
    int _screenWidth, _screenHeight;

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
        FullScreen(_graphics);

        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;

        _mainMenu = new(GraphicsDevice, _screenWidth, _screenHeight);
        _cursor = new();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _mainMenu.LoadContent(Content);
        _cursor.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        MouseInputManager.Update();
        KeyboardInputManager.Update();

        if (KeyboardInputManager.EscapePressed || _mainMenu.ToExit) Exit();

        _cursor.Update();
        _mainMenu.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _mainMenu.Draw(_spriteBatch);
        
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
