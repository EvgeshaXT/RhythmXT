using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Main : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    Stopwatch _stopwatch;
    double _deltaTime, _lastUpdateTime;

    Cursor _cursor;
    MainMenu _mainMenu;
    MapSelectionMenu _mapSelectionMenu;
    int _screenWidth, _screenHeight;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        _stopwatch = Stopwatch.StartNew();
        _deltaTime = 0f; _lastUpdateTime = 0f;

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
        UpdateDeltaTime();
        MouseInputManager.Update();
        KeyboardInputManager.Update();

        if (_mainMenu.ExitAllowed) Exit();

        _cursor.Update();
        if (!(_mainMenu.State == MainMenu.MenuState.Hidden)) _mainMenu.Update(_deltaTime);
        if (!(_mapSelectionMenu.State == MapSelectionMenu.MenuState.Hidden)) _mapSelectionMenu.Update(_deltaTime);

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

    void UpdateDeltaTime()
    {
        double nowUpdateTime = _stopwatch.Elapsed.TotalSeconds;
        _deltaTime = nowUpdateTime - _lastUpdateTime;
        _lastUpdateTime = nowUpdateTime;
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
