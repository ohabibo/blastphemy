// filepath: g:\School\HBO ICT GD\Blok 3\guulaaxeexaa84\game\ShieldProgressBar.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Blok3Game.Engine.GameObjects;
using System;

public class ShieldProgressBar : GameObject
{
    private Texture2D texture;
    private int maxShield;
    private int currentShield;
    private SpriteFont font;
    private AbilityMeter abilityMeter;
    private bool gameOver = false; // Kept for potential use by other systems.

    // Timers for hit effects
    private float hitEffectTimer = 0f;      // for red blink effect
    private float hitCountdownTimer = 0f;   // for countdown effect
    
    public ShieldProgressBar(Texture2D texture, Texture2D abilityTexture, int maxShield, SpriteFont font)
        : base(0, "ShieldProgressBar")
    {
        this.texture = texture;
        this.maxShield = maxShield;
        this.currentShield = maxShield;  // Ensures shield starts full
        this.font = font;
        this.abilityMeter = new AbilityMeter(abilityTexture, font);
    }

    // Expose game over status.
    public bool IsGameOver
    {
        get { return gameOver; }
    }

    // Call this method whenever the player is hit. 
    // Removes 2% from the shield and starts hit effects.
    public void TakeDamage()
    {
        if (gameOver)
            return;

        int damageValue = (int)(maxShield * 0.02f); // 2% damage
        currentShield -= damageValue;
        hitEffectTimer = 0.5f;    // red blink effect
        hitCountdownTimer = 2f;   // countdown duration
        
        if (currentShield <= 0)
        {
            currentShield = 0;
            gameOver = true;
        }
    }

    // You can also update the shield manually if needed.
    public void UpdateShield(int shield)
    {
        this.currentShield = shield;
        if (currentShield <= 0)
        {
            currentShield = 0;
            gameOver = true;
        }
    }

    public void SetAbilityReady(bool ready)
    {
        this.abilityMeter.SetAbilityReady(ready);
    }

    public override void Reset()
    {
        currentShield = maxShield;  // Full shield (should be 100)
        gameOver = false;
        hitEffectTimer = 0f;
        hitCountdownTimer = 0f;
        Console.WriteLine("Shield reset: currentShield = " + currentShield);
    }

    // (RestartGame method removed from UI; game over handling is moved elsewhere.)

    // Update timers for the hit effects. Call this from your game update loop.
    public override void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (hitEffectTimer > 0)
            hitEffectTimer -= delta;
        if (hitCountdownTimer > 0)
            hitCountdownTimer -= delta;
        // Optionally update nested objects
        // abilityMeter.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        float shieldPercentage = (float)currentShield / maxShield;
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
        Vector2 position = new Vector2(radius, spriteBatch.GraphicsDevice.Viewport.Height - radius);

        // Draw shield texture
        Rectangle sourceRectangle = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
        spriteBatch.Draw(texture, position, sourceRectangle, shieldColor, MathHelper.ToRadians(-90), new Vector2(radius, radius), 1f, SpriteEffects.None, 0f);

        // Calculate blinking red effect on shield text if hit recently.
        Color shieldTextColor = Color.White;
        if (hitEffectTimer > 0)
        {
            bool blink = ((int)(hitEffectTimer * 10) % 2 == 0);
            shieldTextColor = blink ? Color.Red : Color.White;
        }
        string shieldText = $"{(int)(shieldPercentage * 100)}%";
        Vector2 textSize = font.MeasureString(shieldText);
        spriteBatch.DrawString(font, shieldText, new Vector2(position.X - textSize.X / 10, position.Y - radius / 2 - textSize.Y / 2), shieldTextColor);

        // Draw the ability meter.
        Vector2 abilityPosition = new Vector2(position.X - 60, position.Y - radius - 500);
        float abilityScale = 2f;
        this.abilityMeter.Draw(spriteBatch, abilityPosition, abilityScale);

        // If the hit countdown timer is active, display the countdown above the shield bar.
        if (hitCountdownTimer > 0)
        {
            string countdownText = $"{Math.Ceiling(hitCountdownTimer)}";
            Vector2 countSize = font.MeasureString(countdownText);
            Vector2 countPosition = new Vector2(position.X - countSize.X / 2, position.Y - radius - countSize.Y - 10);
            spriteBatch.DrawString(font, countdownText, countPosition, Color.Yellow);
        }
    }
}