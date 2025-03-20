using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;

public class AbilityMeter : GameObject
{
    private Texture2D texture;
    private SpriteFont font;
    private bool abilityReady;

    public AbilityMeter(Texture2D texture, SpriteFont font)
        : base(0, "AbilityMeter")
    {
        this.texture = texture;
        this.font = font;
        abilityReady = false;
    }

    public void SetAbilityReady(bool ready)
    {
        abilityReady = ready;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
    {
        Color abilityColor = abilityReady ? Color.White : Color.White * 0.5f;
        spriteBatch.Draw(texture, position, null, abilityColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        if (abilityReady)
        {
            spriteBatch.Draw(texture, position, null, Color.Yellow * 0.5f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}