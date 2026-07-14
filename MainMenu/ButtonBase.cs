using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using RhythmXT.Input;

namespace RhythmXT.MainMenu;

abstract class MainMenuButtonBase
{
    internal Vector2 CLICKED_POSITION;
    internal Vector2 HIDDEN_POSITION;
    internal float SPEED_CLICK_ANIMATION;
    internal string _textureName;

    internal enum ButtonState { Hidden, Appearing, Visible, Disappearing }
    internal event Action ClickedEvent;

    Texture2D _texture;
    Vector2 _position, newPosition, _origin;
    float _scale, newScale;
    readonly float scaleNormal, scaleMousehover;
    Rectangle _area;
    bool _isInsideOfCircle;
    internal bool IsContainsCursor { get; private set; }
    internal ButtonState State { get; private set; }

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

        _isInsideOfCircle = false;
        IsContainsCursor = false;
        State = ButtonState.Hidden;
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

    internal void Update(double gameDeltaTime, bool isInsideOfCircle)
    {
        _isInsideOfCircle = isInsideOfCircle;
        if (State == ButtonState.Appearing || State == ButtonState.Disappearing) AppearingAnimation(gameDeltaTime);
        ContainsCursor();

        if (State != ButtonState.Hidden && !MouseInputManager.Handled)
        {
            Mousehover(gameDeltaTime);
            if (IsContainsCursor && MouseInputManager.MouseLeftButtonRePressed && !MouseInputManager.Handled) Clicked();
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
    }

    internal void Show()
    {
        if (State == ButtonState.Disappearing || State == ButtonState.Hidden) State = ButtonState.Appearing;
    }
    internal void Hide()
    {
        if (State == ButtonState.Appearing || State == ButtonState.Visible) State = ButtonState.Disappearing;
    }
    internal void HideForced()
    {
        _position = HIDDEN_POSITION;
        newPosition = HIDDEN_POSITION;
        State = ButtonState.Hidden;
    }

    internal void ContainsCursor()
    {
        if (_isInsideOfCircle) return;

        int Width = (int)(_texture.Width * _scale);
        int Height = (int)(_texture.Height * _scale);

        _area = new((int)_position.X,
                    (int)_position.Y - Height / 2,
                    Width,
                    Height);

        IsContainsCursor = _area.Contains(MouseInputManager.MousePosition);
    }

    void Mousehover(double gameDeltaTime)
    {
        if (IsContainsCursor) newScale = scaleMousehover;
        else newScale = scaleNormal;

        if (_scale != newScale)
        {
            float lerpFactor = 1 - (float)Math.Exp(-16f * gameDeltaTime);

            _scale += (newScale - _scale) * lerpFactor;

            if (Math.Abs(_scale - newScale) < 0.001f) _scale = newScale;
        }
    }

    void AppearingAnimation(double gameDeltaTime)
    {
        UpdateDirectionForAnimation();

        if (_position != newPosition)
        {
            float lerpFactor = 1 - (float)Math.Exp(-SPEED_CLICK_ANIMATION * gameDeltaTime);
            _position += (newPosition - _position) * lerpFactor;

            if (Vector2.Distance(_position, newPosition) < 1f)
            {
                _position = newPosition;
                
                if (State == ButtonState.Appearing) State = ButtonState.Visible;
                else if (State == ButtonState.Disappearing) State = ButtonState.Hidden;
            }
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (State == ButtonState.Appearing) newPosition = CLICKED_POSITION;
        else if (State == ButtonState.Disappearing) newPosition = HIDDEN_POSITION;
    }

    internal void Clicked() => ClickedEvent?.Invoke();
}