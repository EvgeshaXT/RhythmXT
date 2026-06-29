using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMainButton
{
    MapSelectionMainBackground mapSelectionMainBackground;

    Texture2D _texture;
    Vector2 _position;
    Vector2 _origin;
    internal MapSelectionMainButton(int screenWidth, int screenHeight, string songName)
    {
        _position = new(screenWidth, screenHeight / 2);

        string mapPath = $"Songs/{songName}";
        string[] mapFiles = Directory.GetFiles(mapPath);

        string mapMainXTFile = GetMainXTFile(mapFiles);
        string mapMainImageFile = GetMainImageFile(mapMainXTFile);

        mapSelectionMainBackground = new(screenWidth, screenHeight, $"{mapPath}/{mapMainImageFile}");
    }

    internal void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _texture = content.Load<Texture2D>("MapSelectionMenu/MapSelectionButton");
        _origin = new(_texture.Width, _texture.Height / 2);

        mapSelectionMainBackground.LoadContent(graphicsDevice);
    }

    internal void Update()
    {
        
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        mapSelectionMainBackground.Draw(spriteBatch, color);
        spriteBatch.Draw(_texture, _position, null, color, 0f, _origin, 0.9f, SpriteEffects.None, 0f);
    }

    static string GetMainXTFile(string[] mapFiles)
    {
        foreach (string mapFile in mapFiles)
        {
            string extension = Path.GetExtension(mapFile);

            if (extension == ".xt") return mapFile;
        }

        return "";
    }

    string GetMainImageFile(string mapMainXTFile)
    {
        string[] lines = File.ReadAllLines(mapMainXTFile);
        string[] parts = lines[0].Split(": ");

        return parts[1];
    }
}
