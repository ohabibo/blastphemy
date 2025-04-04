
using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Blok3Game.content.Scripts.Managers
{
 public class LevelManager : GameObjectList
{
    private float timer = 0f;
    private int waveIndex = 0;
    private float waveInterval = 15f; 
    private float totalGameTime = 180f;
    private bool levelComplete = false;

    public EnemyManager enemyManager;
    public BossManager bossManager;
    private Random rand = new Random();

    public LevelManager(EnemyManager manager, BossManager boss)
    {
        enemyManager = manager;
        bossManager = boss;
    }

    public override void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (waveIndex < 3 && timer >= waveIndex * waveInterval)
        {
            int enemies = rand.Next(1, 4);
            int crossEnemies = rand.Next(1, 4);
            int topEnemies = rand.Next(1, 4);
            int bombEnemies = rand.Next(1, 4);

            enemyManager.SpawnWave(enemies, crossEnemies, topEnemies, bombEnemies);

            waveIndex++;

            if (waveIndex >= 3)
            {
                levelComplete = true;
            }

            
        }

        if (timer >= totalGameTime && levelComplete || levelComplete)
        {
            bossManager.SpawnBoss();
        }

        base.Update(gameTime);
    }
}
}