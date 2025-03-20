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
        protected Vector2 targetPosition;
        protected float speed = 100f;
        protected int hitPoints;
        private bool isAlive;
        protected int maxHitPoints = 30;
        protected Texture2D enemySprite;
        protected Random rand;


        public Enemy(Texture2D sprite) : base() 
        {
            hitPoints = maxHitPoints;
            isAlive = true;
            enemySprite = sprite;
            rand = new Random();
            SpawnOutOfScreen();
        }

        private void SpawnOutOfScreen() 
        {
            int screenWidth = 1920;
            int screenHeight = 1080;
            if (rand.Next(0, 2) == 1) {
                position = new Vector2(rand.Next(50, screenWidth - 50), - 50);
            } else if (rand.Next(0, 2) == 1) {
                position = new Vector2(-100, rand.Next(50, screenHeight - 50));
            } else {
                position = new Vector2(screenWidth + 50, rand.Next(50, screenHeight - 50));
            }
        }

        public void SetTargetPosition(Vector2 playerPos)
        {
            targetPosition = playerPos;
        }

        public virtual void UpdateMovement(GameTime gameTime)
        {
            Vector2 direction = targetPosition - position;
            if(direction != Vector2.Zero)
            {
                direction.Normalize();
                velocity = direction * speed;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            if(!isAlive)
            {
                visible = false;
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
                            enemySprite.Width, enemySprite.Height); }
                return base.BoundingBox;
            }
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
            int newHP = hitPoints;
            if (isAlive)
            {
                newHP = hitPoints - damage;
                if (newHP >= 0)
                {
                    hitPoints = newHP;
                }
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
                if (overHeal)
                { hitPoints = newHP; }
                else { if (newHP >= maxHitPoints) { hitPoints = maxHitPoints; } else { hitPoints = newHP; } }
            }
            else { Console.WriteLine("Enemy.Heal failed please input a positive value"); }
        }
        public int GetHP() { return hitPoints; }

        public bool IsAlive() { return isAlive; }
    }
}