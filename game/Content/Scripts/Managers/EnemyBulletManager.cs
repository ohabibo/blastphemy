using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.content.Scripts.Enemies;
using Blok3Game.Engine.GameObjects;
using System;

namespace Blok3Game.content.Scripts.Managers
{
    public class EnemyBulletManager : GameObjectList 
    {
        private Texture2D enemyBulletSprite;
        private GameObject player;
        private float spawnTimer;
        private float spawnInterval = 999f;

        private Vector2 numb1;

        private Vector2 offset1;

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
            enemyPos.X += 22;
            enemyPos.Y += 22;

            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, player.Position);
            Add(enemyBullet);
        }
        public void SpawnEnemyBulletPlus1(Vector2 enemyPos)
        {
            enemyPos.X += 22;
            enemyPos.Y += 22;

            offset1 = enemyPos;
            offset1.X += 1;
            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X -= 2;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X += 1;
            offset1.Y += 1;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.Y -= 2;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
        }

        public void SpawnEnemyBulletCross1(Vector2 enemyPos)
        {
            enemyPos.X += 22;
            enemyPos.Y += 22;

            offset1 = enemyPos;
            offset1.X += 1;
            offset1.Y += 1;
            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X -= 2;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.Y -= 2;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X += 2;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
        }
        public List<GameObject> GetChildren(){
            return children;
        }

        public void SpawnEnemyBulletTop1(Vector2 enemyPos, bool goingLeft)
        {
            
            if (goingLeft) {
                enemyPos.X += 12;
            } else {
                enemyPos.X += 32;
            }
            enemyPos.Y += 38;
            
            offset1.X = enemyPos.X;
            offset1.Y = 1000;
            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X = enemyPos.X + 50;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            offset1.X = enemyPos.X - 50;
            enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
        }

        public void SpawnEnemyBulletBomb1(Vector2 enemyPos)
        {
            enemyPos.X += 22;
            enemyPos.Y += 22;

            offset1.X = enemyPos.X;
            offset1.Y = enemyPos.Y + 2;
            EnemyBullet enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
            Add(enemyBullet);
            for (int i = 0; i < 2; i++) {
                offset1.X++;
                enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
                Add(enemyBullet);
            }
            for (int i = 0; i < 4; i++) {
                offset1.Y--;
                enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
                Add(enemyBullet);
            }
            for (int i = 0; i < 4; i++) {
                offset1.X--;
                enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
                Add(enemyBullet);
            }
            for (int i = 0; i < 4; i++) {
                offset1.Y++;
                enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
                Add(enemyBullet);
            }
            for (int i = 0; i < 2; i++) {
                offset1.X++;
                enemyBullet = new EnemyBullet(enemyBulletSprite, enemyPos, offset1);
                Add(enemyBullet);
            }
        }
    }
}