using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMainButton
{
    string _songArtist, _songTitle;

    Texture2D _texture;
    SpriteFont _font;
    Vector2 _position, _positionArtist, _positionTitle;
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

        string[] parts = songName.Split(" - ");
        _songArtist = parts[0];
        _songTitle = parts[1];
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        _font = content.Load<SpriteFont>("DefaultFont");

        TextureHeight = _texture.Height;
        _origin = new(_texture.Width, TextureHeight / 2);
    }

    internal void Update()
    {
        _positionArtist.X = _position.X + 25;
        _positionArtist.Y = PositionY + 65;
        
        _positionTitle.X = _positionArtist.X;
        _positionTitle.Y = PositionY + 25;
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, _songArtist, _positionArtist, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, _songTitle, _positionTitle, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }
}
