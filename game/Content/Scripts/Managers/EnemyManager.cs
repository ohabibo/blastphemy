using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.Engine.GameObjects;

namespace Blok3Game.content.Scripts.Managers
{
    public class EnemyManager : GameObjectList 
    {
        private Texture2D enemySprite;
        private GameObject player;
        private float spawnTimer;
        private float spawnInterval = 2f;

        public EnemyManager(Texture2D enemyTexture, GameObject playerObject) : base()
        {
            enemySprite = enemyTexture;
            player = playerObject;
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0;
            }

            foreach (GameObject obj in children) 
            {
                if (obj is Enemy enemy) {
                    enemy.SetTargetPosition(player.Position);
                }
            }
            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            Enemy enemy = new Enemy(enemySprite);
            Add(enemy);
        }
        public List<GameObject> GetChildren(){
            return children;
        }
    }
}