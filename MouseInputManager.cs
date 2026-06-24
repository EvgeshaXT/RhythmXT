using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public static class MouseInputManager
{
    private static MouseState _mouseState;
    public static Vector2 MousePosition => _mouseState.Position.ToVector2();
    public static bool MouseLeftClickPressed => _mouseState.LeftButton == ButtonState.Pressed;
    public static void Update()
    {
        _mouseState = Mouse.GetState();
    }
}