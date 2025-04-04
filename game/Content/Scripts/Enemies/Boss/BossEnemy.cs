using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.Engine.Helpers;
using System;
using System.Collections.Generic;
using Blok3Game.content.Scripts.Enemies;

namespace Blok3Game.content.Scripts.Enemies.boss {
    public class BossEnemy : GameObject
    {
        private Vector2 targetPosition;
        private float speed = 100f;
        private int hitPoints;
        private bool isAlive;
        private int maxHitPoints = 1000; // Adjusted for boss
        private Texture2D enemySprite;
        private Texture2D bulletTexture;
        private Random rand;
        private int attackPatternStage = 0; // For different attack patterns
        private BossAttackPatterns attackPatterns;
        private float stageMovementTimer = 0f;
        private List<EnemyBullet> bullets = new List<EnemyBullet>();
        private EnemyBulletManager enemyBulletManager;

        public BossEnemy(Texture2D sprite, Texture2D bulletSprite, EnemyBulletManager bulletManager) : base()
        {
            hitPoints = maxHitPoints;
            isAlive = true;
            enemySprite = sprite;
            rand = new Random();
            SpawnOutOfScreen();
            bulletTexture = bulletSprite;
            enemyBulletManager = bulletManager;
            BossAttackPatterns patterns = new BossAttackPatterns(this, bulletTexture, enemyBulletManager);
            attackPatterns = patterns;
        }

        private void SpawnOutOfScreen() 
        {
            int screenWidth = 1920;
            int screenHeight = 1080;
            if (rand.Next(0, 2) == 1) {         position = new Vector2(rand.Next(50, screenWidth - 50), - 50);                      }        
            else if (rand.Next(0, 2) == 1) {    position = new Vector2(-50, rand.Next(50, screenHeight - 50));                      } 
            else {                              position = new Vector2(screenWidth + 50, rand.Next(50, screenHeight - 50));         }
        }

        public void SetTargetPosition(Vector2 playerPos)
        {
            targetPosition = playerPos;
            attackPatterns.SetPlayerPosition(playerPos);
        }

        public virtual void UpdateMovement(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stageMovementTimer += deltaTime;

            // Different movement patterns based on stage
            switch (attackPatternStage)
            {
                case 0:
                    // Stage 1: Simple tracking movement
                    MoveTowardsTarget(deltaTime);
                    break;
                case 1:
                    // Stage 2: Erratic movement
                    if (stageMovementTimer > 3f)
                    {
                        // Pick a new random position on screen
                        targetPosition = new Vector2(
                            rand.Next(200, 1720),  // Keep away from edges
                            rand.Next(200, 880)
                        );
                        stageMovementTimer = 0f;
                    }
                    MoveTowardsTarget(deltaTime);
                    break;
                case 2:
                    // Stage 3: Circle around player
                    CircleAroundPlayer(deltaTime);
                    break;
            }
        }
        public Texture2D GetTexture() { return enemySprite; }

        private void MoveTowardsTarget(float deltaTime)
        {
            Vector2 direction = targetPosition - position;
            if (direction.Length() > 5f) // Only move if not already at target
            {
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    velocity = direction * speed;
                }
            }
            else
            {
                velocity = Vector2.Zero;
            }
        }

        private void CircleAroundPlayer(float deltaTime)
        {
            // Create a circular path around the player
            float radius = 300f; // Distance from player
            float circleSpeed = 1; // Speed of circulation
            
            float angle = stageMovementTimer * circleSpeed;
            position = targetPosition + new Vector2(
                (float)Math.Cos(angle) * radius,
                (float)Math.Sin(angle) * radius
            );
        }

        public override void Update(GameTime gameTime)
        {
            if (!isAlive)
            {
                visible = false;
                return;
            }
            
            UpdateMovement(gameTime);
            
            // Update attack patterns
            attackPatterns.Update(gameTime, attackPatternStage);
            
            // Check for health and change stages if necessary
            if (hitPoints < maxHitPoints * 2 / 3 && attackPatternStage == 0)
            {
                attackPatternStage = 1;
                speed = 110f; // Increase speed in second stage
            }
            else if (hitPoints < maxHitPoints / 3 && attackPatternStage == 1)
            {
                attackPatternStage = 2;
                speed = 120f; // Further increase speed in final stage
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && enemySprite != null)
            {
                spriteBatch.Draw(
                    enemySprite, 
                    position, 
                    null, 
                    Color.White, 
                    0f, 
                    new Vector2(enemySprite.Width / 2, enemySprite.Height / 2), 
                    1f, 
                    SpriteEffects.None, 
                    0f
                );
            }
        }

        public void Damage(int damage)
        {
            hitPoints -= damage;
            if (hitPoints <= 0)
            {
                isAlive = false;
            }
        }

        public override Rectangle BoundingBox
        {
            get
            {
                if (enemySprite != null)
                {
                    return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y,
                            enemySprite.Width, enemySprite.Height);
                }
                return base.BoundingBox;
            }
        }

        public bool IsAlive => isAlive;
        public Vector2 Position => position;
    }
}