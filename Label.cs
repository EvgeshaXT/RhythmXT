using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Label()
{
    internal string Font { get; set; } = "Yu Gothic 18";
    internal string Text { get; set; } = "label";
    internal Vector2 Position { get; set; } = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
    internal Color Color { get; set; } = Color.White;
    internal Vector2 Origin { get; set; }
    internal float Scale { get; set; } = 1f;

    // SpriteFont spriteFont;

    /*internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_fontArtist, _songArtist, _positionArtist, _artistColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }*/
}