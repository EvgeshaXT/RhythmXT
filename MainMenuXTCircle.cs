using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace RhytmXT;

public class MainMenuXTCircle
{
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale, newScale;
    float radius;
    
    Stopwatch stopwatch;
    double lastUpdateTime;
    
    public MainMenuXTCircle(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 2, screenHeight / 2);

        _scale = 0.7f;
        newScale = _scale;

        stopwatch = Stopwatch.StartNew();
        lastUpdateTime = 0d;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("mainMenuXTCircle");
        _origin = new(_texture.Width / 2, _texture.Height / 2);
        CalculateRadius();
    }

    public void Update()
    {    
        ScaleUpdate();
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

    void ScaleUpdate()
    {
        double nowUpdateTime = stopwatch.Elapsed.TotalSeconds;
        double deltaTime = nowUpdateTime - lastUpdateTime;
        lastUpdateTime = nowUpdateTime;

        if (ContainsCursor(MouseInputManager.MousePosition)) newScale = 0.75f;
        else newScale = 0.7f;

        float lerpFactor = 1 - (float)Math.Exp(-8f * deltaTime);
        _scale += (newScale - _scale) * lerpFactor;
        
        if (Math.Abs(_scale - newScale) < 0.001f) _scale = newScale;
        CalculateRadius();
    }
}