namespace RhythmXT.MainMenu;

internal class MainMenuExitButton : MainMenuButtonBase
{
    public MainMenuExitButton(int screenWidth, int screenHeight)
    {
        CLICKED_POSITION = new(screenWidth / 2f, screenHeight / 1.44444f);
        HIDDEN_POSITION = new(screenWidth / 2.75f, screenHeight / 1.44444f);
        Initialize();
        
        SPEED_CLICK_ANIMATION = 16f;
        _textureName = "MainMenu/Exit";
    }
}