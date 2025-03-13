using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System.Runtime.CompilerServices;

namespace Blok3Game.content.Scripts.Enemies
{
    public class EnemyBullet : GameObject
    {
        private Vector2 targetPosition;
        private float speed = 250f;
        //private float timer;
        //private float deathTime = 0f;
        private Vector2 direction;
        private Texture2D enemyBulletSprite;
        private int damage = 10;
        public EnemyBullet(Texture2D sprite, Vector2 enemyPos, Vector2 playerPos) : base() 
        {
            enemyBulletSprite = sprite;
            position = enemyPos;
            targetPosition = playerPos;
            direction = targetPosition - position;
        }

        public void SetTargetPosition(Vector2 playerPos)
        {
             targetPosition = playerPos;
        }

        public override void Update(GameTime gameTime)
        {
            if(direction != Vector2.Zero)
            {
                direction.Normalize();
                velocity = direction * speed;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && enemyBulletSprite != null)
            {
                spriteBatch.Draw(enemyBulletSprite, position, Color.White);
            }
        }

        public bool CheckCollision(Rectangle other)
        {
            return BoundingBox.Intersects(other);
        }

        public override Rectangle BoundingBox 
        {
            get
            {
                if (enemyBulletSprite != null)
                {
                    return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 
                                          enemyBulletSprite.Width, enemyBulletSprite.Height);                }
                return base.BoundingBox;
            }
        }

        public int Damage { get => damage; set => damage = value; }
    }
}