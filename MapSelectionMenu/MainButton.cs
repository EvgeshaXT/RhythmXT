using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MapSelectionMenu;

class MapSelectionMainButton
{
    int screenHeightHalf;

    Label _titleName, artistLabel, _artistName;

    Texture2D _texture;
    Vector2 _position;
    Color _generalColor;
    Vector2 _origin;
    internal static float TextureHeight { get; set; }
    internal float PositionY
    {
        get => _position.Y;
        set => _position.Y = value;
    }
    internal MapSelectionMainButton(SongMetadata songMetadata)
    {
        screenHeightHalf = GlobalScope.ScreenHeight / 2;
        _position = new(GlobalScope.ScreenWidth, screenHeightHalf);

        _titleName = new()
        {
            Font = "Yu Gothic, 22",
            Mode = Label.OriginMode.Custom,
            Scale = 0.9f
        };

        artistLabel = new()
        {
            Text = $"{Settings.GetTranslation("Artist")}: ",
            Mode = Label.OriginMode.Custom,
            Scale = 0.9f
        };

        _artistName = new()
        {
            Mode = Label.OriginMode.Custom,
            Scale = 0.9f
        };

        if (songMetadata.ArtistOriginalName == "" || !Settings.ShowOriginalNames) _artistName.Text = songMetadata.ArtistName;
        else _artistName.Text = songMetadata.ArtistOriginalName;

        if (songMetadata.TitleOriginalName == "" || !Settings.ShowOriginalNames) _titleName.Text = songMetadata.TitleName;
        else _titleName.Text = songMetadata.TitleOriginalName;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        
        _titleName.LoadContent(content);
        artistLabel.LoadContent(content);
        _artistName.LoadContent(content);

        TextureHeight = _texture.Height;
        _origin = new(_texture.Width, TextureHeight / 2);

        _titleName.Origin = _origin;
        artistLabel.Origin = _origin;
        _artistName.Origin = _origin;
    }

    internal void Update(float height, Color color)
    {
        PositionY = screenHeightHalf + height;

        _titleName.Position.X = _position.X + 30;
        _titleName.Position.Y = PositionY + 18;

        artistLabel.Position.X = _titleName.Position.X;
        artistLabel.Position.Y = PositionY + 50;

        _artistName.Position.X = artistLabel.Position.X + artistLabel.Size.X;
        _artistName.Position.Y = artistLabel.Position.Y;

        _generalColor = color;

        _titleName.Color = color;
        artistLabel.Color = color * 0.7f;
        _artistName.Color = color;
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, _generalColor, 0f, _origin, 0.9f, SpriteEffects.None, 0f);

        _titleName.Draw(spriteBatch);
        artistLabel.Draw(spriteBatch);
        _artistName.Draw(spriteBatch);
    }
}
