using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmXT.Input;
using RhythmXT.MainMenu.Settings;

namespace RhythmXT.MainMenu;

internal class MainMenuManager
{
    internal enum MenuState { Hidden, Appearing, Visible, Disappearing }
    internal MenuState State { get; private set; }
    internal event Action ClickedEvent;
    internal event Action ClickAnimationIsFinishedEvent;

    SettingsForm settingsForm;
    MainMenuBackground _mainMenuBackground;
    MainMenuSettingsButton _mainMenuSettingsButton;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;
    MainMenuMultiButton _mainMenuMultiButton;
    MainMenuExitButton _mainMenuExitButton;
    internal bool SoloClicked { get; private set; }
    bool ExitClicked { get; set; }
    internal bool ExitAllowed { get; private set; }

    GraphicsDevice graphicsDevice;

    RenderTarget2D _buttonRenderTarget;
    Texture2D _maskCircleTexture;
    BlendState blendState;
    Color _generalColor;

    internal MainMenuManager(GraphicsDevice graphicsDevice)
    {
        State = MenuState.Visible;
        this.graphicsDevice = graphicsDevice;

        _mainMenuBackground = new(graphicsDevice);

        settingsForm = new();

        _mainMenuSettingsButton = new();
        _mainMenuSettingsButton.ClickedEvent += () =>
        {
            settingsForm.Show();
            ClickedEvent?.Invoke();
        };
        
        SoloClicked = false;
        _mainMenuSoloButton = new();
        _mainMenuSoloButton.ClickedEvent += () =>
        {
            SoloClicked = true;
            ClickedEvent?.Invoke();
        };

        _mainMenuMultiButton = new();
        /*_mainMenuMultiButton.ClickedEvent += () =>
        {
            ClickedEvent?.Invoke();
        };*/

        ExitClicked = false;
        _mainMenuExitButton = new();
        _mainMenuExitButton.ClickedEvent += () =>
        {
            ExitClicked = true;
            ClickedEvent?.Invoke();
        };

        _mainMenuXTCircle = new();
        _mainMenuXTCircle.ClickedEvent += () =>
        {
            ClickedEvent?.Invoke();
        };

        _buttonRenderTarget = new(graphicsDevice, GlobalScope.ScreenWidth, GlobalScope.ScreenHeight);
        blendState = new()
        {
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceAlpha,
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceAlpha
        };

        _generalColor = Color.White;
    }

    internal void LoadContent(ContentManager content)
    {
        settingsForm.LoadContent(content);

        _mainMenuBackground.LoadContent();
        _mainMenuSettingsButton.LoadContent(content);
        _mainMenuXTCircle.LoadContent(content);
        _mainMenuSoloButton.LoadContent(content);
        _mainMenuMultiButton.LoadContent(content);
        _mainMenuExitButton.LoadContent(content);

        CreateMaskCircleTexture();
    }

    internal void Update(double gameDeltaTime)
    {
        if (settingsForm.State == SettingsForm.FormState.Show) settingsForm.Update();
        else
        {
            if (KeyboardInputManager.EscapeRePressed && !KeyboardInputManager.Handled) ExitClicked = true;
            if (KeyboardInputManager.EnterRePressed && !KeyboardInputManager.Handled)
            {
                if (_mainMenuXTCircle.StatePosition == MainMenuXTCircle.CirclePositionState.Centre) _mainMenuXTCircle.Clicked();
                else _mainMenuSoloButton.Clicked();
            }

            if (SoloClicked || ExitClicked) State = MenuState.Disappearing;
            if (State == MenuState.Appearing) AppearanceAnimation(gameDeltaTime);
            else if (State == MenuState.Visible)
            {
                _mainMenuSettingsButton.Update();
                _mainMenuXTCircle.Update(gameDeltaTime, AnyButtonHaveCursor());
                MainMenuButtonsUpdate(gameDeltaTime);
            }
            else DisappearanceAnimation(gameDeltaTime);
        }
    }

    void MainMenuButtonsUpdate(double deltaTime)
    {
        if (_mainMenuXTCircle.StatePosition == MainMenuXTCircle.CirclePositionState.ToLeft)
        {
            if (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuSoloButton.Show();
            if (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuMultiButton.Show();
            if (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuExitButton.Show();

            _mainMenuSoloButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuMultiButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuExitButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
        }
        
        else if (_mainMenuXTCircle.StatePosition == MainMenuXTCircle.CirclePositionState.ToCentre)
        {
            if (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Disappearing) _mainMenuSoloButton.Hide();
            if (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Disappearing) _mainMenuMultiButton.Hide();
            if (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Disappearing) _mainMenuExitButton.Hide();

            _mainMenuSoloButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuMultiButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuExitButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
        }
    }

    // =============== DRAW =============== //
    // ! spriteBatch Begin() / End() self
    internal void Draw(SpriteBatch spriteBatch)
    {
        RenderMaskedButtons(spriteBatch);

        spriteBatch.Begin();

        _mainMenuBackground.Draw(spriteBatch, _generalColor);
        _mainMenuSettingsButton.Draw(spriteBatch, _generalColor);
        spriteBatch.Draw(_buttonRenderTarget, Vector2.Zero, _generalColor);
        _mainMenuXTCircle.Draw(spriteBatch, _generalColor);
        settingsForm.Draw(spriteBatch);

        spriteBatch.End();
    }

    internal void Show()
    {
        SoloClicked = false;

        _mainMenuSoloButton.HideForced();
        _mainMenuMultiButton.HideForced();
        _mainMenuExitButton.HideForced();

        _mainMenuXTCircle.ResetToCentre();

        State = MenuState.Appearing;
    }

    void RenderMaskedButtons(SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(_buttonRenderTarget);
        graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin();
        MainMenuButtonsDraw(spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin(blendState: blendState);
        spriteBatch.Draw(_maskCircleTexture, _mainMenuXTCircle.Position, null, _generalColor, 0f, _mainMenuXTCircle.Origin, _mainMenuXTCircle.Scale, SpriteEffects.None, 0f);
        spriteBatch.End();

        graphicsDevice.SetRenderTarget(null);
    }

    void MainMenuButtonsDraw(SpriteBatch spriteBatch)
    {
        if (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Hidden) _mainMenuSoloButton.Draw(spriteBatch);
        if (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Hidden) _mainMenuMultiButton.Draw(spriteBatch);
        if (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Hidden) _mainMenuExitButton.Draw(spriteBatch);
    }
    
    void AppearanceAnimation(double gameDeltaTime)
    {
        if (_generalColor.R != 255)
        {
            int stepInt = (int)(255d * 8d /*(animationSpeed)*/ * gameDeltaTime);

            if (_generalColor.R + stepInt >= 255) _generalColor = new Color(255, 255, 255, 255);
            else
            {
                int newValue = _generalColor.R + stepInt;

                _generalColor = new Color(newValue, newValue, newValue, 255);
            }
        }

        else State = MenuState.Visible;
    }

    internal void DisappearanceAnimation(double gameDeltaTime)
    {
        if (_generalColor.R != 0)
        {
            double animationSpeed = 0d;
            
            if (SoloClicked) animationSpeed = 8d;
            else if (ExitClicked) animationSpeed = 4d;

            int stepInt = (int)(255d * animationSpeed * gameDeltaTime);

            if (stepInt >= _generalColor.R) _generalColor = new Color(0, 0, 0, 255);
            else
            {
                int newValue = _generalColor.R - stepInt;

                _generalColor = new Color(newValue, newValue, newValue, 255);
            }
        }

        else
        {
            if (SoloClicked)
            {
                State = MenuState.Hidden;
                ClickAnimationIsFinishedEvent?.Invoke();
            }
            else if (ExitClicked) ExitAllowed = true;
        }
    }

    void CreateMaskCircleTexture()
    {
        int width = _mainMenuXTCircle.TextureWidth;

        _maskCircleTexture = new(graphicsDevice, width, width);
        Color[] colorData = new Color[width * width];

        float radius = width / 2f;
        float radiusSquared = radius * radius;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = x + y * width;

                float dx = x - radius;
                float dy = y - radius;

                if ((dx * dx + dy * dy) / radiusSquared < 1f) colorData[index] = Color.White;
                else colorData[index] = Color.Transparent;
            }
        }

        _maskCircleTexture.SetData(colorData);
    }

    bool AnyButtonHaveCursor()
    {
        return (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuSoloButton.IsContainsCursor) ||
               (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuMultiButton.IsContainsCursor) ||
               (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuExitButton.IsContainsCursor);
    }
}