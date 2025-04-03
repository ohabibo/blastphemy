using Blok3Game.content.Scripts;
using Blok3Game.content.Scripts.Managers;
using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine;
using Blok3Game.Engine.Helpers;
using Blok3Game.content.Scripts.Audio;
using System.Net.Sockets;
using Microsoft.Xna.Framework.Input;
using System.Security.Principal;
using System.Windows;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        public EnemyManager enemyManager;
        private Texture2D backgroundTexture;
        private Texture2D enemyTexture;
        public EnemyBulletManager enemyBulletManager;
        private Texture2D enemyBulletTexture;
        private Texture2D playerTexture; // Added missing field declaration
        public Texture2D playerBulletTexture;
        private Player tempPlayer;
        private GraphicsDevice graphicsDevice;
        public ShieldProgressBar shieldProgressBar { get; set; }
        
        // Add these fields:
        private ContentManager content;
        private SpriteFont shieldFont; // assign this to the font you need for the shield UI

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
            // Update game objects
            base.Update(gameTime);

            // Update the shield (timers, etc.)
            if (shieldProgressBar != null)
            {
                shieldProgressBar.Update(gameTime);
                
                // Check if shield has reached 0%
                if (shieldProgressBar.IsGameOver)
                {
                    // Transition to the Game Over screen
                    GameEnvironment.GameStateManager.AddGameState("GAME_OVER", 
                        new GameOverScreen(graphicsDevice, content, shieldFont));
                    GameEnvironment.GameStateManager.SwitchToState("GAME_OVER");
                    return; // Prevent further update in the current state.
                }
            }

            FMODAudio.Instance.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Add this line to see if anything is drawing
            // Set a background color other than black to verify the Draw method is being called
            spriteBatch.GraphicsDevice.Clear(Color.DarkBlue);
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.SlateBlue);
            
            base.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            // Store the ContentManager for later use.
            this.content = content;
            
            FMODAudio.Instance.PlayMusic("event:/TestMusic");
            // enemyTexture = content.Load<Texture2D>("Sprites/Enemy");
            // playerTexture = content.Load<Texture2D>("Sprites/TempPlayer");
            playerTexture = CreateCircleTexture(32, Color.White);
            playerBulletTexture = CreateCircleTexture(5, Color.Yellow);

            // sets background texture
            backgroundTexture = content.Load<Texture2D>("Assets/Sprites/Background/Background");
            
            // Create a red circle texture for the enemies
            enemyTexture = CreateCircleTexture(32, Color.Red);

            // Create a orange circle texture for the enemy bullets
            enemyBulletTexture = CreateCircleTexture(12, Color.Orange);
            
            
            tempPlayer = new Player(playerTexture, this);
            Add(tempPlayer);

            enemyBulletManager = new EnemyBulletManager(enemyBulletTexture, tempPlayer); // Pass the mock player
            Add(enemyBulletManager);
            
            enemyManager = new EnemyManager(enemyTexture, tempPlayer, enemyBulletManager, tempPlayer); // Pass the mock player
            Add(enemyManager);

            // Optionally load the shield font here
            shieldFont = content.Load<SpriteFont>("Assets/Fonts/shieldfont");
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
                    Vector2 position = new Vector2(x- radius, y - radius);
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
