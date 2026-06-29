using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMenu
{
    enum State { Appearing, Visible, Disappearing }
    State state { get; set; }

    Color _generalColor;

    MapSelectionMainButton _mapMainButton0;
    internal MapSelectionMenu(int screenWidth, int screenHeight)
    {
        state = State.Appearing;
        _generalColor = Color.Black;

        _mapMainButton0 = new(screenWidth, screenHeight, "Camellia - GHOST (2020 Halloween+++++++++ VIP)");
    }

    internal void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _mapMainButton0.LoadContent(content, graphicsDevice);
    }

    internal void Update(double deltaTime)
    {
        if (state == State.Appearing) AppearanceAnimation(deltaTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        _mapMainButton0.Draw(spriteBatch, _generalColor);
    }
    
    void AppearanceAnimation(double deltaTime)
    {
        if (_generalColor.R != 255)
        {
            float lerpFactor = 1 - (float)Math.Exp(-16f * deltaTime);
            byte step = (byte)(255f * lerpFactor);

            _generalColor.R += step;
            _generalColor.G += step;
            _generalColor.B += step;
        }

        else state = State.Visible;
    }
}
