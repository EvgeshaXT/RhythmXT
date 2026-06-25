using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System; // Math
using System.Diagnostics; // Stopwatch only
using System.Linq;

namespace RhytmXT;

public class MainMenuXTCircle : IContainsCursor
{
    readonly double DURATION = 10d;
    readonly float SPEED_ANIMATION_CLICK = 12f;
    readonly float SPEED_ANIMATION_ELSE = 8f;

    IContainsCursor[] containsCursors;

    Texture2D _texture;
    public int TextureWidth => _texture.Width;
    public Vector2 Position { get; private set; }
    Vector2 newPosition;
    public Vector2 Origin { get; private set; }
    int screenWidth, screenHeight;
    public float Scale { get; private set; }
    float newScale;
    float _speedAnimation;
    
    Stopwatch stopwatch;

    public bool IsClicked { get; private set; }
    public bool IsUnClicked { get; private set; }
    
    public MainMenuXTCircle(int screenWidth, int screenHeight, IContainsCursor[] containsCursors)
    {
        Position = new(screenWidth / 2, screenHeight / 2);
        newPosition = Position;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.containsCursors = containsCursors;

        Scale = 0.7f;
        newScale = Scale;
        _speedAnimation = SPEED_ANIMATION_CLICK;

        stopwatch = Stopwatch.StartNew();

        IsClicked = false;
        IsUnClicked = false;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/mainMenuXTCircle");
        Origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    public void Update(double deltaTime)
    {
        mainMenuXTCircle_Clicked(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, null, Color.White, 0, Origin, Scale, SpriteEffects.None, 0f);
    }

    public bool ContainsCursor()
    {
        float dx = Position.X - MouseInputManager.MousePosition.X;
        float dy = Position.Y - MouseInputManager.MousePosition.Y;

        float radius = _texture.Width / 2f * Scale;

        return (dx * dx + dy * dy) <= (radius * radius);
    }

    void mainMenuXTCircle_Clicked(double deltaTime)
    {
        if (MouseInputManager.MouseLeftClickPressed && ContainsCursor())
        {
            IsClicked = true;
            IsUnClicked = false;

            if (stopwatch.IsRunning) stopwatch.Restart();
        }

        mainMenuXTCircle_Animation(deltaTime);
    }

    void mainMenuXTCircle_Animation(double deltaTime)
    {
        if (IsClicked)
        {
            if (ContainsCursor() || containsCursors.Any(c => c.ContainsCursor())) stopwatch.Reset();
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
            float lerpFactor = 1 - (float)Math.Exp(-_speedAnimation * deltaTime);

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