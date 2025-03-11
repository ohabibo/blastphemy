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
        public PlayerAttack(Vector2 _velocity)
        {
            velocity = _velocity;
        }
        public override void Update(GameTime gameTime)
        { }
    }
}