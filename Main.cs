using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;
using RhythmXT.MainMenu;
using RhythmXT.MapSelectionMenu;
namespace RhythmXT;

class Main : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;

    MainMenuManager _mainMenuManager;
    MapSelectionMenuManager _mapSelectionMenuManager;
    int _screenWidth, _screenHeight;

    internal Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";    

        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 1000.0);
        IsFixedTimeStep = true;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        FullScreen(_graphics);

        _screenWidth = GraphicsDevice.Viewport.Width;
        _screenHeight = GraphicsDevice.Viewport.Height;
        GlobalScope.Initialize(_screenWidth, _screenHeight);

        Settings.Load();

        _mainMenuManager = new(GraphicsDevice);
        _mapSelectionMenuManager = new();

        _mainMenuManager.ClickedEvent += SoundEffects.Click;
        _mainMenuManager.ClickAnimationIsFinishedEvent += () => _mapSelectionMenuManager.Show();

        _mapSelectionMenuManager.ToMainMenuEvent += () => _mainMenuManager.Show();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundEffects.LoadContent(Content);

        _mainMenuManager.LoadContent(Content);
        _mapSelectionMenuManager.LoadContent(Content, GraphicsDevice);
        Cursor.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        MouseInputManager.Update();
        KeyboardInputManager.Update();

        if (_mainMenuManager.ExitAllowed) Exit();

        Cursor.Update();
        if (_mainMenuManager.State != MainMenuManager.MenuState.Hidden) _mainMenuManager.Update(gameTime.ElapsedGameTime.TotalSeconds);
        if (_mapSelectionMenuManager.State != MapSelectionMenuManager.MenuState.Hidden) _mapSelectionMenuManager.Update(gameTime.ElapsedGameTime.TotalSeconds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        if (!(_mainMenuManager.State == MainMenuManager.MenuState.Hidden)) _mainMenuManager.Draw(_spriteBatch);
        
        _spriteBatch.Begin();

        if (!(_mapSelectionMenuManager.State == MapSelectionMenuManager.MenuState.Hidden)) _mapSelectionMenuManager.Draw(_spriteBatch);
        Cursor.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    static void FullScreen(GraphicsDeviceManager graphics)
    {
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

        graphics.PreferredBackBufferWidth = displayMode.Width;
        graphics.PreferredBackBufferHeight = displayMode.Height;

        graphics.IsFullScreen = true;
        graphics.ApplyChanges();
    }
}
