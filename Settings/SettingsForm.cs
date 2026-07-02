using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;

namespace RhythmXT.Settings;

internal class SettingsForm
{
    internal enum FormState { Hide, Show }
    internal event Action CloseClicked;
    internal FormState State { get; private set; }

    FormBackground _formBackground;
    
    internal SettingsForm(int screenWidth, int screenHeight)
    {
        State = FormState.Hide;
        _formBackground = new(screenWidth, screenHeight);
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

    internal void LoadContent(ContentManager content)
    {
        _formBackground.LoadContent(content);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (State == FormState.Show) _formBackground.Draw(spriteBatch);
    }

    internal void Show() => State = FormState.Show;
    internal void Hide() => State = FormState.Hide;
}