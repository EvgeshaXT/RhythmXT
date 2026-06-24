using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RhytmXT;

public class MainMenuXTCircle
{
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale, newScale;
    float radius;

    DateTime nowDateTime;
    DateTime lastDateTime;
    
    public MainMenuXTCircle(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 2, screenHeight / 2);

        _scale = 0.7f;
        newScale = _scale;

        nowDateTime = DateTime.Now;
        lastDateTime = nowDateTime;
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
        nowDateTime = DateTime.Now;
        double deltaTime = (nowDateTime - lastDateTime).TotalSeconds;
        lastDateTime = nowDateTime;

        if (ContainsCursor(MouseInputManager.MousePosition)) newScale = 0.75f;
        else newScale = 0.7f;

        float lerpFactor = 1 - (float)Math.Exp(-8f * deltaTime);
        _scale += (newScale - _scale) * lerpFactor;
        
        if (Math.Abs(_scale - newScale) < 0.001f) _scale = newScale;
        CalculateRadius();
    }
}