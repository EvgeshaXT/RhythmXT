using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT;

internal static class Cursor
{
    static Texture2D _texture;
    static Vector2 _position, _origin;

    internal static void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("cursor");
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
    }

    internal static void Update() => _position = MouseInputManager.MousePosition;

    internal static void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, 1f, SpriteEffects.None, 0f);
    }
}