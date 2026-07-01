using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MapSelectionMenu;

internal class MapSelectionMainButton
{
    int screenHeightHalf;
    string _songArtist, _songArtistName, _songTitleName;

    Texture2D _texture;
    SpriteFont _fontArtist, _fontTitle;
    Vector2 _position, _positionArtist, _positionArtistName, _positionTitleName;
    Color _defaultColor, _artistColor;
    Vector2 _origin;
    static internal float TextureHeight { get; set; }
    internal float PositionY
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    internal MapSelectionMainButton(int screenWidth, int screenHeight, string songName)
    {
        screenHeightHalf = screenHeight / 2;
        _position = new(screenWidth, screenHeightHalf);

        string[] parts = songName.Split(" - ");
        _songArtist = "Artist: ";
        _songArtistName = parts[0];
        _songTitleName = parts[1];
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        _fontArtist = content.Load<SpriteFont>("FontComicSansMS18");
        _fontTitle = content.Load<SpriteFont>("FontComicSansMS22");

        TextureHeight = _texture.Height;
        _origin = new(_texture.Width, TextureHeight / 2);
    }

    internal void Update(float height, Color color)
    {
        PositionY = screenHeightHalf + height;
        
        _positionTitleName.X = _position.X + 30;
        _positionTitleName.Y = PositionY + 15;

        _positionArtist.X = _positionTitleName.X;
        _positionArtist.Y = PositionY + 50;

        _positionArtistName.X = _positionArtist.X + 75;
        _positionArtistName.Y = _positionArtist.Y;

        _defaultColor = color;
        _artistColor = color * 0.75f;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, _defaultColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_fontTitle, _songTitleName, _positionTitleName, _defaultColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_fontArtist, _songArtist, _positionArtist, _artistColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_fontArtist, _songArtistName, _positionArtistName, _defaultColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }
}
