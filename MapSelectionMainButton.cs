using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMainButton
{
    string _songName;

    Texture2D _texture;
    Vector2 _position;
    Vector2 _origin;
    static internal float TextureHeight { get; set; }
    internal float PositionY
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    internal MapSelectionMainButton(int screenWidth, int screenHeight, string songName)
    {
        _position = new(screenWidth, screenHeight / 2);
        _songName = songName;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        TextureHeight = _texture.Height;
        _origin = new(_texture.Width, TextureHeight / 2);
    }

    internal void Update()
    {
        
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }
}
