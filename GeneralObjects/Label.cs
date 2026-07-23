using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.GeneralObjects;

class Label
{
    internal enum OriginMode { LeftUp, Up, RightUp, Left, Centre, Right, LeftDown, Down, RightDown, Custom }
    event Action TextChanged;

    internal Font Font = "Yu Gothic, 18";
    internal string Text
    {
        get => _text;
        set
        {
            _text = value;
            TextChanged?.Invoke();
        }
    }
    internal Vector2 Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
    internal Color Color = Color.White;
    internal OriginMode Mode = OriginMode.Centre;
    internal Vector2 Origin { get; set; }
    internal float Scale = 1f;
    internal Vector2 Size { get; private set; }

    SpriteFont spriteFont;
    string _text = "label";
    Vector2 _origin;

    internal Label()
    {
        TextChanged += CalculateSize;
    }

    internal void LoadContent(ContentManager content)
    {
        spriteFont = content.Load<SpriteFont>((string)Font);
        CalculateSize();
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(spriteFont, _text, Position, Color, 0f, _origin, Scale, SpriteEffects.None, 0f);
    }

    void CalculateSize()
    {
        if (spriteFont != null)
        {
            Size = spriteFont.MeasureString(Text);
            UpdateOrigin();
        }
    }

    void UpdateOrigin()
    {
        switch (Mode)
        {
            case OriginMode.LeftUp: _origin = new(0, 0); break;
            case OriginMode.Up: _origin = new(Size.X / 2, 0); break;
            case OriginMode.RightUp: _origin = new(Size.X, 0); break;
            case OriginMode.Left: _origin = new(0, Size.Y / 2); break;
            case OriginMode.Centre: _origin = new(Size.X / 2, Size.Y / 2); break;
            case OriginMode.Right: _origin = new(Size.X, Size.Y / 2); break;
            case OriginMode.LeftDown: _origin = new(0, Size.Y); break;
            case OriginMode.Down: _origin = new(Size.X / 2, Size.Y); break;
            case OriginMode.RightDown: _origin = new(Size.X, Size.Y); break;
            case OriginMode.Custom: _origin = Origin; break;
        }
    }
}