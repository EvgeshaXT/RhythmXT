using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;

    Stopwatch stopwatch;
    double deltaTime, lastUpdateTime;

    public MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        stopwatch = Stopwatch.StartNew();

        _mainMenuBackground = new(graphicsDevice, screenWidth, screenHeight);
        
        _mainMenuSoloButton = new(screenWidth, screenHeight);
        IContainsCursor[] clickables = [_mainMenuSoloButton];
        _mainMenuXTCircle = new(screenWidth, screenHeight, clickables);
    }

    public void LoadContent(ContentManager content)
    {
        _mainMenuBackground.LoadContent();
        _mainMenuXTCircle.LoadContent(content);
        _mainMenuSoloButton.LoadContent(content);
    }

    public void Update()
    {
        UpdateDeltaTime();

        _mainMenuXTCircle.Update(deltaTime);
        if (_mainMenuXTCircle.IsClicked) _mainMenuSoloButton.Update(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuBackground.Draw(spriteBatch);
        _mainMenuXTCircle.Draw(spriteBatch);
        MainMenuSoloButtonDraw(spriteBatch);
    }

    void UpdateDeltaTime()
    {
        double nowUpdateTime = stopwatch.Elapsed.TotalSeconds;
        deltaTime = nowUpdateTime - lastUpdateTime;
        lastUpdateTime = nowUpdateTime;
    }

    void MainMenuSoloButtonDraw(SpriteBatch spriteBatch)
    {
        if (_mainMenuXTCircle.IsClicked) _mainMenuSoloButton.Draw(spriteBatch);
    }
}