using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

public class Enemy
{
    public Vector2 Position;
    public Texture2D Texture;
    public bool IsActive;
    private float speed = 2f;

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

    public Enemy(Texture2D texture, Vector2 startPosition)
    {
        Texture = texture;
        Position = startPosition;
        IsActive = true;
    }

    public void Update()
    {
        Position.Y += speed;
        if (Position.Y > 720) IsActive = false; // Remove when off-screen
    }

    public void Shoot(List<Bullet> bullets, Texture2D bulletTexture, Vector2 playerPosition)
    {
        int bulletCount = 5;
        float angleStep = MathHelper.PiOver4 / (bulletCount - 1); // Spread angle

        Vector2 direction = playerPosition - Position;
        direction.Normalize();
        float baseAngle = (float)Math.Atan2(direction.Y, direction.X);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle - MathHelper.PiOver4 / 2 + i * angleStep;
            Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 4f;
            bullets.Add(new Bullet(bulletTexture, Position, velocity));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White); // Use Color.White to draw the texture with its original colors
    }
}