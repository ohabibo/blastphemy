// filepath: g:\School\HBO ICT GD\Blok 3\guulaaxeexaa84\game\ShieldProgressBar.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ShieldProgressBar
{
    private Texture2D texture;
    private int maxShield;
    private int currentShield;
    private SpriteFont font;

    public ShieldProgressBar(Texture2D texture, int maxShield, SpriteFont font)
    {
        this.texture = texture;
        this.maxShield = maxShield;
        this.currentShield = maxShield;
        this.font = font;
    }

    public void UpdateShield(int shield)
    {
        this.currentShield = shield;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the half-circle shield
        int radius = 100;
        int diameter = radius * 2;
        Texture2D shieldTexture = CreateCircleTexture(spriteBatch.GraphicsDevice, radius, Color.Green);
        spriteBatch.Draw(shieldTexture, new Vector2(0, spriteBatch.GraphicsDevice.Viewport.Height - radius), null, Color.White, 0f, new Vector2(radius, radius), 1f, SpriteEffects.None, 0f);

        // Draw the shield percentage text
        string shieldText = $"{(int)((float)this.currentShield / this.maxShield * 100)}%";
        Vector2 textSize = font.MeasureString(shieldText);
        spriteBatch.DrawString(font, shieldText, new Vector2(radius - textSize.X / 7, spriteBatch.GraphicsDevice.Viewport.Height - radius + 50), Color.White);
    }

    private Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color)
    {
        int diameter = radius * 2;
        Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
        Color[] colorData = new Color[diameter * diameter];

        float radiusSquared = radius * radius;

        for (int x = 0; x < diameter; x++)
        {
            for (int y = 0; y < diameter; y++)
            {
                int index = x * diameter + y;
                Vector2 position = new Vector2(x - radius, y - radius);
                if (position.LengthSquared() <= radiusSquared && position.Y >= 0)
                {
                    colorData[index] = color;
                }
                else
                {
                    colorData[index] = Color.Transparent;
                }
            }
        }

        texture.SetData(colorData);
        return texture;
    }
}