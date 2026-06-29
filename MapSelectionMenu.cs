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

    MapSelectionButton _mapButton0;
    internal MapSelectionMenu(int screenWidth, int screenHeight)
    {
        state = State.Appearing;
        _generalColor = Color.Black;

        _mapButton0 = new(screenWidth, screenHeight);
    }

    internal void LoadContent(ContentManager content)
    {
        _mapButton0.LoadContent(content);
    }

    internal void Update(double deltaTime)
    {
        if (state == State.Appearing) AppearanceAnimation(deltaTime);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        _mapButton0.Draw(spriteBatch, _generalColor);
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
