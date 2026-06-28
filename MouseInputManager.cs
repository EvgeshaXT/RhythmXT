using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

internal static class MouseInputManager
{
    internal static MouseState _nowMouseState;
    internal static MouseState _lastMouseState;
    internal static Vector2 MousePosition => _nowMouseState.Position.ToVector2();
    internal static bool MouseLeftButtonRePressed => _lastMouseState.LeftButton == ButtonState.Released && _nowMouseState.LeftButton == ButtonState.Pressed;
    internal static void Update()
    {
        _lastMouseState = _nowMouseState;
        _nowMouseState = Mouse.GetState();
    }
}