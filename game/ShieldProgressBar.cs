// filepath: g:\School\HBO ICT GD\Blok 3\guulaaxeexaa84\game\ShieldProgressBar.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ShieldProgressBar
{
    private Texture2D texture;
    private int maxShield;
    private int currentShield;
    private SpriteFont font;
    private AbilityMeter abilityMeter;

    public ShieldProgressBar(Texture2D texture, Texture2D abilityTexture, int maxShield, SpriteFont font)
    {
        this.texture = texture;
        this.maxShield = maxShield;
        this.currentShield = maxShield;
        this.font = font;
        this.abilityMeter = new AbilityMeter(abilityTexture, font);
    }

    public void UpdateShield(int shield)
    {
        this.currentShield = shield;
    }

    public void SetAbilityReady(bool ready)
    {
        this.abilityMeter.SetAbilityReady(ready);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        float shieldPercentage = (float)this.currentShield / this.maxShield;

        Color shieldColor;
        if (shieldPercentage > 0.5f)
        {
            shieldColor = Color.Green;
        }
        else if (shieldPercentage > 0.25f)
        {
            shieldColor = Color.Orange;
        }
        else
        {
            shieldColor = Color.Red;
        }

        int radius = texture.Width / 4;
        Vector2 position = new Vector2(radius, graphicsDevice.Viewport.Height - radius);

        Rectangle sourceRectangle = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
        spriteBatch.Draw(texture, position, sourceRectangle, shieldColor, MathHelper.ToRadians(-90), new Vector2(radius, radius), 1f, SpriteEffects.None, 0f);

        string shieldText = $"{(int)(shieldPercentage * 100)}%";
        Vector2 textSize = font.MeasureString(shieldText);
        spriteBatch.DrawString(font, shieldText, new Vector2(position.X - textSize.X / 10, position.Y - radius / 2 - textSize.Y / 2), Color.White);

        Vector2 abilityPosition = new Vector2(position.X - 60, position.Y - radius - 500);
        float abilityScale = 2f;
        this.abilityMeter.Draw(spriteBatch, abilityPosition, abilityScale);
    }
}