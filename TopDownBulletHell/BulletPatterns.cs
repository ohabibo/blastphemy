using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public static class BulletPatterns
{
    public static void ShootSpiralPattern(List<Bullet> bullets, Texture2D bulletTexture, Vector2 position, float angleOffset)
    {
        int bulletCount = 10;
        float angleStep = MathHelper.TwoPi / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep + angleOffset;
            Vector2 velocity = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 5f;
            bullets.Add(new Bullet(bulletTexture, position, velocity));
        }
    }

    public static void ShootCirclePattern(List<Bullet> bullets, Texture2D bulletTexture, Vector2 position)
    {
        int bulletCount = 12;
        float angleStep = MathHelper.TwoPi / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector2 velocity = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 5f;
            bullets.Add(new Bullet(bulletTexture, position, velocity));
        }
    }

    public static void ShootLinePattern(List<Bullet> bullets, Texture2D bulletTexture, Vector2 position, Vector2 direction, int bulletCount, float spacing)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 bulletPosition = position + direction * spacing * i;
            bullets.Add(new Bullet(bulletTexture, bulletPosition, direction * 5f));
        }
    }
}