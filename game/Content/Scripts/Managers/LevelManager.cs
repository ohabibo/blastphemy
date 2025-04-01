
using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Blok3Game.content.Scripts.Managers;

namespace Blok3Game.content.Scripts.Managers
{
public class LevelManager : GameObjectList
{
private float Timer = 0;


public LevelManager()
{

}
        public override void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            base.Update(gameTime);
        }

    }
}