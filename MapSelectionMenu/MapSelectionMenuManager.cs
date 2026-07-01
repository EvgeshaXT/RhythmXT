using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT.MapSelectionMenu;

internal class MapSelectionMenuManager
{
    internal enum MenuState { Hidden, Appearing, Visible, Disappearing }
    internal MenuState State { get; private set; }
    internal event Action ToMainMenuEvent;

    Color _generalColor;
    Random random;

    MapSelectionMainBackground _mapSelectionMainBackground;
    List<MapSelectionMainButton> _mapSelectionMainButtonList;
    internal MapSelectionMenuManager(int screenWidth, int screenHeight)
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

    internal void Update(double gameDeltaTime)
    {
        if (KeyboardInputManager.EscapeRePressed) State = MenuState.Disappearing;

        if (State == MenuState.Appearing) AppearanceAnimation(gameDeltaTime);
        else if (State == MenuState.Disappearing) DisappearanceAnimation(gameDeltaTime);

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
    
    void AppearanceAnimation(double gameDeltaTime)
    {
        if (_generalColor.R != 255)
        {
            int stepInt = (int)(255d * 8d /*(animationSpeed)*/ * gameDeltaTime);

            if (_generalColor.R + stepInt >= 255) _generalColor = new(255, 255, 255, 255);
            else
            {
                int newValue = _generalColor.R + stepInt;

                _generalColor = new Color(newValue, newValue, newValue, 255);
            }
        }

        else State = MenuState.Visible;
    }

    void DisappearanceAnimation(double gameDeltaTime)
    {
        if (_generalColor.R != 0)
        {
            int stepInt = (int)(255d * 8d /*(animationSpeed)*/ * gameDeltaTime);

            if (stepInt >= _generalColor.R) _generalColor = new Color(0, 0, 0, 255);
            else
            {
                int newValue = _generalColor.R - stepInt;

                _generalColor = new Color(newValue, newValue, newValue, 255);
            }
        }

        else
        {
            State = MenuState.Hidden;
            ToMainMenuEvent?.Invoke();
        }
    }
}
