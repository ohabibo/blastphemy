using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Blok3Game.Engine.GameObjects;
using Blok3Game.content.Scripts.Audio; // Add this using directive

namespace Blok3Game.GameStates
{
    // This screen replaces the active game state when a game over occurs.
    // It stops updating game objects and only shows the game-over message.
    public class GameOverScreen : GameObjectList
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private SpriteFont font;
        private string message = "YOU COMMITTED A HERESY !\nPress R to Restart";

        public GameOverScreen(GraphicsDevice graphicsDevice, ContentManager content, SpriteFont font)
            : base()
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            // Load the new font asset for the game over message.
            this.font = content.Load<SpriteFont>("Assets/Fonts/gameFont");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.R))
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            // Stop and reset audio first
            FMODAudio.Instance.StopAndReset();

            // Create a fresh game state instance and initialize its content.
            GameState newState = new GameState(graphicsDevice);
            newState.Initialize(content);
            
            // Reload shield assets.
            Texture2D shieldTexture = content.Load<Texture2D>("Assets/Sprites/Shield/shield");
            Texture2D abilityTexture = content.Load<Texture2D>("Assets/Sprites/Ability/ability");
            // Load the gameplay font (using "gamefont")
            SpriteFont gameFont = content.Load<SpriteFont>("Assets/Fonts/shieldfont");
            
            // Create a new ShieldProgressBar.
            ShieldProgressBar newShieldBar = new ShieldProgressBar(shieldTexture, abilityTexture, 100, gameFont);
            
            // Inline reset logic:
            // Set the shield to full (100) and clear any hit/timer flags.
            newShieldBar.Reset();
            // (Optional debug output)
            //Console.WriteLine("RestartGame: Shield reset invoked. Expected currentShield = 100.");

            // Assign this new, reset ShieldProgressBar to the new game state.
            newState.shieldProgressBar = newShieldBar;
            
            // newState.Initialize should also re-create the Player (reset position & full lives)
            // as well as enemy manager and enemy bullet manager with fresh instances.
            GameEnvironment.GameStateManager.AddGameState("GAME_STATE", newState);
            GameEnvironment.GameStateManager.SwitchToState("GAME_STATE");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Assume spriteBatch.Begin() has already been called.
            Vector2 size = font.MeasureString(message);
            Vector2 position = new Vector2(graphicsDevice.Viewport.Width / 2 - size.X / 2,
                                           graphicsDevice.Viewport.Height / 2 - size.Y / 2);
            spriteBatch.DrawString(font, message, position, Color.White);
        }
    }
}