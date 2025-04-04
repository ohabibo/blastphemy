using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Collections.Generic;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.content.Scripts.Enemies.boss;
using Microsoft.Xna.Framework.Input;
using Blok3Game.GameStates;

namespace Blok3Game.content.Scripts
{
    // A temporary placeholder for the player
    public class Player : GameObject
    {
        private Texture2D sprite;
        private int hitPoints;
        private bool isAlive;
        private int maxHitPoints;
        private int maxVelocity;
        public float blasphemyCharge;
        private GameState gameState;
        private List<PlayerAttack> bullets;
        private Blasphemy blasphemyAbility;
        private int bulletspeed;
        private float attackCooldownCounter;
        private float attackCooldown;
        private int baseAttackDamage;
        private int abilityAttackDamage;
        private Vector2 aimDirection;
        public Player(Texture2D playerSprite, GameState _gameState) : base()
        {
            aimDirection = new Vector2(0,1);
            bullets = new List<PlayerAttack>();
            gameState = _gameState;
            blasphemyAbility = new Blasphemy(_gameState);
            sprite = playerSprite;
            maxHitPoints = 100;
            hitPoints = maxHitPoints;
            isAlive = true;
            maxVelocity = 300;
            bulletspeed = 800;
            attackCooldownCounter = 0f;
            attackCooldown = 150f;
            blasphemyCharge = 0f;
            baseAttackDamage = 10;
            abilityAttackDamage = 50;
            // Start at bottom center
            position = new Vector2(400, 500); // Adjust for your screen dimensions
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            // Simple WASD movement for testing
            velocity = Vector2.Zero;
            if (inputHelper.IsKeyDown(Keys.Space))
            {
                if (attackCooldownCounter <= 0)
                {
                    DoBaseAttack();
                    attackCooldownCounter = attackCooldown;
                }
            }
            if (inputHelper.IsKeyDown(Keys.F)) { DoBlasphemy(); }

            if (inputHelper.IsKeyDown(Keys.A) && position.X > 0) { velocity.X = -1; }

            if (inputHelper.IsKeyDown(Keys.D) && position.X < 1920) { velocity.X = 1; }

            if (inputHelper.IsKeyDown(Keys.W) && position.Y > 0) { velocity.Y = -1; }

            if (inputHelper.IsKeyDown(Keys.S) && position.Y < 1080) { velocity.Y = 1; }

            if (!(velocity.X == 0 && velocity.Y == 0)) { velocity = Vector2.Normalize(velocity) * maxVelocity; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && sprite != null)
            {
                spriteBatch.Draw(sprite, position, Color.White);
            }

            foreach (PlayerAttack obj in bullets)
            {
                obj.Draw(gameTime, spriteBatch);
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (attackCooldownCounter > 0) { attackCooldownCounter -= (float)gameTime.ElapsedGameTime.TotalMilliseconds; }
            foreach (PlayerAttack obj in bullets)
            {
                obj.Update(gameTime);
            }

            foreach (GameObject obj in gameState.enemyManager.GetChildren())
            {
                if (obj is Enemy enemy && enemy.Visible)
                {
                    foreach (PlayerAttack bullet in bullets)
                    {
                        if (bullet.CheckCollision(enemy.BoundingBox))
                        {
                            enemy.Damage(bullet.Damage);
                            bullet.Visible = false;
                            bullets.Remove(bullet);
                            break;
                        }
                    }
                }
            }

            foreach (GameObject obj in gameState.enemyBulletManager.GetChildren())
            {
                if (obj is EnemyBullet enemyBullet && enemyBullet.Visible)
                {
                    if (enemyBullet.CheckCollision(this.BoundingBox))
                    {
                        this.Damage(enemyBullet.Damage);
                        // Update the shield UI as well:
                        gameState.shieldProgressBar.TakeDamage();
                        enemyBullet.Visible = false;
                        gameState.enemyBulletManager.Remove(enemyBullet);
                        Console.WriteLine($"Player hit by enemy bullet. Player HP: {hitPoints}");
                    }
                }
            }

            foreach (GameObject obj in gameState.bossManager.GetChildren())
            {
                if (obj is BossEnemy bossEnemy && bossEnemy.Visible)
                {
                    foreach (PlayerAttack bullet in bullets)
                    {
                        if (bullet.CheckCollision(bossEnemy.BoundingBox))
                        {
                            bossEnemy.Damage(bullet.Damage);
                            bullet.Visible = false;
                            bullets.Remove(bullet);
                            break;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
        public void SetHP(int newHP)
        {
            if (newHP >= 0)
            {
                hitPoints = newHP;
            }
            else { Console.WriteLine("Failed to setHP due to invalid input. HP cannot be lower than 0 Inputted HP was " + newHP); }
        }
        public void Damage(int damage)
        {
            int newHP;
            if (isAlive)
            {
                newHP = hitPoints - damage;
                if (newHP >= 0) hitPoints = newHP;
                else
                {
                    hitPoints = 0;
                    isAlive = false;
                }
            }
        }
        public void Heal(int heal, bool overHeal)
        {
            if (heal > 0)
            {
                int newHP = hitPoints + heal;
                if (overHeal) { hitPoints = newHP; }

                else { if (newHP >= maxHitPoints) { hitPoints = maxHitPoints; } else { hitPoints = newHP; } }
            }
            else { Console.WriteLine("Player.Heal failed please input a positive value"); }
        }
        public int GetHP() { return hitPoints; }
        public void SetMaxVelocity(int newMaxVelocity)
        {
            if (newMaxVelocity >= 0)
            {
                maxVelocity = newMaxVelocity;
            }
            else { Console.WriteLine("Couldn't set Player.maxVelocity please input a value greater than zero"); }
        }
        private void DoBaseAttack()
        {
            Vector2 closestEnemy = Vector2.Zero;
            Vector2 playerCenterPosition = this.Position;
            playerCenterPosition.X += sprite.Width / 2;
            playerCenterPosition.Y += sprite.Height / 2;
            Vector2 bulletStartPosition = playerCenterPosition;
            bulletStartPosition.X -= gameState.playerBulletTexture.Width / 2 - 5/2;
            bulletStartPosition.Y -= gameState.playerBulletTexture.Height / 2 - 5/2;
            Vector2 enemyOffset = new Vector2();
            List<GameObject> Enemies = gameState.enemyManager.GetChildren();
            List<GameObject> Boss = gameState.bossManager.GetChildren();

            foreach (GameObject obj in Boss)
            {
                if (obj is BossEnemy bossEnemy && bossEnemy.Visible)
                {
                    if (closestEnemy == Vector2.Zero) { closestEnemy = bossEnemy.Position; }
                    else
                    {
                        float closestEnemyDistance = Vector2.Distance(closestEnemy, playerCenterPosition);
                        float objEnemyDistance = Vector2.Distance(bossEnemy.Position, playerCenterPosition);
                        if (closestEnemyDistance > objEnemyDistance)
                            closestEnemy = bossEnemy.Position;
                        enemyOffset.X = bossEnemy.GetTexture().Width / 2;
                        enemyOffset.Y = bossEnemy.GetTexture().Height / 2;
                    }
                }
            }
            if (closestEnemy == Vector2.Zero) { } else aimDirection = Vector2.Normalize(closestEnemy + enemyOffset - bulletStartPosition);
            foreach (Enemy obj in Enemies)
            {
                if (!obj.Visible) continue;

                if (closestEnemy == Vector2.Zero) { closestEnemy = obj.Position; }
                else
                {
                    float closestEnemyDistance = Vector2.Distance(closestEnemy, playerCenterPosition);
                    float objEnemyDistance = Vector2.Distance(obj.Position, playerCenterPosition);
                    if (closestEnemyDistance > objEnemyDistance)
                    closestEnemy = obj.Position;
                    enemyOffset.X = obj.GetTexture().Width / 2;
                    enemyOffset.Y = obj.GetTexture().Height / 2;
                }
            }
            if(closestEnemy == Vector2.Zero) {} else aimDirection = Vector2.Normalize(closestEnemy + enemyOffset - bulletStartPosition);
            bullets.Add(
                new PlayerAttack(
                    aimDirection * bulletspeed,
                    gameState.playerBulletTexture,
                    bulletStartPosition,
                    baseAttackDamage
                    )
                );
        }

        public void AddToBlasphemy(float increment)
        {
            float newValue = blasphemyCharge + increment;
            if (newValue > 0 && newValue <= 100)
            {
                blasphemyCharge = newValue;
            }
        }
        private void DoBlasphemy()
        {
            if (blasphemyCharge == 100)
            {
                blasphemyCharge -= 100;
                blasphemyAbility.trigger(abilityAttackDamage);
            }
            Console.WriteLine(blasphemyCharge);
        }

        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); }
        }
    }
}