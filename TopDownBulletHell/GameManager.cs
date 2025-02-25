using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class GameManager
{
    public Player Player;
    public List<Bullet> Bullets = new List<Bullet>();
    public List<Enemy> Enemies = new List<Enemy>();

    public Texture2D PlayerTexture;
    public Texture2D BulletTexture;
    public Texture2D EnemyTexture;

public GameManager(Texture2D playerTexture, Texture2D bulletTexture, Texture2D enemyTexture)
{
    PlayerTexture = playerTexture;
    BulletTexture = bulletTexture;
    EnemyTexture = enemyTexture;

    Player = new Player(PlayerTexture, new Vector2(1280 / 2 - PlayerTexture.Width / 2, 500)); // Center horizontally
}

public void Update(GameTime gameTime) // Add gameTime parameter
{
    Player.Update(gameTime, Bullets, BulletTexture);

    foreach (var bullet in Bullets) bullet.Update();
    Bullets.RemoveAll(b => !b.IsActive);

    foreach (var enemy in Enemies) enemy.Update();
    Enemies.RemoveAll(e => !e.IsActive);

    // Collision detection
    for (int i = Bullets.Count - 1; i >= 0; i--)
    {
        for (int j = Enemies.Count - 1; j >= 0; j--)
        {
            if (Bullets[i].BoundingBox.Intersects(Enemies[j].BoundingBox))
            {
                Bullets[i].IsActive = false;
                Enemies[j].IsActive = false;
            }
        }
    }

    if (Enemies.Count == 0)
    {
        Enemies.Add(new Enemy(EnemyTexture, new Vector2(1280 / 2 - EnemyTexture.Width / 2, 0))); // Center horizontally
    }
}

    public void Draw(SpriteBatch spriteBatch)
    {
        Player.Draw(spriteBatch);

        foreach (var bullet in Bullets)
            bullet.Draw(spriteBatch);

        foreach (var enemy in Enemies)
            enemy.Draw(spriteBatch);
    }
}