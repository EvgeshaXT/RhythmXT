using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;
using RhythmXT.GeneralObjects;

namespace RhythmXT.MainMenu.Settings;

class SettingsForm
{
    internal enum FormState { Hide, Show }
    internal event Action CloseClicked;
    internal FormState State { get; private set; }

    Background _background;
    Label _languageLabel, _showOriginalNamesLabel;
    OptionsLabel _languageOptions;
    
    internal SettingsForm(GraphicsDevice graphicsDevice)
    {
        State = FormState.Hide;
        _background = new(graphicsDevice);

        _languageLabel = new()
        {
            Font = "Yu Gothic, 22",
            Text = $"{RhythmXT.Settings.GetTranslation("Language")}: ",
            Position = new(GlobalScope.ScreenWidth / 2, GlobalScope.ScreenHeight / 2.15f),
            Mode = Label.OriginMode.Right
        };
        _languageOptions = new(["English", "Русский"], 0)
        {
            Font = "Yu Gothic, 22",
            Position = new(GlobalScope.ScreenWidth / 2 + 10, GlobalScope.ScreenHeight / 2.15f),
            Mode = OptionsLabel.OriginMode.Left
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
        _languageLabel.LoadContent(content);
        _showOriginalNamesLabel.LoadContent(content);
        _languageOptions.LoadContent(content);
    }

    internal void Update()
    {
        KeyboardInputManager.Handled = true;

        if (State == FormState.Show && KeyboardInputManager.EscapeRePressed)
        {
            State = FormState.Hide;
            CloseClicked?.Invoke();
        }

        if (KeyboardInputManager.EnterRePressed) _languageOptions.Update(1);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (State == FormState.Show) 
        {
            _background.Draw(spriteBatch);
            _languageLabel.Draw(spriteBatch);
            _showOriginalNamesLabel.Draw(spriteBatch);
            _languageOptions.Draw(spriteBatch);
        }
    }

    internal void Show() => State = FormState.Show;
    internal void Hide() => State = FormState.Hide;
}