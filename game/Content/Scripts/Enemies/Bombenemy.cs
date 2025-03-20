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
    public class BombEnemy : Enemy
    {
        public bool crossShot = false;

        private float explodeDistance = 100f;
        public BombEnemy(Texture2D sprite) : base(sprite) 
        {
            //maxHitPoints = 50;
            hitPoints = maxHitPoints;
            enemySprite = sprite;
            speed = 40f;
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
                position = new Vector2(-50, rand.Next(50, screenHeight - 50));
            } else {
                position = new Vector2(screenWidth + 50, rand.Next(50, screenHeight - 50));
            }
        }

        public new void SetTargetPosition(Vector2 playerPos)
        {
            targetPosition = playerPos;
        }

        public override void UpdateMovement(GameTime gameTime)
        {
            Vector2 direction = targetPosition - position;
            direction.Normalize();
            velocity = direction * speed;
            
            if (Vector2.Distance(position, targetPosition) <= explodeDistance) {
                hitPoints = 0;
            }

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}