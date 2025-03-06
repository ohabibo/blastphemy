using Blok3Game.content.Scripts;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private EnemyManager enemyManager;
        private Texture2D enemyTexture;
        private EnemyBulletManager enemyBulletManager;
        private Texture2D enemyBulletTexture;
        private Texture2D playerTexture; // Added missing field declaration
        private MockPlayer tempPlayer;
        private GraphicsDevice graphicsDevice;
        
        public GameState(GraphicsDevice graphicsDevice) : base()
        {           
            this.graphicsDevice = graphicsDevice;     
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Add this line to see if anything is drawing
            // Set a background color other than black to verify the Draw method is being called
            spriteBatch.GraphicsDevice.Clear(Color.DarkBlue);
            
            base.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            // enemyTexture = content.Load<Texture2D>("Sprites/Enemy");
            // playerTexture = content.Load<Texture2D>("Sprites/TempPlayer");
            playerTexture = CreateCircleTexture(32, Color.White);
            
            // Create a red circle texture for the enemies
            enemyTexture = CreateCircleTexture(32, Color.Red);

            // Create a orange circle texture for the enemy bullets
            enemyBulletTexture = CreateCircleTexture(16, Color.Orange);
            
            
            tempPlayer = new MockPlayer(playerTexture);
            Add(tempPlayer);
            
            enemyManager = new EnemyManager(enemyTexture, tempPlayer); // Pass the mock player
            Add(enemyManager);

            enemyBulletManager = new EnemyBulletManager(enemyBulletTexture, tempPlayer); // Pass the mock player
            Add(enemyBulletManager);
        }

        public void Initialize(ContentManager content)
        {
            LoadContent(content);
        }

        private Texture2D CreateCircleTexture(int radius, Color color)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];
            
            float radiusSquared = radius * radius;
            
            // Fill the texture with the circle
            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    
                    // Calculate distance from center
                    Vector2 position = new Vector2(x - radius, y - radius);
                    if (position.LengthSquared() <= radiusSquared)
                    {
                        colorData[index] = color;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            
            texture.SetData(colorData);
            return texture;
        }
    }
}
