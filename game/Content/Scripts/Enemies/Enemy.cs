using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System.Runtime.CompilerServices;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.content.Scripts;

namespace Blok3Game.content.Scripts.Enemies
{
    public class Enemy : GameObject
    {
        private Vector2 targetPosition;
        private float speed = 100f;
        private Texture2D enemySprite;
        private Random rand;
        private float shootTimer;
        private float shootInterval = 2f;
        public EnemyBulletManager enemyBulletManager;

        public Enemy(Texture2D sprite) : base() 
        {
            enemySprite = sprite;
            rand = new Random();
            SpawnAboveScreen();
        }

        private void SpawnAboveScreen() 
        {
            int screenWidth = 1920;
            position = new Vector2(rand.Next(50, screenWidth - 50), - 50);
        }

        public void SetTargetPosition(Vector2 playerPos)
        {
            targetPosition = playerPos;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = targetPosition - position;
            if(direction != Vector2.Zero)
            {
                direction.Normalize();
                velocity = direction * speed;
            }

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shootTimer >= shootInterval) {
                enemyBulletManager.SpawnAimedEnemyBullet1(position);
                shootTimer = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && enemySprite != null)
            {
                spriteBatch.Draw(enemySprite, position, Color.White);
            }
        }

        public override Rectangle BoundingBox 
        {
            get
            {
                if (enemySprite != null)
                {
                    return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 
                                          enemySprite.Width, enemySprite.Height);                }
                return base.BoundingBox;
            }
        }
    }
}