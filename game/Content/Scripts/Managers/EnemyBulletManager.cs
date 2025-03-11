using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.Engine.GameObjects;

namespace Blok3Game.content.Scripts.Managers
{
    public class EnemyBulletManager : GameObjectList 
    {
        private Texture2D enemyBulletSprite;
        private GameObject player;
        private float spawnTimer;
        private float spawnInterval = 999f;

        private Vector2 numb1;

        public EnemyBulletManager(Texture2D enemyBulletTexture, GameObject playerObject) : base()
        {
            enemyBulletSprite = enemyBulletTexture;
            player = playerObject;
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTimer >= spawnInterval)
            {
                numb1 = new Vector2(0, 0);
                SpawnAimedEnemyBullet1(numb1);
                spawnTimer = 0;
            }

            /*
            foreach (GameObject obj in children) 
            {
                if (obj is EnemyBullet enemyBullet) {
                    enemyBullet.SetTargetPosition(player.Position);
                }
            }
            */
            base.Update(gameTime);
        }

        public void SpawnAimedEnemyBullet1(Vector2 enemyPos)
        {
            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, player.Position);
            Add(enemyBullet);
        }
    }
}