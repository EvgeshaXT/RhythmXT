using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MapSelectionMenu
{
    internal enum MenuState { Hidden, Appearing, Visible, Disappearing }
    internal MenuState State { get; private set; }
    internal event Action ToMainMenuEvent;

    Color _generalColor;
    Random random;

    MapSelectionMainBackground _mapSelectionMainBackground;
    List<MapSelectionMainButton> _mapSelectionMainButtonList;
    internal MapSelectionMenu(int screenWidth, int screenHeight)
    {
        State = MenuState.Hidden;
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
        if (KeyboardInputManager.EscapeRePressed) State = MenuState.Disappearing;

        if (State == MenuState.Appearing) AppearanceAnimation(deltaTime);
        else if (State == MenuState.Disappearing) DisappearanceAnimation(deltaTime);

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

    internal void Show()
    {
        if (State == MenuState.Disappearing || State == MenuState.Hidden) State = MenuState.Appearing;
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

        else State = MenuState.Visible;
    }

    void DisappearanceAnimation(double deltaTime)
    {
        if (_generalColor.R != 0)
        {
            float stepFloat = 255f * 18f /*(animationSpeed)*/ * (float)deltaTime;
            int stepInt = (int)stepFloat;

            if (stepInt >= _generalColor.R)
            {
                _generalColor.R = 0;
                _generalColor.G = 0;
                _generalColor.B = 0;
            }
            else
            {
                _generalColor.R -= (byte)stepInt;
                _generalColor.G -= (byte)stepInt;
                _generalColor.B -= (byte)stepInt;
            }
        }

        else
        {
            State = MenuState.Hidden;
            ToMainMenuEvent?.Invoke();
        }
    }
}
