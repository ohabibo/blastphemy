using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class HitComboCounter
{
    private SpriteFont font;
    private int hitComboCount = 0;
    private float comboDelay = 2f; // seconds before the counter resets
    private float comboTimer = 0f;
    public bool IsActive { get; private set; }

    public HitComboCounter(SpriteFont font)
    {
        this.font = font;
        Reset();
    }
    
    // Call this method whenever an enemy is hit.
    public void RegisterHit()
    {
        hitComboCount++;
        comboTimer = comboDelay; // restart timer
        IsActive = true;
        Console.WriteLine("Hit registered! Current count: " + hitComboCount);
    }
    
    // Update the timer. When it reaches 0, reset the combo.
    public void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (comboTimer > 0)
        {
            comboTimer -= delta;
        }
        else
        {
            Reset();
        }
    }
    
    private void Reset()
    {
        hitComboCount = 0;
        comboTimer = 0f;
        IsActive = false;
    }
    
    // Draw the hit combo counter at the given position.
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        // Only draw the hit combo counter if it’s active.
        if (!IsActive)
            return;
        
        string text = hitComboCount + " Hit" + (hitComboCount > 1 ? "s" : "");
        spriteBatch.DrawString(font, text, position, color);
    }

    public string GetDisplayText()
    {
        if (!IsActive)
            return string.Empty;
        return hitComboCount + " Hit" + (hitComboCount > 1 ? "s" : "");
    }
}