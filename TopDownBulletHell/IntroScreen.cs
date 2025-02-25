using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class IntroScreen
{
    private Texture2D backgroundTexture;
    private SpriteFont font;
    private bool isFinished;

    public bool IsFinished => isFinished;

    public IntroScreen(Texture2D backgroundTexture, SpriteFont font)
    {
        this.backgroundTexture = backgroundTexture;
        this.font = font;
        isFinished = false;
    }

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            isFinished = true;
        }
    }

public void Draw(SpriteBatch spriteBatch)
{
    if (backgroundTexture != null)
    {
        spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
    }

    string message = "Press Enter to Start";
    Vector2 textSize = font.MeasureString(message);
    Vector2 position = new Vector2(
        (spriteBatch.GraphicsDevice.Viewport.Width - textSize.X) / 2,
        (spriteBatch.GraphicsDevice.Viewport.Height - textSize.Y) / 2
    );

    spriteBatch.DrawString(font, message, position, Color.White);
    }
}