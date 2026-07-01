using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;
using RhythmXT.MainMenu;
using RhythmXT.MapSelectionMenu;

namespace RhythmXT;

internal class Main : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    double _gameDeltaTime;

    Cursor _cursor;
    MainMenuManager _mainMenuManager;
    MapSelectionMenuManager _mapSelectionMenuManager;
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

        _mainMenuManager = new(GraphicsDevice, _screenWidth, _screenHeight);
        _mapSelectionMenuManager = new(_screenWidth, _screenHeight);

        _mainMenuManager.ClickedEvent += SoundEffects.Click;
        _mainMenuManager.ClickAnimationIsFinishedEvent += () => _mapSelectionMenuManager.Show();

        _mapSelectionMenuManager.ToMainMenuEvent += () => _mainMenuManager.Show();

        _cursor = new();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundEffects.LoadContent(Content);

        _mainMenuManager.LoadContent(Content);
        _mapSelectionMenuManager.LoadContent(Content, GraphicsDevice);
        _cursor.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _gameDeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

        MouseInputManager.Update();
        KeyboardInputManager.Update();

        if (_mainMenuManager.ExitAllowed) Exit();

        _cursor.Update();
        if (!(_mainMenuManager.State == MainMenuManager.MenuState.Hidden)) _mainMenuManager.Update(_gameDeltaTime);
        if (!(_mapSelectionMenuManager.State == MapSelectionMenuManager.MenuState.Hidden)) _mapSelectionMenuManager.Update(_gameDeltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        if (!(_mainMenuManager.State == MainMenuManager.MenuState.Hidden)) _mainMenuManager.Draw(_spriteBatch);
        
        _spriteBatch.Begin();

        if (!(_mapSelectionMenuManager.State == MapSelectionMenuManager.MenuState.Hidden)) _mapSelectionMenuManager.Draw(_spriteBatch);
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
