using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;

    public MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        _mainMenuBackground = new(graphicsDevice, screenWidth, screenHeight);
        _mainMenuXTCircle = new(screenWidth, screenHeight);
    }

    public void LoadContent(ContentManager content)
    {
        _mainMenuBackground.LoadContent();
        _mainMenuXTCircle.LoadContent(content);
    }

    public void Update()
    {
        _mainMenuXTCircle.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuBackground.Draw(spriteBatch);
        _mainMenuXTCircle.Draw(spriteBatch);
    }
}