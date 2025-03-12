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

            introScreen = new IntroScreen(logoTexture, font);
            shieldProgressBar = new ShieldProgressBar(shieldTexture, 100, shieldFont);

            GameState gameState = new GameState(GraphicsDevice);
            gameState.Initialize(Content); 
            
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
                shieldProgressBar.UpdateShield(80); // Example value
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
                shieldProgressBar.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        private void Game_Exiting(object sender, ExitingEventArgs e)
        {
            SocketClient.Instance.Disconnect();
        }
    }
}
