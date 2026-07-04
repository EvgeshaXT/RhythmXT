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

    internal enum CirclePositionState { Centre, ToLeft, ToCentre }
    internal CirclePositionState StatePosition { get; private set; }
    internal event Action ClickedEvent;

    Texture2D _texture;
    internal int TextureWidth => _texture.Width;
    internal Vector2 Position { get; private set; }
    Vector2 newPosition;
    internal Vector2 Origin { get; private set; }
    internal float Scale { get; private set; }
    float newScale;
    float _speedAnimation;
    
    Stopwatch stopwatch;
    
    public MainMenuXTCircle()
    {
        StatePosition = CirclePositionState.Centre;
        Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
        newPosition = Position;

        Scale = 0.7f;
        newScale = Scale;
        _speedAnimation = SPEED_ANIMATION_CLICK;

        stopwatch = Stopwatch.StartNew();
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/mainMenuXTCircle");
        Origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Update(double gameDeltaTime, bool anyButtonHaveCursor)
    {
        if ((MouseInputManager.MouseLeftButtonRePressed && ContainsCursor()) || KeyboardInputManager.EnterRePressed) Clicked();

        mainMenuXTCircle_Animation(gameDeltaTime, anyButtonHaveCursor);
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, Position, null, color, 0, Origin, Scale, SpriteEffects.None, 0f);
    }

    internal void ResetToCentre()
    {
        Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);
        StatePosition = CirclePositionState.Centre;
        Scale = 0.7f;
    }

    internal bool ContainsCursor()
    {
        float dx = Position.X - MouseInputManager.MousePosition.X;
        float dy = Position.Y - MouseInputManager.MousePosition.Y;

        float radius = _texture.Width / 2f * Scale;

        return (dx * dx + dy * dy) <= (radius * radius);
    }

    internal void Clicked()
    {
        MouseInputManager.Handled = true;
        
        if (StatePosition == CirclePositionState.Centre)
        {
            StatePosition = CirclePositionState.ToLeft;
            ClickedEvent?.Invoke();
        }

        if (stopwatch.IsRunning) stopwatch.Restart();
    }

    void mainMenuXTCircle_Animation(double gameDeltaTime, bool anyButtonHaveCursor)
    {
        if (StatePosition == CirclePositionState.ToLeft)
        {
            if (ContainsCursor() || anyButtonHaveCursor) stopwatch.Reset();
            else stopwatch.Start();

            if (stopwatch.Elapsed.TotalSeconds >= DURATION)
            {
                stopwatch.Reset();

                StatePosition = CirclePositionState.ToCentre;
            }
        }

        UpdateDirectionForAnimation();


        if (Scale != newScale || Position != newPosition)
        {
            float lerpFactor = 1 - (float)Math.Exp(-_speedAnimation * gameDeltaTime);

            Position += (newPosition - Position) * lerpFactor;
            Scale += (newScale - Scale) * lerpFactor;

            if (Vector2.Distance(Position, newPosition) < 1f) 
            {
                Position = newPosition;
                if (StatePosition == CirclePositionState.ToCentre) StatePosition = CirclePositionState.Centre;
            }
            if (Math.Abs(Scale - newScale) < 0.001f) Scale = newScale;
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (StatePosition == CirclePositionState.ToLeft)
        {
            newPosition = new(GlobalScope.ScreenWidth / 3, GlobalScope.ScreenHeight / 2);
            newScale = 0.45f;
            _speedAnimation = SPEED_ANIMATION_CLICK;
        }
        else
        {
            newPosition = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2);

            if (ContainsCursor()) newScale = 0.75f;
            else newScale = 0.7f;

            _speedAnimation = SPEED_ANIMATION_ELSE;
        }
    }
}