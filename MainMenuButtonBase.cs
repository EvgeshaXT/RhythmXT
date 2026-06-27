using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RhytmXT;

internal abstract class MainMenuButtonBase : IContainsCursor
{
    internal Vector2 CLICKED_POSITION;
    internal Vector2 HIDDEN_POSITION;
    internal float SPEED_CLICK_ANIMATION;
    internal string _textureName;

    Texture2D _texture;
    Vector2 _position, newPosition, _origin;
    float _scale, newScale;
    readonly float scaleNormal, scaleMousehover;
    Rectangle _area;
    internal bool IsAppearing { get; set; }
    internal bool IsDisappearing { get; set; }

    protected MainMenuButtonBase()
    {
        CLICKED_POSITION = Vector2.Zero;
        HIDDEN_POSITION = Vector2.Zero;

        _position = Vector2.Zero;
        newPosition = Vector2.Zero;

        _scale = 0.55f;
        newScale = _scale;
        scaleNormal = _scale;
        scaleMousehover = 0.6f;

        IsAppearing = false;
        IsDisappearing = false;
    }

    internal void Initialize()
    {
        _position = HIDDEN_POSITION;
        newPosition = _position;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>(_textureName);
        _origin = new(0, _texture.Height / 2);
    }

    internal void Update(double deltaTime)
    {
        if (IsAppearing || IsDisappearing) AppearingAnimation(deltaTime);

        Mousehover(deltaTime);
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

    void Mousehover(double deltaTime)
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

    void AppearingAnimation(double deltaTime)
    {
        UpdateDirectionForAnimation();

        if (_position != newPosition)
        {
            float lerpFactor = 1 - (float)Math.Exp(-SPEED_CLICK_ANIMATION * deltaTime);
            _position += (newPosition - _position) * lerpFactor;

            if (Vector2.Distance(_position, newPosition) < 1f) _position = newPosition;
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (IsAppearing)
        {
            newPosition = CLICKED_POSITION;
        }
        else if (IsDisappearing)
        {
            newPosition = HIDDEN_POSITION;
        }
    }
}