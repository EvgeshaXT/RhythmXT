namespace RhytmXT;

internal class MainMenuMultiButton : MainMenuButtonBase
{
    public MainMenuMultiButton(int screenWidth, int screenHeight)
    {
        CLICKED_POSITION = new(screenWidth / 2f + 25, screenHeight / 2f);
        HIDDEN_POSITION = new(screenWidth / 3f, screenHeight / 2f);

        _textureName = "MainMenu/Multi";

        Initialize();
    }
}