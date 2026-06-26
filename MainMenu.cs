using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

internal class MainMenu
{
    MainMenuBackground _mainMenuBackground;
    MainMenuXTCircle _mainMenuXTCircle;
    MainMenuSoloButton _mainMenuSoloButton;
    MainMenuMultiButton _mainMenuMultiButton;

    Stopwatch stopwatch;
    double deltaTime, lastUpdateTime;
    GraphicsDevice graphicsDevice;

    RenderTarget2D _buttonRenderTarget;
    Texture2D _maskCircleTexture;
    BlendState blendState;

    public MainMenu(GraphicsDevice graphicsDevice, int screenWidth, int screenHeight)
    {
        stopwatch = Stopwatch.StartNew();
        this.graphicsDevice = graphicsDevice;

        _mainMenuBackground = new(graphicsDevice, screenWidth, screenHeight);
        
        _mainMenuSoloButton = new(screenWidth, screenHeight);
        _mainMenuMultiButton = new(screenWidth, screenHeight);

        IContainsCursor[] clickables = [_mainMenuSoloButton, _mainMenuMultiButton];
        _mainMenuXTCircle = new(screenWidth, screenHeight, clickables);

        _buttonRenderTarget = new(graphicsDevice, screenWidth, screenHeight);
        blendState = new()
        {
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceAlpha,
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceAlpha
        };
    }

    internal void LoadContent(ContentManager content)
    {
        _mainMenuBackground.LoadContent();
        _mainMenuXTCircle.LoadContent(content);
        _mainMenuSoloButton.LoadContent(content);
        _mainMenuMultiButton.LoadContent(content);

        CreateMaskCircleTexture();
    }

    internal void Update()
    {
        UpdateDeltaTime();

        _mainMenuXTCircle.Update(deltaTime);
        MainMenuButtonsUpdate();
    }

    void MainMenuButtonsUpdate()
    {
        if (_mainMenuXTCircle.IsClicked)
        {
            if (!_mainMenuSoloButton.IsAppearing)
            {
                _mainMenuSoloButton.IsAppearing = true;
                _mainMenuSoloButton.IsDisappearing = false;
            }

            if (!_mainMenuMultiButton.IsAppearing)
            {
                _mainMenuMultiButton.IsAppearing = true;
                _mainMenuMultiButton.IsDisappearing = false;
            }

            _mainMenuSoloButton.Update(deltaTime);
            _mainMenuMultiButton.Update(deltaTime);
        }
        else if (_mainMenuXTCircle.IsUnClicked)
        {
            if (!_mainMenuSoloButton.IsDisappearing)
            {
                _mainMenuSoloButton.IsAppearing = false;
                _mainMenuSoloButton.IsDisappearing = true;
            }

            if (!_mainMenuMultiButton.IsDisappearing)
            {
                _mainMenuMultiButton.IsAppearing = false;
                _mainMenuMultiButton.IsDisappearing = true;
            }

            _mainMenuSoloButton.Update(deltaTime);
            _mainMenuMultiButton.Update(deltaTime);
        }
    }

    // =============== DRAW =============== //
    // ! spriteBatch Begin() / End() self
    internal void Draw(SpriteBatch spriteBatch)
    {
        RenderMaskedButtons(spriteBatch);

        spriteBatch.Begin();

        _mainMenuBackground.Draw(spriteBatch);
        spriteBatch.Draw(_buttonRenderTarget, Vector2.Zero, Color.White);
        _mainMenuXTCircle.Draw(spriteBatch);

        spriteBatch.End();
    }

    void RenderMaskedButtons(SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(_buttonRenderTarget);
        graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin();
        MainMenuButtonsDraw(spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin(blendState: blendState);
        spriteBatch.Draw(_maskCircleTexture, _mainMenuXTCircle.Position, null, Color.White, 0f, _mainMenuXTCircle.Origin, _mainMenuXTCircle.Scale, SpriteEffects.None, 0f);
        spriteBatch.End();

        graphicsDevice.SetRenderTarget(null);
    }

    void MainMenuButtonsDraw(SpriteBatch spriteBatch)
    {
        if (_mainMenuSoloButton.IsAppearing || (_mainMenuSoloButton.IsDisappearing && _mainMenuXTCircle.IsUnClicked))
        {
            _mainMenuSoloButton.Draw(spriteBatch);
        }

        if (_mainMenuMultiButton.IsAppearing || (_mainMenuMultiButton.IsDisappearing && _mainMenuXTCircle.IsUnClicked))
        {
            _mainMenuMultiButton.Draw(spriteBatch);
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

    void UpdateDeltaTime()
    {
        double nowUpdateTime = stopwatch.Elapsed.TotalSeconds;
        deltaTime = nowUpdateTime - lastUpdateTime;
        lastUpdateTime = nowUpdateTime;
    }
}