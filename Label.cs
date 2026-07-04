using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class Label
{
    internal Font Font = "Yu Gothic, 18";
    internal string Text = "label";
    internal Vector2 Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
    internal Color Color = Color.White;
    internal Vector2 Origin = new(0, 0);
    internal float Scale = 1f;

    internal SpriteFont spriteFont;

    internal void LoadContent(ContentManager content)
    {
        spriteFont = content.Load<SpriteFont>((string)Font);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(spriteFont, Text, Position, Color, 0f, Origin, Scale, SpriteEffects.None, 0f);
    }
}

internal class Font
{
    internal string Name { get; set; } = "Yu Gothic";
    internal int Size { get; set; } = 18;

    public static implicit operator Font(string value)
    {
        string[] parts = value.Split(", ");
        return new Font()
        {
            Name = parts[0],
            Size = Convert.ToInt32(parts[1])
        };
    }

    public static explicit operator string(Font font) => $"{font.Name}, {font.Size}";
}