using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Data;

namespace Blok3Game.content.Scripts
{
    public class PlayerAttack : GameObject
    {
        private Texture2D sprite;
        private int damage;
        public PlayerAttack(Vector2 _velocity, Texture2D _sprite, Vector2 _position, int damage)
        {
            velocity = _velocity;
            position = _position;
            sprite = _sprite;
            this.damage = damage;
        }

        public bool CheckCollision(Rectangle other)
        {
            return BoundingBox.Intersects(other);
        }
        public override Rectangle BoundingBox 
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    sprite.Width,
                    sprite.Height
                );
            }            
        }
        public int Damage => damage;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (visible && sprite != null)
            {
                spriteBatch.Draw(sprite, position, Color.White);
            }
        }
    }


}