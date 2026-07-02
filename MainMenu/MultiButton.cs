namespace RhythmXT.MainMenu;

internal class MainMenuMultiButton : MainMenuButtonBase
{
    public MainMenuMultiButton(int screenWidth, int screenHeight)
    {
        CLICKED_POSITION = new(screenWidth / 2f + 25, screenHeight / 2f);
        HIDDEN_POSITION = new(screenWidth / 3f, screenHeight / 2f);
        Initialize();

        SPEED_CLICK_ANIMATION = 18f;
        _textureName = "MainMenu/Multi";
    }
}