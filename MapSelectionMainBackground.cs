using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMainBackground
{
    int screenWidth, screenHeight;
    string currentSongName, _imagePath;
    Texture2D _texture;
    internal MapSelectionMainBackground(int screenWidth, int screenHeight, string currentSongName)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.currentSongName = currentSongName;
    }

    internal void LoadContent(GraphicsDevice graphicsDevice)
    {
        string songPath = $"Songs/{currentSongName}";
        string[] songFiles = Directory.GetFiles(songPath);

        string songMainXTFile = GetMainXTFile(songFiles);
        string songMainImageFile = GetMainImageFile(songMainXTFile);

        _imagePath = $"{songPath}/{songMainImageFile}";

        using Stream stream = File.OpenRead(_imagePath);
        _texture = Texture2D.FromStream(graphicsDevice, stream);
    }

    internal void Update()
    {
        
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, new Rectangle(0, 0, screenWidth, screenHeight), color * 0.7f);
    }

    static string GetMainXTFile(string[] songFiles)
    {
        foreach (string songFile in songFiles)
        {
            string extension = Path.GetExtension(songFile);

            if (extension == ".xt") return songFile;
        }

        return "";
    }

    string GetMainImageFile(string songMainXTFile)
    {
        string[] lines = File.ReadAllLines(songMainXTFile);
        string[] parts = lines[0].Split(": ");

        return parts[1];
    }
}
