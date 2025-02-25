using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        if (Position.Y > 600) IsActive = false; // Remove when off-screen
    }

    public void Shoot(List<Bullet> bullets, Texture2D bulletTexture)
    {
        bullets.Add(new Bullet(bulletTexture, Position, new Vector2(0, 4))); // Simple downward shot
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White); // Use Color.White to draw the texture with its original colors
    }
}