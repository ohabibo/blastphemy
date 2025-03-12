using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using System;
using System.Runtime.CompilerServices;
using Blok3Game.GameStates;

namespace Blok3Game.content.Scripts
{
    public class Blasphemy : GameObject
    {
        private GameState gameState;

        public Blasphemy(GameState _gameState) { 
            gameState = _gameState;
        }
        public void trigger(int damage){
            gameState.enemyManager.DamageAllChildren(damage);
        }
    }
}