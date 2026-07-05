using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MainMenu.Settings;

class Background
{
    Texture2D _texture;
    Rectangle _rectangle;

    internal Background()
    {
        int width = 640;
        int height = 192;

        _rectangle = new Rectangle((GlobalScope.ScreenWidth - width) / 2, (GlobalScope.ScreenHeight - height) / 2, width, height);
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("formBackground");
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangle, Color.White);
    }
}