using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmXT;

internal class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;
    MainMenuMultiButton _mainMenuMultiButton;
    MainMenuExitButton _mainMenuExitButton;
    internal bool StopUpdateAndDraw { get; private set; }
    internal bool SoloClicked { get; private set; }
    bool ExitClicked { get; set; }
    internal bool ExitAllowed { get; private set; }

    GraphicsDevice graphicsDevice;

    RenderTarget2D _buttonRenderTarget;
    Texture2D _maskCircleTexture;
    BlendState blendState;
    Color _generalColor;

    internal MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        this.graphicsDevice = graphicsDevice;
        StopUpdateAndDraw = false;

        _mainMenuBackground = new(graphicsDevice, screenWidth, screenHeight);
        
        SoloClicked = false;
        _mainMenuSoloButton = new(screenWidth, screenHeight);
        _mainMenuSoloButton.ClickedEvent += () => SoloClicked = true;

        _mainMenuMultiButton = new(screenWidth, screenHeight);

        ExitClicked = false;
        _mainMenuExitButton = new(screenWidth, screenHeight);
        _mainMenuExitButton.ClickedEvent += () => ExitClicked = true;

        _mainMenuXTCircle = new(screenWidth, screenHeight);

        _buttonRenderTarget = new(graphicsDevice, screenWidth, screenHeight);
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
        _mainMenuBackground.LoadContent();
        _mainMenuXTCircle.LoadContent(content);
        _mainMenuSoloButton.LoadContent(content);
        _mainMenuMultiButton.LoadContent(content);
        _mainMenuExitButton.LoadContent(content);

        CreateMaskCircleTexture();
    }

    internal void Update(double deltaTime)
    {
        if (!StopUpdateAndDraw)
        {
            if (SoloClicked || ExitClicked) ColorToBlackout(deltaTime);
            else
            {
                _mainMenuXTCircle.Update(deltaTime, AnyButtonHaveCursor());
                MainMenuButtonsUpdate(deltaTime);
            }
        }
    }

    void MainMenuButtonsUpdate(double deltaTime)
    {
        if (_mainMenuXTCircle.IsClicked)
        {
            if (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuSoloButton.Show();
            if (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuMultiButton.Show();
            if (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Appearing) _mainMenuExitButton.Show();

            _mainMenuSoloButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuMultiButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
            _mainMenuExitButton.Update(deltaTime, _mainMenuXTCircle.ContainsCursor());
        }
        
        else if (_mainMenuXTCircle.IsUnClicked)
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
        if (!StopUpdateAndDraw)
        {
            RenderMaskedButtons(spriteBatch);

            spriteBatch.Begin();

            _mainMenuBackground.Draw(spriteBatch, _generalColor);
            spriteBatch.Draw(_buttonRenderTarget, Vector2.Zero, _generalColor);
            _mainMenuXTCircle.Draw(spriteBatch, _generalColor);

            spriteBatch.End();
        }
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

    internal void ColorToBlackout(double deltaTime)
    {
        if (_generalColor.R != 0)
        {
            float animationSpeed = 0f;
            
            if (SoloClicked) animationSpeed = 14f;
            else if (ExitClicked) animationSpeed = 6f;

            float lerpFactor = 1 - (float)Math.Exp(-animationSpeed * deltaTime);
            byte step = (byte)(255f * lerpFactor);

            _generalColor.R -= step;
            _generalColor.G -= step;
            _generalColor.B -= step;
        }

        else
        {
            if (SoloClicked) StopUpdateAndDraw = true;
            else if (ExitClicked)
            {
                Thread.Sleep(300);
                ExitAllowed = true;
            }
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
        return (_mainMenuSoloButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuSoloButton.ContainsCursor()) ||
               (_mainMenuMultiButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuMultiButton.ContainsCursor()) ||
               (_mainMenuExitButton.State != MainMenuButtonBase.ButtonState.Hidden && _mainMenuExitButton.ContainsCursor());
    }
}