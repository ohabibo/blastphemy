using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class IntroScreen
{
    private Texture2D backgroundTexture;
    private Texture2D logoTexture;
    private SpriteFont font;
    private bool isFinished;
    private float fadeAlpha;
    private bool fadingIn;

    public bool IsFinished => isFinished;

    public IntroScreen(Texture2D backgroundTexture, Texture2D logoTexture, SpriteFont font)
    {
        this.backgroundTexture = backgroundTexture;
        this.logoTexture = logoTexture;
        this.font = font;
        isFinished = false;
        fadeAlpha = 0f;
        fadingIn = true;
    }

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            isFinished = true;
        }

        // Update fading effect
        float fadeSpeed = 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (fadingIn)
        {
            fadeAlpha += fadeSpeed;
            if (fadeAlpha >= 1f)
            {
                fadeAlpha = 1f;
                fadingIn = false;
            }
        }
        else
        {
            fadeAlpha -= fadeSpeed;
            if (fadeAlpha <= 0f)
            {
                fadeAlpha = 0f;
                fadingIn = true;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (backgroundTexture != null)
        {
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }

        if (logoTexture != null)
        {
            Vector2 logoPosition = new Vector2(
                (spriteBatch.GraphicsDevice.Viewport.Width - logoTexture.Width) / 2,
                (spriteBatch.GraphicsDevice.Viewport.Height - logoTexture.Height) / 2
            );
            spriteBatch.Draw(logoTexture, logoPosition, Color.White * fadeAlpha);
        }

        string message = "Press Enter to Start";
        Vector2 textSize = font.MeasureString(message);
        Vector2 position = new Vector2(
            (spriteBatch.GraphicsDevice.Viewport.Width - textSize.X) / 2,
            (spriteBatch.GraphicsDevice.Viewport.Height - textSize.Y) / 2 + 100
        );

        spriteBatch.DrawString(font, message, position, Color.White);
    }
}