using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMenu
{
    enum State { Appearing, Visible, Disappearing }
    State state { get; set; }

    Color _generalColor;
    Random random;

    MapSelectionMainBackground _mapSelectionMainBackground;
    List<MapSelectionMainButton> _mapSelectionMainButtonList;
    internal MapSelectionMenu(int screenWidth, int screenHeight)
    {
        state = State.Appearing;
        _generalColor = Color.Black;

        _mapSelectionMainButtonList = [];

        string[] songsFolders = GetSongsFolders();
        foreach (string songFolder in songsFolders)
        {
            string songName = Path.GetFileName(songFolder);

            MapSelectionMainButton mapSelectionMainButton = new(screenWidth, screenHeight, songName);
            _mapSelectionMainButtonList.Add(mapSelectionMainButton);
        }

        random = new();
        string mapDefault = songsFolders[random.Next(songsFolders.Length)];
        _mapSelectionMainBackground = new(screenWidth, screenHeight, mapDefault);
    }

    internal void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _mapSelectionMainBackground.LoadContent(graphicsDevice);

        foreach (MapSelectionMainButton mapSelectionMainButton in _mapSelectionMainButtonList)
        {
            mapSelectionMainButton.LoadContent(content);
        }
    }

    internal void Update(double deltaTime)
    {
        if (state == State.Appearing) AppearanceAnimation(deltaTime);

        float height = 0;

        foreach (MapSelectionMainButton mapSelectionMainButton in _mapSelectionMainButtonList)
        {
            mapSelectionMainButton.Update(height);
            height += MapSelectionMainButton.TextureHeight;
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        _mapSelectionMainBackground.Draw(spriteBatch, _generalColor);
        
        foreach (MapSelectionMainButton mapSelectionMainButton in _mapSelectionMainButtonList)
        {
            mapSelectionMainButton.Draw(spriteBatch, _generalColor);
        }
    }

    string[] GetSongsFolders()
    {
        string songsPath = "Songs";
        string[] songsFolders = Directory.GetDirectories(songsPath);

        return songsFolders;
    }
    
    void AppearanceAnimation(double deltaTime)
    {
        if (_generalColor.R != 255)
        {
            float stepFloat = 255f * 18f /*(animationSpeed)*/ * (float)deltaTime;
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
