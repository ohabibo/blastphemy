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
    public class TopEnemy : Enemy
    {

        public bool goingLeft = false;
        public bool crossShot = false;
        public TopEnemy(Texture2D sprite) : base(sprite) 
        {
            hitPoints = maxHitPoints;
            enemySprite = sprite;
            rand = new Random();
            SpawnAboveScreen();
            targetPosition.X = position.X;
            targetPosition.Y = rand.Next(30, 80);
            speed = rand.Next(80, 120);
        }

        private void SpawnAboveScreen() 
        {
            int screenWidth = 1520;
            position = new Vector2(rand.Next(50, screenWidth - 50), - 50);
        }

        public override void UpdateMovement(GameTime gameTime)
        {
            int screenWidth = 1520;
            Vector2 direction = targetPosition - position;
            if(Vector2.Distance(position, targetPosition) >= 10)
            {
                direction.Normalize();
                velocity = direction * speed;
            }
            else 
            {
                //Kies random richting om in te bewegen
                velocity = Vector2.Zero;
                if (rand.Next(0, 2) == 1) {         targetPosition.X = 50;                      goingLeft = true;          } 
                else {                              targetPosition.X = screenWidth - 50;        goingLeft = false;          }
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}