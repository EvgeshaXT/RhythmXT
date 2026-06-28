using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionButton
{
    Texture2D _texture;
    Vector2 _position;
    Vector2 _origin;
    internal MapSelectionButton(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth, screenHeight / 2);
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        _origin = new(_texture.Width, _texture.Height / 2);
    }

    internal void Update()
    {
        
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }
}
