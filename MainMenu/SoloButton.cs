namespace RhythmXT.MainMenu;

internal class MainMenuSoloButton : MainMenuButtonBase
{
    public MainMenuSoloButton()
    {
        CLICKED_POSITION = new(GlobalScope.ScreenWidth / 2f, GlobalScope.ScreenHeight / 3.25f);
        HIDDEN_POSITION = new(GlobalScope.ScreenWidth / 2.75f, GlobalScope.ScreenHeight / 3.25f);
        Initialize();
        
        SPEED_CLICK_ANIMATION = 16f;
        _textureName = "MainMenu/Solo";
    }
}