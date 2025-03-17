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
using Blok3Game.Engine.UI;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        public EnemyManager enemyManager;
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
        private HitComboCounter hitComboCounter;
        private SpriteFont uiFont;  // assign via LoadContent

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
            
            // Debug: The following block triggers the hit combo UI when H is pressed.
            // It is now commented out.
            /*
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                OnEnemyHit();
            }
            */
            
            if (shieldProgressBar != null)
            {
                shieldProgressBar.Update(gameTime);
                if (shieldProgressBar.IsGameOver)
                {
                    GameEnvironment.GameStateManager.AddGameState("GAME_OVER", 
                        new GameOverScreen(graphicsDevice, content, shieldFont));
                    GameEnvironment.GameStateManager.SwitchToState("GAME_OVER");
                    return;
                }
            }
            
            if (hitComboCounter != null)
                hitComboCounter.Update(gameTime);
            
            FMODAudio.Instance.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkBlue);
            
            base.Draw(gameTime, spriteBatch);

// Inside the Draw method, update the hit combo counter drawing:
            if(hitComboCounter != null)
            {
                // Use the same font used by HitComboCounter (uiFont) for measuring the text.
                Vector2 textSize = uiFont.MeasureString(hitComboCounter.GetDisplayText());
                Vector2 position = new Vector2((graphicsDevice.Viewport.Width - textSize.X) / 2, 20);
                hitComboCounter.Draw(spriteBatch, position, Color.Yellow);
            }
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

            
            // Create a red circle texture for the enemies
            enemyTexture = CreateCircleTexture(32, Color.Red);

            // Create a orange circle texture for the enemy bullets
            enemyBulletTexture = CreateCircleTexture(12, Color.Orange);
            
            
            tempPlayer = new Player(playerTexture, this);
            Add(tempPlayer);

            enemyBulletManager = new EnemyBulletManager(enemyBulletTexture, tempPlayer); // Pass the mock player
            Add(enemyBulletManager);
            
            enemyManager = new EnemyManager(enemyTexture, tempPlayer, enemyBulletManager); // Pass the mock player
            Add(enemyManager);

            // Optionally load the shield font here
            shieldFont = content.Load<SpriteFont>("Assets/Fonts/shieldfont");

            uiFont = content.Load<SpriteFont>("Assets/Fonts/shieldfont");
            hitComboCounter = new HitComboCounter(uiFont);
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

        // Call this method from wherever the enemy hit is confirmed, for example from Player or Enemy collision logic.
        public void OnEnemyHit()
        {
            hitComboCounter?.RegisterHit();
        }
    }
}
