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

    public Player(Texture2D texture, Vector2 startPosition)
    {
        Texture = texture;
        Position = new Vector2(1280 / 2 - Texture.Width / 2, startPosition.Y); // Center horizontally
    }

    public void Update(GameTime gameTime, List<Bullet> bullets, Texture2D bulletTexture)
    {
        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.W)) Position.Y -= speed;
        if (keyboard.IsKeyDown(Keys.S)) Position.Y += speed;
        if (keyboard.IsKeyDown(Keys.A)) Position.X -= speed;
        if (keyboard.IsKeyDown(Keys.D)) Position.X += speed;

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
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}