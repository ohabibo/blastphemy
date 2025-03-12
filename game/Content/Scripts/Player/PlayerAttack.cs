using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace Blok3Game.content.Scripts
{
    public class PlayerAttack : GameObject
    {
        private Texture2D sprite;
        public PlayerAttack(Vector2 _velocity, Texture2D _sprite, Vector2 _position)
        {
            velocity = _velocity;
            position = _position;
            sprite = _sprite;
        }
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