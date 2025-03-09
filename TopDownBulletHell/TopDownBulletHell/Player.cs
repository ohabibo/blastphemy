using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Player
{
    public Vector2 Position;
    public Texture2D Texture;
    private float speed = 5f;
    private float shootCooldown = 0.5f; // Cooldown time in seconds
    private float shootTimer = 0f;
    public float ShieldPercentage { get; private set; } = 100f;
    public bool IsGameOver { get; private set; } = false;

    // Adjust the hitbox size here
    private int hitboxWidth = 20;
    private int hitboxHeight = 20;

    // Invincibility and knockback
    private bool isInvincible = false;
    private float invincibilityDuration = 2f; // Duration of invincibility in seconds
    private float invincibilityTimer = 0f;
    private Vector2 knockbackVelocity = Vector2.Zero;
    private float knockbackSpeed = 10f; // Speed of knockback

    public Rectangle BoundingBox => new Rectangle(
        (int)Position.X + (Texture.Width - hitboxWidth) / 2,
        (int)Position.Y + (Texture.Height - hitboxHeight) / 2,
        hitboxWidth,
        hitboxHeight
    );

    public Player(Texture2D texture, Vector2 startPosition)
    {
        Texture = texture;
        Position = new Vector2(1280 / 2 - Texture.Width / 2, startPosition.Y); // Center horizontally
    }

    public void Update(GameTime gameTime, List<Bullet> bullets, Texture2D bulletTexture)
    {
        if (IsGameOver) return;

        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.W)) Position.Y -= speed;
        if (keyboard.IsKeyDown(Keys.S)) Position.Y += speed;
        if (keyboard.IsKeyDown(Keys.A)) Position.X -= speed;
        if (keyboard.IsKeyDown(Keys.D)) Position.X += speed;

        // Apply knockback
        Position += knockbackVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        knockbackVelocity *= 0.9f; // Dampen knockback over time

        // Keep player inside screen bounds
        Position.X = MathHelper.Clamp(Position.X, 0, 1280 - Texture.Width); // Adjusted for new resolution width
        Position.Y = MathHelper.Clamp(Position.Y, 0, 720 - Texture.Height); // Adjusted for new resolution height

        // Shooting logic
        shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (keyboard.IsKeyDown(Keys.Space) && shootTimer >= shootCooldown)
        {
            BulletPatterns.ShootSpiralPattern(bullets, bulletTexture, Position, 0);
            shootTimer = 0f;
        }

        // Update invincibility timer
        if (isInvincible)
        {
            invincibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color drawColor = isInvincible ? Color.Red : Color.White; // Change color when invincible
        spriteBatch.Draw(Texture, Position, drawColor);
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (isInvincible) return;

        ShieldPercentage -= damage;
        if (ShieldPercentage <= 0)
        {
            IsGameOver = true;
        }
        else
        {
            isInvincible = true;
            knockbackVelocity = knockbackDirection * knockbackSpeed;
        }
    }

    public void Reset()
    {
        ShieldPercentage = 100f;
        IsGameOver = false;
        Position = new Vector2(1280 / 2 - Texture.Width / 2, 500); // Reset position
        isInvincible = false;
        invincibilityTimer = 0f;
        knockbackVelocity = Vector2.Zero;
    }
}