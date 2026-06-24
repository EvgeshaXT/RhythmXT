using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public static class MouseInputManager
{
    static MouseState _lastMouseState;
    static MouseState _nowMouseState;
    public static Vector2 MousePosition => _nowMouseState.Position.ToVector2();
    public static bool MouseLeftClickPressed => _nowMouseState.LeftButton == ButtonState.Pressed;
    public static bool MouseLeftClickPrReleased => _lastMouseState.LeftButton == ButtonState.Pressed && _nowMouseState.LeftButton == ButtonState.Released;
    public static void Update()
    {
        _lastMouseState = _nowMouseState;
        _nowMouseState = Mouse.GetState();
    }
}