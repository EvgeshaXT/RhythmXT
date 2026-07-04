namespace RhythmXT;

internal static class GlobalScope
{
    internal static int ScreenWidth { get; private set; }
    internal static int ScreenHeight { get; private set; }

    internal static void Initialize(int screenWidth, int screenHeight)
    {
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
    }
}