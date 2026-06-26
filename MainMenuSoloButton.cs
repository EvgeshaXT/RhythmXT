using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RhytmXT;

internal class MainMenuSoloButton : IContainsCursor
{
    Texture2D _texture;
    Vector2 _position, newPosition, _origin;
    float _scale, newScale;
    int screenWidth, screenHeight;
    readonly float scaleNormal, scaleMousehover;
    Rectangle _area;
    internal bool IsAppearing { get; set; }
    internal bool IsDisappearing { get; set; }
    
    public MainMenuSoloButton(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 3f, screenHeight / 3.25f);
        newPosition = _position;

        _scale = 0.55f;
        newScale = _scale;
        scaleNormal = _scale;
        scaleMousehover = 0.6f;

        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;

        IsAppearing = false;
        IsDisappearing = false;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/Solo");
        _origin = new(0, _texture.Height / 2);
    }

    internal void Update(double deltaTime)
    {
        if (IsAppearing || IsDisappearing) mainMenuSolo_AppearingAnimation(deltaTime);

        mainMenuSolo_Mousehover(deltaTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
    }

    public bool ContainsCursor()
    {
        int Width = (int)(_texture.Width * _scale);
        int Height = (int)(_texture.Height * _scale);

        _area = new((int)_position.X,
                    (int)_position.Y - Height / 2,
                    Width,
                    Height);

        return _area.Contains(MouseInputManager.MousePosition);
    }

    void mainMenuSolo_Mousehover(double deltaTime)
    {
        if (ContainsCursor()) newScale = scaleMousehover;
        else newScale = scaleNormal;

        if (_scale != newScale)
        {
            float lerpFactor = 1 - (float)Math.Exp(-16f * deltaTime);

            _scale += (newScale - _scale) * lerpFactor;

            if (Math.Abs(_scale - newScale) < 0.001f) _scale = newScale;
        }
    }

    void mainMenuSolo_AppearingAnimation(double deltaTime)
    {
        UpdateDirectionForAnimation();

        if (_position != newPosition)
        {
            float lerpFactor = 1 - (float)Math.Exp(-16f * deltaTime);
            _position += (newPosition - _position) * lerpFactor;

            if (Vector2.Distance(_position, newPosition) < 1f) _position = newPosition;
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (IsAppearing)
        {
            newPosition = new(screenWidth / 2f, screenHeight / 3.25f);
        }
        else if (IsDisappearing)
        {
            newPosition = new(screenWidth / 2.75f, screenHeight / 3.25f);
        }
    }
}