using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

internal static class KeyboardInputManager
{
    internal static KeyboardState _nowKeyboardState;
    internal static KeyboardState _lastKeyboardState;
    internal static bool EscapePressed => _nowKeyboardState.IsKeyDown(Keys.Escape);
    internal static bool EnterPressed => _lastKeyboardState.IsKeyDown(Keys.Enter) && _nowKeyboardState.IsKeyUp(Keys.Enter);
    internal static void Update()
    {
        _lastKeyboardState = _nowKeyboardState;
        _nowKeyboardState = Keyboard.GetState();
    }
}