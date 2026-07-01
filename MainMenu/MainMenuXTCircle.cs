using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System; // Math
using System.Diagnostics; // Stopwatch only
using RhythmXT.Input;

namespace RhythmXT.MainMenu;

internal class MainMenuXTCircle
{
    readonly double DURATION = 10d;
    readonly float SPEED_ANIMATION_CLICK = 12f;
    readonly float SPEED_ANIMATION_ELSE = 8f;

    internal event Action ClickedEvent;

    Texture2D _texture;
    internal int TextureWidth => _texture.Width;
    internal Vector2 Position { get; private set; }
    Vector2 newPosition;
    internal Vector2 Origin { get; private set; }
    int screenWidth, screenHeight;
    internal float Scale { get; private set; }
    float newScale;
    float _speedAnimation;
    
    Stopwatch stopwatch;

    internal bool IsClicked { get; private set; }
    internal bool IsUnClicked { get; private set; }
    
    public MainMenuXTCircle(int screenWidth, int screenHeight)
    {
        Position = new(screenWidth / 2, screenHeight / 2);
        newPosition = Position;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;

        Scale = 0.7f;
        newScale = Scale;
        _speedAnimation = SPEED_ANIMATION_CLICK;

        stopwatch = Stopwatch.StartNew();

        IsClicked = false;
        IsUnClicked = false;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/mainMenuXTCircle");
        Origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update(double gameDeltaTime, bool anyButtonHaveCursor)
    {
        mainMenuXTCircle_Clicked(gameDeltaTime, anyButtonHaveCursor);
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, Position, null, color, 0, Origin, Scale, SpriteEffects.None, 0f);
    }

    internal void ResetToCentre()
    {
        IsUnClicked = true;
        IsClicked = false;
        Position = new(screenWidth / 2, screenHeight / 2);
        Scale = 0.7f;
    }

    internal bool ContainsCursor()
    {
        float dx = Position.X - MouseInputManager.MousePosition.X;
        float dy = Position.Y - MouseInputManager.MousePosition.Y;

        float radius = _texture.Width / 2f * Scale;

        return (dx * dx + dy * dy) <= (radius * radius);
    }

    void mainMenuXTCircle_Clicked(double gameDeltaTime, bool anyButtonHaveCursor)
    {
        if ((MouseInputManager.MouseLeftButtonRePressed && ContainsCursor()) || KeyboardInputManager.EnterRePressed)
        {
            MouseInputManager.Handled = true;
            
            if (!IsClicked)
            {
                IsClicked = true;
                ClickedEvent?.Invoke();
            }
            IsUnClicked = false;

            if (stopwatch.IsRunning) stopwatch.Restart();
        }

        mainMenuXTCircle_Animation(gameDeltaTime, anyButtonHaveCursor);
    }

    void mainMenuXTCircle_Animation(double gameDeltaTime, bool anyButtonHaveCursor)
    {
        if (IsClicked)
        {
            if (ContainsCursor() || anyButtonHaveCursor) stopwatch.Reset();
            else stopwatch.Start();

            if (stopwatch.Elapsed.TotalSeconds >= DURATION)
            {
                stopwatch.Reset();

                IsUnClicked = true;
                IsClicked = false;
            }
        }

        UpdateDirectionForAnimation();


        if (Scale != newScale || Position != newPosition)
        {
            float lerpFactor = 1 - (float)Math.Exp(-_speedAnimation * gameDeltaTime);

            Position += (newPosition - Position) * lerpFactor;
            Scale += (newScale - Scale) * lerpFactor;

            if (Vector2.Distance(Position, newPosition) < 1f) Position = newPosition;
            if (Math.Abs(Scale - newScale) < 0.001f) Scale = newScale;

            if (IsUnClicked && Position == newPosition) IsUnClicked = false;
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (IsClicked)
        {
            newPosition = new(screenWidth / 3, screenHeight / 2);
            newScale = 0.45f;
            _speedAnimation = SPEED_ANIMATION_CLICK;
        }
        else
        {
            newPosition = new(screenWidth / 2, screenHeight / 2);

            if (ContainsCursor()) newScale = 0.75f;
            else newScale = 0.7f;

            _speedAnimation = SPEED_ANIMATION_ELSE;
        }
    }
}