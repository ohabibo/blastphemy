using System;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BaseProject
{
    public class Game : GameEnvironment
    {
        private IntroScreen introScreen;
        private bool isIntroFinished;
        private Texture2D logoTexture;
        private SpriteFont font;
        private SpriteFont shieldFont;
        private ShieldProgressBar shieldProgressBar;
        private Texture2D shieldTexture;
        private Texture2D abilityTexture;

        public Game() : base()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            SocketClient.Instance.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            screen = new Point(1920, 1080);
            ApplyResolutionSettings();

            logoTexture = Content.Load<Texture2D>("Assets/Logo/logo");
            font = Content.Load<SpriteFont>("Assets/Fonts/gamefont");
            shieldFont = Content.Load<SpriteFont>("Assets/Fonts/shieldfont"); // Load the shieldfont for the shield percentage text
            shieldTexture = Content.Load<Texture2D>("Assets/Sprites/Shield/shield");
            abilityTexture = Content.Load<Texture2D>("Assets/Sprites/Ability/ability"); // Load the ability texture

            introScreen = new IntroScreen(logoTexture, font);
            shieldProgressBar = new ShieldProgressBar(shieldTexture, abilityTexture, 100, shieldFont);

            GameState gameState = new GameState(GraphicsDevice);
            gameState.Initialize(Content); 
            
            // Set the shield UI for the game state:
            gameState.shieldProgressBar = shieldProgressBar;

            GameStateManager.AddGameState("GAME_STATE", gameState);
            GameStateManager.SwitchToState("GAME_STATE");

            // Add an event handler for when the game exits.
            Exiting += new EventHandler<ExitingEventArgs>(Game_Exiting);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isIntroFinished)
            {
                introScreen.Update(gameTime);
                if (introScreen.IsFinished)
                    isIntroFinished = true;
            }
            else
            {
                base.Update(gameTime);
                // Update the shield's timers, but don't call TakeDamage() here.
                shieldProgressBar.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (!isIntroFinished)
            {
                introScreen.Draw(spriteBatch);
            }
            else
            {
                base.Draw(gameTime);

                spriteBatch.Begin();

                // Use the shieldProgressBar from the current game state.
                if (GameEnvironment.GameStateManager.CurrentGameState is GameState currentState && currentState.shieldProgressBar != null)
                {
                    currentState.shieldProgressBar.Draw(spriteBatch, GraphicsDevice);
                }

                spriteBatch.End();
            }
        }

        private void Game_Exiting(object sender, ExitingEventArgs e)
        {
            SocketClient.Instance.Disconnect();
        }
    }
}
