using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT.MainMenu;

internal class MainMenuSettingsButton
{
    internal event Action ClickedEvent;

    SettingsHoverPlate _settingsHoverPlate;
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale;

    internal MainMenuSettingsButton(int screenWidth)
    {
        _position = new(screenWidth - 54, 54);
        _scale = 0.15f;

        _settingsHoverPlate = new(_position);
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/Settings");
        _origin = new(_texture.Width / 2, _texture.Height / 2);

        _settingsHoverPlate.LoadContent(content);
    }

    internal void Update()
    {
        _settingsHoverPlate.Update();
        if (_settingsHoverPlate.IsContainsCursor && MouseInputManager.MouseLeftButtonRePressed) Clicked();
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        if (_settingsHoverPlate.IsContainsCursor) _settingsHoverPlate.Draw(spriteBatch, color);
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, _scale, SpriteEffects.None, 0f);
    }

    internal void Clicked() => ClickedEvent?.Invoke();
}

// <======= HoverPlate =======> //
internal class SettingsHoverPlate
{
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale;
    
    Rectangle _area;
    internal bool IsContainsCursor { get; set; }

    internal SettingsHoverPlate(Vector2 position)
    {
        _position = position;
        _scale = 0.18f;

        IsContainsCursor = false;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/hoverPlate");
        _origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update() => ContainsCursor();

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, _scale, SpriteEffects.None, 0f);
    }

    void ContainsCursor()
    {
        int Width = (int)(_texture.Width * _scale);
        int Height = (int)(_texture.Height * _scale);

        _area = new((int)_position.X - Width / 2, 
                    (int)_position.Y - Height / 2, 
                    Width, 
                    Height
    );

        IsContainsCursor = _area.Contains(MouseInputManager.MousePosition);
    }
}
