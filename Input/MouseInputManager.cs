using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RhythmXT.Input;

static class MouseInputManager
{
    static MouseState _nowMouseState;
    static MouseState _lastMouseState;
    internal static bool Handled { get; set; }
    internal static Vector2 MousePosition => _nowMouseState.Position.ToVector2();
    internal static bool MouseLeftButtonRePressed => _lastMouseState.LeftButton == ButtonState.Released && _nowMouseState.LeftButton == ButtonState.Pressed;
    internal static void Update()
    {
        Handled = false;
        
        _lastMouseState = _nowMouseState;
        _nowMouseState = Mouse.GetState();
    }
}