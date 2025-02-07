using System;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;

namespace BaseProject
{
    public class Game : GameEnvironment
    {
        public Game() : base()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            SocketClient.Instance.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
			
            screen = new Point(800, 600);
            ApplyResolutionSettings();
			
            GameStateManager.AddGameState("GAME_STATE", new GameState());
            GameStateManager.SwitchToState("GAME_STATE");

            // Add an event handler for when the game exits.
            Exiting += new EventHandler<ExitingEventArgs>(Game_Exiting);
        }

        private void Game_Exiting(object sender, ExitingEventArgs e)
        {
            SocketClient.Instance.Disconnect();
        }
	}
}
