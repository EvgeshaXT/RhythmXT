using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Main : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    double _gameDeltaTime;

    Cursor _cursor;
    MainMenu _mainMenu;
    MapSelectionMenu _mapSelectionMenu;
    int _screenWidth, _screenHeight;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";    

        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 1000.0);
        IsFixedTimeStep = true;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();

        _gameDeltaTime = 0d;
    }

    protected override void Initialize()
    {
        FullScreen(_graphics);

        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;

        _mainMenu = new(GraphicsDevice, _screenWidth, _screenHeight);
        _mapSelectionMenu = new(_screenWidth, _screenHeight);

        _mainMenu.ClickedEvent += SoundEffects.Click;
        _mainMenu.ClickAnimationIsFinishedEvent += () => _mapSelectionMenu.Show();

        _mapSelectionMenu.ToMainMenuEvent += () => _mainMenu.Show();

        _cursor = new();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundEffects.LoadContent(Content);

        _mainMenu.LoadContent(Content);
        _mapSelectionMenu.LoadContent(Content, GraphicsDevice);
        _cursor.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _gameDeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

        MouseInputManager.Update();
        KeyboardInputManager.Update();

        if (_mainMenu.ExitAllowed) Exit();

        _cursor.Update();
        if (!(_mainMenu.State == MainMenu.MenuState.Hidden)) _mainMenu.Update(_gameDeltaTime);
        if (!(_mapSelectionMenu.State == MapSelectionMenu.MenuState.Hidden)) _mapSelectionMenu.Update(_gameDeltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        if (!(_mainMenu.State == MainMenu.MenuState.Hidden)) _mainMenu.Draw(_spriteBatch);
        
        _spriteBatch.Begin();

        if (!(_mapSelectionMenu.State == MapSelectionMenu.MenuState.Hidden)) _mapSelectionMenu.Draw(_spriteBatch);
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
