using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT.Core;

public static class MouseInputManager
{
    private static MouseState _mouseState;
    public static Vector2 MousePosition => _mouseState.Position.ToVector2();
    public static void Update()
    {
        _mouseState = Mouse.GetState();
    }
}