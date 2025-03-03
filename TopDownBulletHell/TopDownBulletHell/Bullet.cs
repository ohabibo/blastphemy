using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Bullet
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Texture2D Texture;
    public bool IsActive;

    public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

    public Bullet(Texture2D texture, Vector2 position, Vector2 velocity)
    {
        Texture = texture;
        Position = position;
        Velocity = velocity;
        IsActive = true;
    }

    public void Update()
    {
        Position += Velocity;
        if (Position.Y < 0 || Position.Y > 720) // Off-screen check
            IsActive = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}