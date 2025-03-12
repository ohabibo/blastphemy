using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.Engine.GameObjects;
using System;

namespace Blok3Game.content.Scripts.Managers
{
    public class EnemyManager : GameObjectList 
    {
        private Texture2D enemySprite;
        private GameObject player;
        private float spawnTimer;
        private float spawnInterval = 2f;
        public EnemyBulletManager enemyBulletManager;

        private float shootTimer;
        private float shootInterval = 2f;

        public EnemyManager(Texture2D enemyTexture, GameObject playerObject, EnemyBulletManager bulletManager) : base()
        {
            enemySprite = enemyTexture;
            player = playerObject;
            enemyBulletManager = bulletManager;
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer >= spawnInterval)
            {
                SpawnCrossEnemy();
                spawnTimer = 0;
            }

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject obj in children) 
            {
                if (obj is CrossEnemy crossEnemy) {
                    crossEnemy.SetTargetPosition(player.Position);

                    if(shootTimer > shootInterval) 
                    {
                        if (crossEnemy.crossShot) {
                            enemyBulletManager.SpawnEnemyBulletCross1(crossEnemy.Position);
                            crossEnemy.crossShot = false;
                        } else {
                            enemyBulletManager.SpawnEnemyBulletPlus1(crossEnemy.Position);
                            crossEnemy.crossShot = true;
                        }
                    }
                } else if (obj is Enemy enemy) {
                    enemy.SetTargetPosition(player.Position);

                    if(shootTimer > shootInterval) 
                    {
                        enemyBulletManager.SpawnAimedEnemyBullet1(enemy.Position);
                    }
                }
            }

            if(shootTimer > shootInterval) 
            {
                shootTimer = 0;
            }
            Console.WriteLine(shootTimer);


            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            Enemy enemy = new Enemy(enemySprite);
            Add(enemy);
        }
        private void SpawnCrossEnemy()
        {
            CrossEnemy crossEnemy = new CrossEnemy(enemySprite);
            Add(crossEnemy);
        }
    }
}