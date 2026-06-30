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
            float stepFloat = 255f * 20f /*(animationSpeed)*/ * (float)deltaTime;
            int stepInt = (int)stepFloat;

            if (_generalColor.R + stepInt >= 255)
            {
                _generalColor.R = 255;
                _generalColor.G = 255;
                _generalColor.B = 255;
            }
            else
            {
                _generalColor.R += (byte)stepInt;
                _generalColor.G += (byte)stepInt;
                _generalColor.B += (byte)stepInt;
            }
        }

        else state = State.Visible;
    }
}
