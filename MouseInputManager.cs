using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

internal static class MouseInputManager
{
    internal static MouseState _mouseState;
    internal static Vector2 MousePosition => _mouseState.Position.ToVector2();
    internal static bool MouseLeftClickPressed => _mouseState.LeftButton == ButtonState.Pressed;
    internal static void Update()
    {
        _mouseState = Mouse.GetState();
    }
}