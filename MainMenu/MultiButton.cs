namespace RhythmXT.MainMenu;

class MainMenuMultiButton : MainMenuButtonBase
{
    internal MainMenuMultiButton()
    {
        CLICKED_POSITION = new(GlobalScope.ScreenWidth / 2f + 25, GlobalScope.ScreenHeight / 2f);
        HIDDEN_POSITION = new(GlobalScope.ScreenWidth / 3f, GlobalScope.ScreenHeight / 2f);
        Initialize();

        SPEED_CLICK_ANIMATION = 18f;
        _textureName = "MainMenu/Multi";
    }
}