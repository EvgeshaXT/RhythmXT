using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;

    public MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
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
        _mainMenuXTCircle.Update();
        if (_mainMenuXTCircle.IsClicked) _mainMenuSoloButton.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuBackground.Draw(spriteBatch);
        _mainMenuXTCircle.Draw(spriteBatch);
        MainMenuSoloButtonDraw(spriteBatch);
    }

    void MainMenuSoloButtonDraw(SpriteBatch spriteBatch)
    {
        if (_mainMenuXTCircle.IsClicked) _mainMenuSoloButton.Draw(spriteBatch);
    }
}