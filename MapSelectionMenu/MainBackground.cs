using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MapSelectionMenu;

internal class MapSelectionMainBackground
{
    string _imagePath;
    Texture2D _texture;

    internal MapSelectionMainBackground(string imagePath)
    {
        _imagePath = imagePath;
    }

    internal void LoadContent(GraphicsDevice graphicsDevice)
    {
        using Stream stream = File.OpenRead(_imagePath);
        _texture = Texture2D.FromStream(graphicsDevice, stream);
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, new Rectangle(0, 0, GlobalScope.ScreenWidth, GlobalScope.ScreenHeight), color * 0.7f);
    }
}
