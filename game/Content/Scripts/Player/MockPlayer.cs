using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.content.Scripts
{
    // A temporary placeholder for the player
    public class MockPlayer : GameObject
    {
        private Texture2D sprite;
        
        public MockPlayer(Texture2D playerSprite) : base()
        {
            sprite = playerSprite;
            // Start at bottom center
            position = new Vector2(400, 500); // Adjust for your screen dimensions
        }
        
        public override void HandleInput(InputHelper inputHelper)
        {
            // Simple WASD movement for testing
            velocity = Vector2.Zero;
            
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                velocity.X = -200;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                velocity.X = 200;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                velocity.Y = -200;
            if (inputHelper.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                velocity.Y = 200;
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