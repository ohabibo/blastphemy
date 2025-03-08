using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class AbilityMeter
{
    private Texture2D abilityTexture;
    private SpriteFont font;
    private bool abilityReady;

    public AbilityMeter(Texture2D abilityTexture, SpriteFont font)
    {
        this.abilityTexture = abilityTexture;
        this.font = font;
        this.abilityReady = false;
    }

    public void SetAbilityReady(bool ready)
    {
        this.abilityReady = ready;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
    {
        Color abilityColor = abilityReady ? Color.White : Color.White * 0.5f;
        spriteBatch.Draw(abilityTexture, position, null, abilityColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        if (abilityReady)
        {
            spriteBatch.Draw(abilityTexture, position, null, Color.Yellow * 0.5f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}