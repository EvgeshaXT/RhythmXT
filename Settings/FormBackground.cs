using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.Settings;

internal class FormBackground
{
    Texture2D _texture;
    Rectangle _rectangle;

    internal FormBackground(int screenWidth, int screenHeight)
    {
        int width = 1024;
        int height = 1024;

        _rectangle = new Rectangle((screenWidth - width) / 2, (screenHeight - height) / 2, width, height);
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