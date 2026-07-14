using System;
using static System.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT.MainMenu.Settings;

class Background
{
    Texture2D _texture;
    Rectangle _rectangle;

    internal Background(GraphicsDevice graphicsDevice)
    {
        int width = 640;
        int height = 192;

        _texture = GetTexture(graphicsDevice, width, height, 32f, new Color(12, 12, 14));
        _rectangle = new Rectangle((GlobalScope.ScreenWidth - width) / 2, (GlobalScope.ScreenHeight - height) / 2, width, height);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangle, Color.White);
    }

    Texture2D GetTexture(GraphicsDevice graphicsDevice, int width, int height, float radius, Color color)
    {
        Texture2D texture = new(graphicsDevice, width, height);
        Color[] colors = new Color[width * height];

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        float sizeX = halfWidth - radius;
        float sizeY = halfHeight - radius;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float displacedX = x - halfWidth;
                float displacedY = y - halfHeight;

                // Working only in one part of rectangle
                float absX = Abs(displacedX);
                float absY = Abs(displacedY);

                // vector to Up Right Corner
                float dx = absX - sizeX;
                float dy = absY - sizeY;

                double outsideDist = Sqrt(Max(dx, 0)*Max(dx, 0) + Max(dy, 0)*Max(dy, 0));
                double insideDist = Min(Max(dx, dy), 0);
                float dist = (float)(outsideDist + insideDist - radius);

                // Smoothing
                float a = 1f - MathHelper.Clamp(dist + 0.5f, 0f, 1f);
                colors[y * width + x] = color  * a;
            }
        }

        texture.SetData(colors);
        return texture;
    }
}