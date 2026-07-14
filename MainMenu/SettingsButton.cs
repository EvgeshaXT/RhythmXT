using System;
using static System.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT.MainMenu;

class MainMenuSettingsButton
{
    internal event Action ClickedEvent;

    SettingsHoverPlate _settingsHoverPlate;
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale;

    internal MainMenuSettingsButton(GraphicsDevice graphicsDevice)
    {
        _position = new(GlobalScope.ScreenWidth - 54, 54);
        _scale = 0.15f;

        _settingsHoverPlate = new(graphicsDevice, _position);
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/Settings");
        _origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update()
    {
        if (!MouseInputManager.Handled)
        {
            _settingsHoverPlate.Update();
            if (_settingsHoverPlate.IsContainsCursor && MouseInputManager.MouseLeftButtonRePressed) Clicked();
        }
        else _settingsHoverPlate.IsContainsCursor = false;
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        if (_settingsHoverPlate.IsContainsCursor) _settingsHoverPlate.Draw(spriteBatch, color);
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, _scale, SpriteEffects.None, 0f);
    }

    internal void Clicked() => ClickedEvent?.Invoke();
}

// <======= HoverPlate =======> //
class SettingsHoverPlate
{
    Texture2D _texture;
    Vector2 _position, _origin;
    
    Rectangle _area;
    internal bool IsContainsCursor { get; set; }

    internal SettingsHoverPlate(GraphicsDevice graphicsDevice, Vector2 position)
    {
        _position = position;

        IsContainsCursor = false;

        _texture = GetTexture(graphicsDevice, 98, 98, 22f, new Color(55, 55, 55, 128));
        _origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update() => ContainsCursor();

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, 1f, SpriteEffects.None, 0f);
    }

    Texture2D GetTexture(GraphicsDevice graphicsDevice, int width, int height, float radius, Color color)
    {
        Texture2D texture = new(graphicsDevice, width, height);
        Color[] colors = new Color[width * height];

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        float sizeX = halfWidth - radius;
        float sizeY = halfHeight - radius;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float displacedX = x - halfWidth;
                float displacedY = y - halfHeight;

                // Working only in one part of rectangle
                float absX = Abs(displacedX);
                float absY = Abs(displacedY);

                // vector to Up Right Corner
                float dx = absX - sizeX;
                float dy = absY - sizeY;

                double outsideDist = Sqrt(Max(dx, 0)*Max(dx, 0) + Max(dy, 0)*Max(dy, 0));
                double insideDist = Min(Max(dx, dy), 0);
                float dist = (float)(outsideDist + insideDist - radius);

                // Smoothing
                float a = 1f - MathHelper.Clamp(dist + 0.5f, 0f, 1f);
                colors[y * width + x] = color  * a;
            }
        }

        texture.SetData(colors);
        return texture;
    }

    void ContainsCursor()
    {
        int Width = _texture.Width;
        int Height = _texture.Height;

        _area = new((int)_position.X - Width / 2, 
                    (int)_position.Y - Height / 2, 
                    Width, 
                    Height
    );

        IsContainsCursor = _area.Contains(MouseInputManager.MousePosition);
    }
}
