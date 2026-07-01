using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MapSelectionMenu;

internal class MapSelectionMainBackground
{
    int screenWidth, screenHeight;
    string currentSongPath, _imagePath;
    Texture2D _texture;
    internal MapSelectionMainBackground(int screenWidth, int screenHeight, string currentSongPath)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.currentSongPath = currentSongPath;
    }

    internal void LoadContent(GraphicsDevice graphicsDevice)
    {
        string[] songFiles = Directory.GetFiles(currentSongPath);

        string songMainXTFile = GetMainXTFile(songFiles);
        string songMainImageFile = GetMainImageFile(songMainXTFile);

        _imagePath = $"{currentSongPath}/{songMainImageFile}";

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
        
        foreach (string line in lines)
        {
            if (line.StartsWith("Image: "))
            {
                string[] parts = line.Split(": ");
                return parts[1];
            }
        }

        return "";
    }
}
