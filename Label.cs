using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Label
{
    internal string Font { get; set; } = "Yu Gothic 18";
    internal string Text { get; set; } = "label";
    internal Vector2 Position { get; set; } = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
    internal Color Color { get; set; } = Color.White;
    internal Vector2 Origin { get; set; }
    internal float Scale { get; set; } = 1f;

    SpriteFont spriteFont;

    internal void LoadContent(ContentManager content)
    {
        spriteFont = content.Load<SpriteFont>(Font);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(spriteFont, Text, Position, Color, 0f, Origin, Scale, SpriteEffects.None, 0f);
    }
}