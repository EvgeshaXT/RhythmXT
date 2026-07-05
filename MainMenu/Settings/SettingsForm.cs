using System;
using Microsoft.Xna.Framework;
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
    Label _languageLabel/*, _showOriginalNamesLabel*/;
    
    internal SettingsForm()
    {
        State = FormState.Hide;
        _background = new();
        _languageLabel = new()
        {
            Text = "Language: ",
            Position = new(GlobalScope.ScreenWidth / 3, GlobalScope.ScreenHeight / 3f)
        };
    }

    internal void LoadContent(ContentManager content)
    {
        _background.LoadContent(content);
        _languageLabel.LoadContent(content);
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
        }
    }

    internal void Show() => State = FormState.Show;
    internal void Hide() => State = FormState.Hide;
}