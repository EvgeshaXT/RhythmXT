using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT.MainMenu.Settings;

class SettingsForm
{
    internal enum FormState { Hide, Show }
    internal event Action CloseClicked;
    internal FormState State { get; private set; }

    Background _background;
    Label _languageLabel, _showOriginalNamesLabel;
    
    internal SettingsForm()
    {
        State = FormState.Hide;
        _background = new();
        _languageLabel = new()
        {
            Font = "Yu Gothic, 22",
            Text = $"{RhythmXT.Settings.GetTranslation("Language")}: ",
            Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2.15f),
            Mode = Label.OriginMode.Right
        };
        _showOriginalNamesLabel = new()
        {
            Font = "Yu Gothic, 22",
            Text = $"{RhythmXT.Settings.GetTranslation("ShowOriginalNames")}: ",
            Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 1.85f),
            Mode = Label.OriginMode.Right
        };
    }

    internal void LoadContent(ContentManager content)
    {
        _background.LoadContent(content);
        _languageLabel.LoadContent(content);
        _showOriginalNamesLabel.LoadContent(content);
    }

    internal void Update()
    {
        KeyboardInputManager.Handled = true;

        if (State == FormState.Show && KeyboardInputManager.EscapeRePressed)
        {
            State = FormState.Hide;
            CloseClicked?.Invoke();
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (State == FormState.Show) 
        {
            _background.Draw(spriteBatch);
            _languageLabel.Draw(spriteBatch);
            _showOriginalNamesLabel.Draw(spriteBatch);
        }
    }

    internal void Show() => State = FormState.Show;
    internal void Hide() => State = FormState.Hide;
}