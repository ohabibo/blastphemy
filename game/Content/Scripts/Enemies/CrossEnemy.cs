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
    public class CrossEnemy : Enemy
    {
        public bool crossShot = false;
        public CrossEnemy(Texture2D sprite) : base(sprite) 
        {
            //maxHitPoints = 50;
            hitPoints = maxHitPoints;
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
            if(Vector2.Distance(position, targetPosition) >= 500)
            {
                direction.Normalize();
                velocity = direction * speed;
            }
            else 
            {
                velocity = Vector2.Zero;
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}