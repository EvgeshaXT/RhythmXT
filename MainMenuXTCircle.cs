using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System; // Math
using System.Diagnostics; // Stopwatch only

namespace RhytmXT;

public class MainMenuXTCircle
{
    Texture2D _texture;
    Vector2 _position, newPosition, _origin;
    int screenWidth, screenHeight;
    float _scale, newScale;
    float radius;
    
    Stopwatch stopwatch, stopwatchMainMenuXTCircle_ClickedAnimation;
    double deltaTime, lastUpdateTime;

    bool _isClicked, _isUnClicked;
    
    public MainMenuXTCircle(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 2, screenHeight / 2);
        newPosition = _position;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;

        _scale = 0.7f;
        newScale = _scale;

        stopwatch = Stopwatch.StartNew();
        stopwatchMainMenuXTCircle_ClickedAnimation = new();
        deltaTime = 0d; lastUpdateTime = 0d;

        _isClicked = false;
        _isUnClicked = false;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("mainMenuXTCircle");
        _origin = new(_texture.Width / 2, _texture.Height / 2);
        CalculateRadius();
    }

    public void Update()
    {
        UpdateDeltaTime();

        mainMenuXTCircle_Clicked();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
    }

    bool ContainsCursor(Vector2 cursorPosition)
    {
        float distance = Vector2.Distance(_position, cursorPosition);

        return distance <= radius;
    }

    void CalculateRadius() => radius = _texture.Width / 2 * _scale;

    void UpdateDeltaTime()
    {
        double nowUpdateTime = stopwatch.Elapsed.TotalSeconds;
        deltaTime = nowUpdateTime - lastUpdateTime;
        lastUpdateTime = nowUpdateTime;
    }

    void mainMenuXTCircle_Clicked()
    {
        if (MouseInputManager.MouseLeftClickPressed && ContainsCursor(MouseInputManager.MousePosition))
        {
            _isClicked = true;
            _isUnClicked = false;

            if (stopwatchMainMenuXTCircle_ClickedAnimation.IsRunning) stopwatchMainMenuXTCircle_ClickedAnimation.Restart();
        }

        mainMenuXTCircle_Animation();
    }

    void mainMenuXTCircle_Animation()
    {
        if (_isClicked)
        {
            if (MouseInputManager.MouseLeftClickPrReleased) stopwatchMainMenuXTCircle_ClickedAnimation.Start();

            if (stopwatchMainMenuXTCircle_ClickedAnimation.Elapsed.TotalSeconds >= 5d)
            {
                stopwatchMainMenuXTCircle_ClickedAnimation.Reset();

                _isUnClicked = true;
                _isClicked = false;
            }
        }

        UpdateDirectionForAnimation();


        if (_scale != newScale || _position != newPosition)
        {
            CalculateRadius();

            float lerpFactor = 1 - (float)Math.Exp(-12f * deltaTime);

            _position += (newPosition - _position) * lerpFactor;
            _scale += (newScale - _scale) * lerpFactor;

            if (Vector2.Distance(_position, newPosition) < 1f) _position = newPosition;
            if (Math.Abs(_scale - newScale) < 0.001f) _scale = newScale;

            if (_isUnClicked && _position == newPosition) _isUnClicked = false;
        }
    }

    void UpdateDirectionForAnimation()
    {
        if (_isClicked)
        {
            newPosition = new(screenWidth / 3, screenHeight / 2);
            newScale = 0.45f;
        }
        else
        {
            newPosition = new(screenWidth / 2, screenHeight / 2);

            if (ContainsCursor(MouseInputManager.MousePosition)) newScale = 0.75f;
            else newScale = 0.7f;
        }
    }
}