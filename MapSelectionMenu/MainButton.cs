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
    Color _generalColor, _artistColor;
    Vector2 _origin;
    static internal float TextureHeight { get; set; }
    internal float PositionY
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    internal MapSelectionMainButton(int screenWidth, int screenHeight, SongMetadata songMetadata)
    {
        screenHeightHalf = screenHeight / 2;
        _position = new(screenWidth, screenHeightHalf);

        _songArtist = "Artist: ";
        if (songMetadata.ArtistOriginalName == "") _songArtistName = songMetadata.ArtistName;
        else _songArtistName = songMetadata.ArtistOriginalName;

        if (songMetadata.TitleOriginalName == "") _songTitleName = songMetadata.TitleName;
        else _songTitleName = songMetadata.TitleOriginalName;
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
        _positionTitleName.Y = PositionY + 18;

        _positionArtist.X = _positionTitleName.X;
        _positionArtist.Y = PositionY + 50;

        _positionArtistName.X = _positionArtist.X + 70;
        _positionArtistName.Y = _positionArtist.Y;

        _generalColor = color;
        _artistColor = color * 0.7f;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, _generalColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        if (_songTitleName != null) spriteBatch.DrawString(_fontTitle, _songTitleName, _positionTitleName, _generalColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_fontArtist, _songArtist, _positionArtist, _artistColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
        if (_songArtistName != null) spriteBatch.DrawString(_fontArtist, _songArtistName, _positionArtistName, _generalColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }
}
