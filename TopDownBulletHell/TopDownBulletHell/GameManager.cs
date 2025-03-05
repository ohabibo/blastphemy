using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public class GameManager
{
    public Player Player;
    public List<Bullet> Bullets = new List<Bullet>();
    public List<Bullet> EnemyBullets = new List<Bullet>();
    public List<Enemy> Enemies = new List<Enemy>();

    public Texture2D PlayerTexture;
    public Texture2D BulletTexture;
    public Texture2D EnemyTexture;
    public Texture2D EnemyBulletTexture;
    private SpriteFont font;
    private ShieldProgressBar shieldProgressBar;

    public GameManager(Texture2D playerTexture, Texture2D bulletTexture, Texture2D enemyTexture, Texture2D enemyBulletTexture, SpriteFont font, Texture2D shieldTexture, SpriteFont shieldFont)
    {
        PlayerTexture = playerTexture;
        BulletTexture = bulletTexture;
        EnemyTexture = enemyTexture;
        EnemyBulletTexture = enemyBulletTexture;
        this.font = font;

        Player = new Player(PlayerTexture, new Vector2(1280 / 2 - PlayerTexture.Width / 2, 500)); // Center horizontally
        shieldProgressBar = new ShieldProgressBar(shieldTexture, new Vector2(10, 600), shieldFont); // Position at bottom left
    }

    public void Update(GameTime gameTime)
    {
        if (Player.IsGameOver)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                RestartGame();
            }
            else
            {
                shieldProgressBar.Update(gameTime); // Continue updating the shield progress bar
            }
            return;
        }

        Player.Update(gameTime, Bullets, BulletTexture);

        foreach (var bullet in Bullets) bullet.Update();
        Bullets.RemoveAll(b => !b.IsActive);

        foreach (var enemy in Enemies)
        {
            enemy.Update(gameTime); // Pass the gameTime parameter here
            if (enemy.IsActive)
            {
                enemy.Shoot(EnemyBullets, EnemyBulletTexture, Player.Position);
            }
        }
        Enemies.RemoveAll(e => !e.IsActive);

        foreach (var bullet in EnemyBullets) bullet.Update();
        EnemyBullets.RemoveAll(b => !b.IsActive);

        // Collision detection between player's bullets and enemies
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

        // Collision detection between enemy's bullets and player
        for (int i = EnemyBullets.Count - 1; i >= 0; i--)
        {
            if (EnemyBullets[i].BoundingBox.Intersects(Player.BoundingBox))
            {
                EnemyBullets[i].IsActive = false;
                Player.TakeDamage(25f); // Each bullet takes away 25%
                shieldProgressBar.UpdateShield(Player.ShieldPercentage);
            }
        }

        if (Enemies.Count == 0)
        {
            Enemies.Add(new Enemy(EnemyTexture, new Vector2(1280 / 2 - EnemyTexture.Width / 2, 0))); // Center horizontally
        }

        shieldProgressBar.Update(gameTime); // Update the shield progress bar
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in Bullets)
            bullet.Draw(spriteBatch);

        foreach (var enemy in Enemies)
            enemy.Draw(spriteBatch);

        foreach (var bullet in EnemyBullets)
            bullet.Draw(spriteBatch);

        // Draw the shield progress bar
        shieldProgressBar.Draw(spriteBatch);

        // Draw the player on top of the shield progress bar
        Player.Draw(spriteBatch);

        if (Player.IsGameOver)
        {
            string gameOverMessage = "Game Over! Press R to Restart";
            Vector2 textSize = font.MeasureString(gameOverMessage);
            Vector2 position = new Vector2(
                (spriteBatch.GraphicsDevice.Viewport.Width - textSize.X) / 2,
                (spriteBatch.GraphicsDevice.Viewport.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, gameOverMessage, position, Color.White);
        }
    }

    private void RestartGame()
    {
        Player.Reset();
        Bullets.Clear();
        EnemyBullets.Clear();
        Enemies.Clear();
        Enemies.Add(new Enemy(EnemyTexture, new Vector2(1280 / 2 - EnemyTexture.Width / 2, 0))); // Center horizontally
        shieldProgressBar.Reset(); // Reset the shield progress bar
    }
}