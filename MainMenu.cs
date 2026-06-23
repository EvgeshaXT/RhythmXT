using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenu
{
    MainMenuXTCircle _mainMenuXTCircle;

    public MainMenu(int screenWidth, int screenHeight)
    {
        _mainMenuXTCircle = new(screenWidth, screenHeight);
    }

    public void LoadContent(ContentManager content)
    {
        _mainMenuXTCircle.LoadContent(content);
    }

    public void Update()
    {
        _mainMenuXTCircle.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuXTCircle.Draw(spriteBatch);
    }
}