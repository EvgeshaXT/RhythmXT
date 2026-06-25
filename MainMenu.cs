using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;
    MainMenuMultiButton _mainMenuMultiButton;

    Stopwatch stopwatch;
    double deltaTime, lastUpdateTime;

    public MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        stopwatch = Stopwatch.StartNew();

        _mainMenuBackground = new(graphicsDevice, screenWidth, screenHeight);
        
        _mainMenuSoloButton = new(screenWidth, screenHeight);
        _mainMenuMultiButton = new(screenWidth, screenHeight);
        IContainsCursor[] clickables = [_mainMenuSoloButton, _mainMenuMultiButton];
        _mainMenuXTCircle = new(screenWidth, screenHeight, clickables);
    }

    public void LoadContent(ContentManager content)
    {
        _mainMenuBackground.LoadContent();
        _mainMenuXTCircle.LoadContent(content);
        _mainMenuSoloButton.LoadContent(content);
        _mainMenuMultiButton.LoadContent(content);
    }

    public void Update()
    {
        UpdateDeltaTime();

        _mainMenuXTCircle.Update(deltaTime);
        MainMenuButtonsUpdate();

        if (_mainMenuXTCircle.IsClicked) _mainMenuMultiButton.Update(deltaTime);
    }

    void MainMenuButtonsUpdate()
    {
        if (_mainMenuXTCircle.IsClicked)
        {
            if (!_mainMenuSoloButton.IsAppearing)
            {
                _mainMenuSoloButton.IsAppearing = true;
                _mainMenuSoloButton.IsDisappearing = false;
            }

            _mainMenuSoloButton.Update(deltaTime);
        }
        else if (_mainMenuXTCircle.IsUnClicked)
        {
            if (!_mainMenuSoloButton.IsDisappearing)
            {
                _mainMenuSoloButton.IsAppearing = false;
                _mainMenuSoloButton.IsDisappearing = true;
            }

            _mainMenuSoloButton.Update(deltaTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuBackground.Draw(spriteBatch);
        MainMenuButtonsDraw(spriteBatch);
        _mainMenuXTCircle.Draw(spriteBatch);
    }

    void MainMenuButtonsDraw(SpriteBatch spriteBatch)
    {
        if (_mainMenuSoloButton.IsAppearing || (_mainMenuSoloButton.IsDisappearing && _mainMenuXTCircle.IsUnClicked))
        {
            _mainMenuSoloButton.Draw(spriteBatch);
            _mainMenuMultiButton.Draw(spriteBatch);
        }
    }

    void UpdateDeltaTime()
    {
        double nowUpdateTime = stopwatch.Elapsed.TotalSeconds;
        deltaTime = nowUpdateTime - lastUpdateTime;
        lastUpdateTime = nowUpdateTime;
    }
}