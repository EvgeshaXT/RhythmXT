using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
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
    SpriteFont fontYuGothic18, fontYuGothic22;

    MapSelectionMainBackground _mapSelectionMainBackground;
    List<SongMetadata> _songMetadataList;
    List<MapSelectionMainButton> _mapSelectionMainButtonList;
    internal MapSelectionMenuManager()
    {
        State = MenuState.Hidden;
        _generalColor = Color.Black;

        _songMetadataList = [];
        _mapSelectionMainButtonList = [];

        string[] songsFolders = GetSongsFolders();
        foreach (string songFolder in songsFolders)
        {
            SongMetadata songMetadata = SongMetadata.ParseXTFile(songFolder);
            _songMetadataList.Add(songMetadata);

            MapSelectionMainButton mapSelectionMainButton = new(songMetadata);
            _mapSelectionMainButtonList.Add(mapSelectionMainButton);
        }

        random = new();
        _mapSelectionMainBackground = new(_songMetadataList[random.Next(_songMetadataList.Count)].ImagePath);
    }

    internal void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _mapSelectionMainBackground.LoadContent(graphicsDevice);

        fontYuGothic18 = content.Load<SpriteFont>("Yu Gothic 18");
        fontYuGothic22 = content.Load<SpriteFont>("Yu Gothic 22");
        foreach (MapSelectionMainButton mapSelectionMainButton in _mapSelectionMainButtonList)
        {
            mapSelectionMainButton.LoadContent(content, fontYuGothic18, fontYuGothic22);
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
            mapSelectionMainButton.Update(height, _generalColor);
            height += MapSelectionMainButton.TextureHeight;
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        _mapSelectionMainBackground.Draw(spriteBatch, _generalColor);

        foreach (MapSelectionMainButton mapSelectionMainButton in _mapSelectionMainButtonList)
        {
            mapSelectionMainButton.Draw(spriteBatch);
        }
    }

    internal void Show()
    {
        if (State == MenuState.Disappearing || State == MenuState.Hidden) State = MenuState.Appearing;
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

    static string[] GetSongsFolders()
    {
        string songsPath = "Songs";
        string[] songsFolders = Directory.GetDirectories(songsPath);

        return songsFolders;
    }

    static string GetArtistName(string songName)
    {
        string songXTPath = $"Songs/{songName}/main.xt";
        string[] lines = File.ReadAllLines(songXTPath);
        
        foreach (string line in lines)
        {
            if (line.StartsWith("Artist: "))
            {
                string[] parts = line.Split(": ");
                return parts[1];
            }
        }

        return "";
    }

    static string GetTitleName(string songName)
    {
        string songXTPath = $"Songs/{songName}/main.xt";
        string[] lines = File.ReadAllLines(songXTPath);
        
        foreach (string line in lines)
        {
            if (line.StartsWith("Title: "))
            {
                string[] parts = line.Split(": ");
                return parts[1];
            }
        }

        return "";
    }
}
