using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Collections.Generic;
using Blok3Game.content.Scripts.Managers;
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
        private float blasphemyCharge;
        private GameState gameState;
        private List<PlayerAttack> bullets;
        private Blasphemy blasphemyAbility;
        private int bulletspeed;
        public Player(Texture2D playerSprite, GameState _GameState) : base()
        {
            blasphemyAbility = new Blasphemy();
            gameState = _GameState;
            sprite = playerSprite;
            maxHitPoints = 1000;
            hitPoints = maxHitPoints;
            isAlive = true;
            maxVelocity = 200;
            bulletspeed = 30;
            blasphemyCharge = 0f;
            // Start at bottom center
            position = new Vector2(400, 500); // Adjust for your screen dimensions
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            // Simple WASD movement for testing
            velocity = Vector2.Zero;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                DoBaseAttack();
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                velocity.X = -1;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                velocity.X = 1;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                velocity.Y = -1;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                velocity.Y = 1;
            if (!(velocity.X == 0 && velocity.Y == 0))
            {
                velocity = Vector2.Normalize(velocity) * maxVelocity;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && sprite != null)
            {
                spriteBatch.Draw(sprite, position, Color.White);
            }
        }
        public override void Update(GameTime gameTime)
        {
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
            List<GameObject> Enemies = gameState.enemyManager.GetChildren();
            foreach (GameObject obj in Enemies)
            {
                if (closestEnemy == Vector2.Zero)
                {
                    closestEnemy = obj.Position;
                }
                else
                {
                    Vector2 closestEnemyDistance =closestEnemy - this.Position;
                    Vector2 objEnemyDistance = obj.Position - this.Position;
                    if (closestEnemyDistance.Length() > objEnemyDistance.Length())
                    {
                        closestEnemy = obj.Position;
                    }
                }
            }
            bullets.Add(new PlayerAttack(Vector2.Normalize(closestEnemy - this.position) * bulletspeed));

        }
        public void AddToBlasphemy(float increment)
        {
            float newValue = blasphemyCharge + increment;
            if (newValue > 0)
            {
                blasphemyCharge = newValue;
            }
        }
        private void DoBlasphemy()
        {
            blasphemyCharge -= 100;
        }
    }
}