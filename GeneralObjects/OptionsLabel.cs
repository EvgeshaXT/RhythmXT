using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.GeneralObjects;

class OptionsLabel
{
    internal enum OriginMode { LeftUp, Up, RightUp, Left, Centre, Right, LeftDown, Down, RightDown, Custom }

    event Action<int> SelectedIndexChanged;

    internal Font Font = "Yu Gothic, 18";
    internal Vector2 Position { get; set; } = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
    internal Color Color { get; set; } = Color.White;
    internal OriginMode Mode { get; set; } = OriginMode.Centre;
    internal Vector2 Origin { get; set; }
    internal float Scale { get; set; } = 1f;
    internal Vector2 Size { get; private set; }

    SpriteFont spriteFont;
    readonly string[] _options;
    int _selectedIndex;
    Vector2 _origin;

    internal OptionsLabel(string[] options, int selectedIndex = 0)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        if (_options.Length == 0) throw new ArgumentException("Options array cannot be empty.");

        _selectedIndex = Math.Clamp(selectedIndex, 0, _options.Length - 1);
        SelectedIndexChanged += _ => CalculateSize();
    }


    internal string SelectedText => _options[_selectedIndex];

    internal int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            int newIndex = Math.Clamp(value, 0, _options.Length - 1);
            if (newIndex == _selectedIndex) return;
            _selectedIndex = newIndex;
            SelectedIndexChanged?.Invoke(_selectedIndex);
        }
    }

    internal void LoadContent(ContentManager content)
    {
        spriteFont = content.Load<SpriteFont>((string)Font);
        CalculateSize();
    }

    internal void Update(int delta)
    {
        SelectedIndex = (_selectedIndex + delta + _options.Length) % _options.Length;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (spriteFont == null) return;
        spriteBatch.DrawString(spriteFont, SelectedText, Position, Color, 0f, _origin, Scale, SpriteEffects.None, 0f);
    }

    void CalculateSize()
    {
        if (spriteFont != null)
        {
            Size = spriteFont.MeasureString(SelectedText);
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