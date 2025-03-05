using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ShieldProgressBar
{
    private Texture2D texture;
    private Vector2 position;
    private float currentShieldPercentage;
    private float targetShieldPercentage;
    private SpriteFont font;
    private float countdownSpeed = 50f; // Speed of the countdown effect

    public ShieldProgressBar(Texture2D texture, Vector2 position, SpriteFont font)
    {
        this.texture = texture;
        this.position = position;
        this.currentShieldPercentage = 100f;
        this.targetShieldPercentage = 100f;
        this.font = font;
    }

    public void UpdateShield(float percentage)
    {
        targetShieldPercentage = MathHelper.Clamp(percentage, 0, 100);
    }

    public void Update(GameTime gameTime)
    {
        if (currentShieldPercentage > targetShieldPercentage)
        {
            currentShieldPercentage -= countdownSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentShieldPercentage < targetShieldPercentage)
            {
                currentShieldPercentage = targetShieldPercentage;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float radians = MathHelper.ToRadians(180 * (currentShieldPercentage / 100f));
        Color shieldColor = GetShieldColor(currentShieldPercentage);

        spriteBatch.Draw(texture, position, null, shieldColor, MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, (int)(texture.Height * (currentShieldPercentage / 100f))), shieldColor, MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);

        // Draw the shield percentage text
        string percentageText = $"{(int)currentShieldPercentage}%";
        Vector2 textSize = font.MeasureString(percentageText);
        Vector2 textPosition = new Vector2(position.X + texture.Width / 2 - textSize.X / 2, position.Y + texture.Height / 3 - textSize.Y / 2);
        spriteBatch.DrawString(font, percentageText, textPosition, Color.White);
    }

    private Color GetShieldColor(float percentage)
    {
        if (percentage > 75)
            return Color.Green;
        else if (percentage > 50)
            return Color.Yellow;
        else if (percentage > 25)
            return Color.Orange;
        else
            return Color.Red;
    }

    public void Reset()
    {
        currentShieldPercentage = 100f;
        targetShieldPercentage = 100f;
    }
}