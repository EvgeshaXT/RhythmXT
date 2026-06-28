using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MainMenuBackground
{
    GraphicsDevice graphicsDevice;
    Texture2D _texture;
    Vector2 _position, _origin;
    Random _random;

    public MainMenuBackground(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        this.graphicsDevice = graphicsDevice;
        
        _position = new(screenWidth / 2, screenHeight / 2);
        _random = new();
    }
    
    internal void LoadContent()
    {
        _texture = LoadTexture();
        _origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    internal void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _position, null, color * 0.5f, 0, _origin, 1f, SpriteEffects.None, 0f);
    }

    Texture2D LoadTexture()
    {
        string[] files = Directory.GetFiles("Content/bg/");
        string file = files[_random.Next(files.Length)];

        using Stream stream = File.OpenRead(file);
        return Texture2D.FromStream(graphicsDevice, stream);
    }
}