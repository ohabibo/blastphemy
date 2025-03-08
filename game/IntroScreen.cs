// filepath: g:\School\HBO ICT GD\Blok 3\guulaaxeexaa84\game\IntroScreen.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class IntroScreen
{
    private Texture2D logoTexture;
    private SpriteFont font;
    private bool isFinished;
    private float fadeAlpha;
    private bool fadingIn;

    public bool IsFinished => isFinished;

    public IntroScreen(Texture2D logoTexture, SpriteFont font)
    {
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
        // Clear the screen with a black color
        spriteBatch.GraphicsDevice.Clear(Color.Black);

        // Begin the sprite batch
        spriteBatch.Begin();

        // Center the logo and text
        Vector2 logoPosition = new Vector2(
            (spriteBatch.GraphicsDevice.Viewport.Width - logoTexture.Width * 2) / 2,
            (spriteBatch.GraphicsDevice.Viewport.Height - logoTexture.Height * 2) / 2 - 50
        );
        Vector2 textPosition = new Vector2(
            (spriteBatch.GraphicsDevice.Viewport.Width - font.MeasureString("Press Enter to Start").X) / 2,
            (spriteBatch.GraphicsDevice.Viewport.Height + logoTexture.Height * 2) / 2
        );

        // Draw the logo and text
        spriteBatch.Draw(logoTexture, logoPosition, null, Color.White * fadeAlpha, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f); // Keep the logo bigger
        spriteBatch.DrawString(font, "Press Enter to Start", textPosition, Color.White);

        // End the sprite batch
        spriteBatch.End();
    }
}

