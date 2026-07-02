namespace RhythmXT.MainMenu;

internal class MainMenuSoloButton : MainMenuButtonBase
{
    public MainMenuSoloButton(int screenWidth, int screenHeight)
    {
        CLICKED_POSITION = new(screenWidth / 2f, screenHeight / 3.25f);
        HIDDEN_POSITION = new(screenWidth / 2.75f, screenHeight / 3.25f);
        Initialize();
        
        SPEED_CLICK_ANIMATION = 16f;
        _textureName = "MainMenu/Solo";
    }
}