using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.Engine.Helpers;
using System;
using System.Collections.Generic;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.content.Scripts.Audio;

namespace Blok3Game.content.Scripts.Enemies.boss
{
    public class BossAttackPatterns
    {
        private BossEnemy boss;
        private Texture2D bulletTexture;
        private Random rand;
        private float timer = 0f;
        private float patternTimer = 0f;
        private float rotationAngle = 0f;
        private int currentSubPattern = 0;
        private Vector2 playerPosition;
        private EnemyBulletManager bulletManager;

        // Modify constructor to accept bulletManager and add null checks
        public BossAttackPatterns(BossEnemy boss, Texture2D bulletTexture, EnemyBulletManager bulletManager)
        {
            this.boss = boss;
            this.bulletTexture = bulletTexture;
            this.bulletManager = bulletManager ?? throw new ArgumentNullException(nameof(bulletManager));
            rand = new Random();
        }

        public void SetPlayerPosition(Vector2 playerPos)
        {
            playerPosition = playerPos;
        }

        public void Update(GameTime gameTime, int attackPatternStage)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += deltaTime;
            patternTimer += deltaTime;
            rotationAngle += deltaTime * 2f;

            // Execute attack pattern based on boss stage
            switch (attackPatternStage)
            {
                case 0:
                    Console.WriteLine("Stage 1: Basic patterns");
                    FMODAudio.Instance.SetParameter("Boss Stage", 1);
                    // Stage 1: Simpler patterns
                    if (timer >= 0.5f)
                    {
                        // Alternate between basic attacks
                        if (currentSubPattern == 0)
                        {
                            SpreadShot(5, 30f);
                            currentSubPattern = 1;
                        }
                        else
                        {
                            CircularBurst(8);
                            currentSubPattern = 0;
                        }
                        timer = 0f;
                    }
                    break;

                case 1:
                    Console.WriteLine("Stage 2: Complex patterns");
                    FMODAudio.Instance.SetParameter("Boss Stage", 2); 
                    // Stage 2: More complex patterns
                    if (timer >= 0.5f)
                    {
                        // Cycle through multiple attack types
                        switch (currentSubPattern)
                        {
                            case 0:
                                SpiralShot(3, 15f);
                                currentSubPattern = 1;
                                break;
                            case 1:
                                WavePattern(7, 30f);
                                currentSubPattern = 2;
                                break;
                            case 2:
                                CrossfirePattern();
                                currentSubPattern = 0;
                                break;
                        }
                        timer = 0f;
                    }
                    break;

                case 2:
                    Console.WriteLine("Stage 3: Intense bullet hell");
                    FMODAudio.Instance.SetParameter("Boss Stage", 3);
                    // Stage 3: Intense bullet hell
                    if (timer >= 0.5f)
                    {
                        // Execute multiple patterns simultaneously
                        switch ((int)(patternTimer % 5))
                        {
                            case 0:
                                DualSpiral(3, rotationAngle);
                                break;
                            case 1:
                                HomingBarrage(4);
                                break;
                            case 2:
                                GridPattern(5, 5);
                                break;
                            case 3:
                                RadialBurst(12);
                                break;
                            case 4:
                                SweepingLaser(20, 40f);
                                break;
                        }
                        timer = 0f;
                    }

                    // Every few seconds, add an additional pattern
                    if (patternTimer >= 5f)
                    {
                        CircularBurst(20);
                        WavePattern(5, 20f);
                        patternTimer = 0f;
                    }
                    break;
            }
        }

        #region Attack Patterns

        // Fires bullets in a spread pattern towards the player
        private void SpreadShot(int bulletCount, float spreadAngle)
        {
            if (boss == null || bulletManager == null) return;

            Vector2 toPlayer = playerPosition - boss.Position;
            if (toPlayer != Vector2.Zero)
            {
                toPlayer.Normalize();

                float baseAngle = (float)Math.Atan2(toPlayer.Y, toPlayer.X);
                float angleStep = MathHelper.ToRadians(spreadAngle) / (bulletCount - 1);

                for (int i = 0; i < bulletCount; i++)
                {
                    float currentAngle = baseAngle - MathHelper.ToRadians(spreadAngle) / 2 + angleStep * i;
                    Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                    
                    // Use the new method to spawn a directional bullet
                    bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
                }
            }
        }

        // Fires bullets in a circular pattern
        private void CircularBurst(int bulletCount)
        {
            if (boss == null || bulletManager == null) return;

            float angleStep = MathHelper.TwoPi / bulletCount;
            
            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = angleStep * i;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                
                bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
            }
        }

        // Fires bullets in a spiral pattern
        private void SpiralShot(int arms, float degreesPerShot)
        {
            if (boss == null || bulletManager == null) return;

            for (int i = 0; i < arms; i++)
            {
                float currentAngle = rotationAngle + MathHelper.TwoPi * i / arms;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                
                bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
            }
        }

        // Creates a dual spiral pattern
        private void DualSpiral(int armsPerSpiral, float rotation)
        {
            if (boss == null || bulletManager == null) return;

            float angleStep = MathHelper.TwoPi / armsPerSpiral;
            
            // First spiral
            for (int i = 0; i < armsPerSpiral; i++)
            {
                float currentAngle = rotation + angleStep * i;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                
                bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
            }
            
            // Second spiral (offset)
            for (int i = 0; i < armsPerSpiral; i++)
            {
                float currentAngle = rotation + angleStep * i + angleStep / 2;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                
                bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
            }
        }

        // Creates a wave-like pattern
        private void WavePattern(int bulletCount, float waveAmplitude)
        {
            if (boss == null || bulletManager == null) return;

            Vector2 toPlayer = playerPosition - boss.Position;
            if (toPlayer != Vector2.Zero)
            {
                toPlayer.Normalize();
                
                float baseAngle = (float)Math.Atan2(toPlayer.Y, toPlayer.X);
                
                for (int i = 0; i < bulletCount; i++)
                {
                    float offset = (float)Math.Sin(i * 0.5f) * MathHelper.ToRadians(waveAmplitude);
                    float currentAngle = baseAngle + offset;
                    Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                    
                    bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
                }
            }
        }

        // Creates a crossfire pattern with 4 arms
        private void CrossfirePattern()
        {
            if (boss == null || bulletManager == null) return;

            int arms = 4;
            int bulletsPerArm = 5;
            float armWidth = MathHelper.ToRadians(15f);
            
            for (int arm = 0; arm < arms; arm++)
            {
                float baseAngle = rotationAngle + arm * MathHelper.PiOver2;
                
                for (int i = 0; i < bulletsPerArm; i++)
                {
                    float spreadOffset = armWidth * (i - bulletsPerArm / 2) / bulletsPerArm;
                    float currentAngle = baseAngle + spreadOffset;
                    Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                    
                    bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
                }
            }
        }

        // Creates bullets that home in on the player
        private void HomingBarrage(int bulletCount)
        {
            if (boss == null || bulletManager == null) return;

            for (int i = 0; i < bulletCount; i++)
            {
                bulletManager.SpawnAimedEnemyBullet1(boss.Position);
            }
        }

        // Creates a grid pattern of bullets
        private void GridPattern(int rows, int cols)
        {
            if (boss == null || bulletManager == null) return;

            Vector2 screenSize = new Vector2(1920, 1080);
            float xStep = screenSize.X / (cols + 1);
            float yStep = screenSize.Y / (rows + 1);
            
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Vector2 targetPos = new Vector2(xStep * (col + 1), yStep * (row + 1));
                    
                    // Calculate direction to the target grid position
                    Vector2 direction = targetPos - boss.Position;
                    if (direction != Vector2.Zero)
                    {
                        direction.Normalize();
                        bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
                    }
                }
            }
        }

        // Creates a radial burst pattern with bullets of different speeds
        private void RadialBurst(int bulletCount)
        {
            if (boss == null || bulletManager == null) return;

            float angleStep = MathHelper.TwoPi / bulletCount;
            
            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = angleStep * i;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                
                bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
            }
        }

        // Creates a sweeping laser-like pattern of bullets
        private void SweepingLaser(int bulletCount, float sweepAngle)
        {
            if (boss == null || bulletManager == null) return;

            Vector2 toPlayer = playerPosition - boss.Position;
            if (toPlayer != Vector2.Zero)
            {
                toPlayer.Normalize();
                
                float baseAngle = (float)Math.Atan2(toPlayer.Y, toPlayer.X);
                float startAngle = baseAngle - MathHelper.ToRadians(sweepAngle / 2);
                float angleStep = MathHelper.ToRadians(sweepAngle) / bulletCount;
                
                for (int i = 0; i < bulletCount; i++)
                {
                    float currentAngle = startAngle + angleStep * i;
                    Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                    
                    bulletManager.SpawnDirectionalEnemyBullet(boss.Position, direction);
                }
            }
        }

        #endregion
    }
}