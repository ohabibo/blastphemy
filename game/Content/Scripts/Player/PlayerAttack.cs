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
        private Vector2 direction;
        private PlayerAttack(Vector2 _direction, Vector2 _velocity)
        {
            direction = _direction;
            velocity = _velocity;
        }
        public override void Update(GameTime gameTime)
        { }
    }
}