using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Cursor
{
    Texture2D _texture;
    Vector2 _position, _origin;

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("cursor");
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update() => _position = MouseInputManager.MousePosition;

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, 1f, SpriteEffects.None, 0f);
    }
}