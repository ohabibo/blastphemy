using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.content.Scripts.Enemies.boss;
using Blok3Game.Engine.GameObjects;
using System;

namespace Blok3Game.content.Scripts.Managers
{
     public class BossManager : GameObjectList
     {
            private List<BossEnemy> bossEnemies;
            private Texture2D bossSprite;
            private int maxBosses = 1; // Adjusted for boss
            private Random rand;

            private GameObject player;

            private Texture2D bulletTexture;
            private EnemyBulletManager enemyBulletManager;
    
            public BossManager(Texture2D sprite, Texture2D bullet, GameObject Player, EnemyBulletManager EnemyBulletManager) : base()
            {
                bossSprite = sprite;
                enemyBulletManager = EnemyBulletManager;
                player = Player;
                bulletTexture = bullet;
                bossEnemies = new List<BossEnemy>();
                rand = new Random();
            }
    
            public void SpawnBoss()
            {
                if (bossEnemies.Count < maxBosses)
                {
                    BossEnemy newBoss = new BossEnemy(bossSprite, bulletTexture, enemyBulletManager);
                    bossEnemies.Add(newBoss);
                    Add(newBoss);
                }
            }
    
            public override void Update(GameTime gameTime)
            {
                foreach (var boss in bossEnemies)
                {
                    boss.SetTargetPosition(player.Position);
                    boss.Update(gameTime);
                    if (!boss.IsAlive)
                    {
                        bossEnemies.Remove(boss);
                        Remove(boss);
                        break; // Exit the loop to avoid modifying the collection while iterating
                    }
                }
            }

            public List<GameObject> GetChildren(){
            return children;
        }
     }
}