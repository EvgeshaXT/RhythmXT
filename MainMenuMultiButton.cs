using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RhytmXT;

public class MainMenuMultiButton : IContainsCursor
{
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale, newScale;
    readonly float scaleNormal, scaleMousehover;
    Rectangle _area;
    
    public MainMenuMultiButton(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 2f + 25f, screenHeight / 2f);
        _scale = 0.55f;
        newScale = _scale;
        scaleNormal = _scale;
        scaleMousehover = 0.6f;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/Multi");
        _origin = new(0, _texture.Height / 2);
        UpdateArea();
    }

    public void Update(double deltaTime)
    {
        mainMenuSolo_Mousehover(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
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

        UpdateArea();
    }

    public bool ContainsCursor() =>_area.Contains(MouseInputManager.MousePosition);

    void UpdateArea()
    {
        int Width = (int)(_texture.Width * _scale);
        int Height = (int)(_texture.Height * _scale);

        _area = new((int)_position.X,
                    (int)_position.Y - Height / 2,
                    Width,
                    Height);
    }
}