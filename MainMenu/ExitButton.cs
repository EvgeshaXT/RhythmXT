namespace RhythmXT.MainMenu;

internal class MainMenuExitButton : MainMenuButtonBase
{
    public MainMenuExitButton()
    {
        CLICKED_POSITION = new(GlobalScope.ScreenWidth / 2f, GlobalScope.ScreenHeight / 1.44444f);
        HIDDEN_POSITION = new(GlobalScope.ScreenWidth / 2.75f, GlobalScope.ScreenHeight / 1.44444f);
        Initialize();
        
        SPEED_CLICK_ANIMATION = 16f;
        _textureName = "MainMenu/Exit";
    }
}