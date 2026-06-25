using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class MainMenuSoloButton
{
    Texture2D _texture;
    Vector2 _position, _origin;
    float _scale;
    Rectangle _area;
    
    public MainMenuSoloButton(int screenWidth, int screenHeight)
    {
        _position = new(screenWidth / 1.6f, screenHeight / 3.5f);
        _scale = 0.55f;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("MainMenu/Solo");
        _origin = new(_texture.Width / 2, _texture.Height / 2);
        UpdateArea();
    }

    public void Update()
    {
        mainMenuSolo_Mousehover();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
    }

    void mainMenuSolo_Mousehover()
    {
        if (ObjectContainsCursor())_scale = 0.6f;
        else _scale = 0.55f;

        UpdateArea();
    }

    bool ObjectContainsCursor() =>_area.Contains(MouseInputManager.MousePosition);

    void UpdateArea()
    {
        int Width = (int)(_texture.Width * _scale);
        int Height = (int)(_texture.Height * _scale);

        _area = new((int)_position.X - Width / 2,
                    (int)_position.Y - Height / 2,
                    Width,
                    Height);
    }
}