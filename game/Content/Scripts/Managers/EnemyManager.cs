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
        public Player player1;

        private float shootTimer;
        private float shootInterval = 4f;

        private float blastphemyIncrease = 10f;

        public int totalEnemies = 0;

        private Random rand = new Random();

        public EnemyManager(Texture2D enemyTexture, GameObject playerObject, EnemyBulletManager bulletManager, Player player2) : base()
        {
            enemySprite = enemyTexture;
            player = playerObject;
            enemyBulletManager = bulletManager;
            player1 = player2;
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer >= spawnInterval)
            {
                SpawnWave(1, 2, 1, 1);
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
                } else if (obj is TopEnemy topEnemy) {
                    if(shootTimer > shootInterval) {            enemyBulletManager.SpawnEnemyBulletTop1(topEnemy.Position, topEnemy.goingLeft);             }
                }else if (obj is BombEnemy bombEnemy) {
                    bombEnemy.SetTargetPosition(player.Position);
                    if(bombEnemy.GetHP() <= 0) {                enemyBulletManager.SpawnEnemyBulletBomb1(bombEnemy.Position);           } 
                }else if (obj is Enemy enemy) {
                    enemy.SetTargetPosition(player.Position);
                    if(shootTimer > shootInterval) {            enemyBulletManager.SpawnAimedEnemyBullet1(enemy.Position);              }
                }

                if (obj is Enemy enemy1) {
                    if(enemy1.GetHP() <= 0) 
                    {
                        Remove(obj);
                        totalEnemies--;
                        player1.AddToBlasphemy(blastphemyIncrease);
                    }
                }
            }

            if(shootTimer > shootInterval) {        shootTimer = 0;         }

            base.Update(gameTime);
            //Console.WriteLine(totalEnemies);
        }

        private void SpawnEnemy()
        {
            Enemy enemy = new Enemy(enemySprite);
            Add(enemy);
        }
        public List<GameObject> GetChildren() {          return children;        }

        public void DamageAllChildren(int damage)
        {
            foreach (Enemy enemy in children) {         enemy.Damage(damage);       }
        }
        private void SpawnCrossEnemy()
        {
            CrossEnemy crossEnemy = new CrossEnemy(enemySprite);
            Add(crossEnemy);
        }

        private void SpawnTopEnemy()
        {
            TopEnemy topEnemy = new TopEnemy(enemySprite);
            Add(topEnemy);
        }

        private void SpawnBombEnemy()
        {
            BombEnemy bombEnemy = new BombEnemy(enemySprite);
            Add(bombEnemy);
        }

        public void SpawnWave(int enemies, int crossEnemies, int topEnemies, int bombEnemies) {
            for (int i = 0; i < enemies; i++) {             SpawnEnemy();               }
            for (int i = 0; i < crossEnemies; i++) {        SpawnCrossEnemy();          }
            for (int i = 0; i < topEnemies; i++) {          SpawnTopEnemy();            }
            for (int i = 0; i < bombEnemies; i++) {         SpawnBombEnemy();           }
            totalEnemies += enemies + crossEnemies + topEnemies+ bombEnemies;
        }
    }
}