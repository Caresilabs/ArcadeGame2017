using CloudColony.Framework;
using CloudColony.Framework.Tools;
using Microsoft.Xna.Framework;
using Pacifier.Entities;
using Pacifier.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacifier.Simulation
{
    public class Spawner
    {
        private const int MAX_ENEMIES = 700;

        public World World { get; private set; }

        private float time = 0;

        private float bellTime;
        private float enemyTime;

        public Spawner(World world)
        {
            this.World = world;
        }

        public void Update(float delta)
        {
            if (World.State != World.WorldState.RUNNING)
                return;

            time += delta;
            bellTime -= delta;
            enemyTime -= delta;

            if (bellTime < 0)
            {
                SpawnDumbbell();

                bellTime = MathHelper.Max(2, MathUtils.Random(3.5f, 5.5f) - (time * 0.03f));
            }

            if (enemyTime < 0)
            {
                if (World.Enemies.Count < MAX_ENEMIES)
                    SpawnEnemies((int)(Math.Max(1, time / 3f)));

                enemyTime = MathUtils.Random(2.5f, 3.5f);
            }
        }

        private void SpawnEnemies(int count)
        {
            Vector2 size;// = new Vector2();
            var pos = RandomPointOnEdge(out size);
            for (int i = 0; i < count; i++)
            {
                var enemyPos = pos;
                enemyPos.X += MathUtils.Random(0, size.X);
                enemyPos.Y += MathUtils.Random(0, size.Y);

                var enemy = new Enemy(World, PR.Enemy, enemyPos.X, enemyPos.Y, .5f, .5f);
                enemy.KeepInside();

                if (IsSafeSpawn(enemy, enemy.Bounds.Radius * 4))
                {
                    World.AddEnemy(enemy);
                }
            }
        }

        private Vector2 RandomPointOnEdge(out Vector2 size)
        {
            var val = MathUtils.Random(0, 100);
            if (val < 25)
            {
                size.X = 4;
                size.Y = 1;
                return new Vector2(MathUtils.Random(0, World.WORLD_HEIGHT), 0);
            }
            else if (val < 50)
            {
                size.X = 4;
                size.Y = -1;
                return new Vector2(MathUtils.Random(0, World.WORLD_WIDTH), World.WORLD_HEIGHT);
            }
            else if (val < 75)
            {
                size.X = 1;
                size.Y = 4;
                return new Vector2(0, MathUtils.Random(0, World.WORLD_HEIGHT));
            }
            else
            {
                size.X = -1;
                size.Y = 4;
                return new Vector2(World.WORLD_WIDTH, MathUtils.Random(0, World.WORLD_WIDTH));
            }
        }

        public void SpawnDumbbell()
        {
            Dumbbell score = new Dumbbell(World, PR.Dumbbell, MathUtils.Random(1, World.WORLD_WIDTH - 1), MathUtils.Random(1, World.WORLD_HEIGHT - 1));
            for (int i = 0; i < 10; ++i)
            {
                if (IsSafeSpawn(score, score.Bounds.Radius * 4f))
                {
                    World.Add(score);
                    break;
                }
                else
                {
                    score.SetPosition(MathUtils.Random(1, World.WORLD_WIDTH - 1), MathUtils.Random(1, World.WORLD_HEIGHT - 1));
                }
            }
        }

        private Circle tmpCircle = new Circle(Vector2.Zero, 0);
        private bool IsSafeSpawn(Entity e, float r)
        {
            tmpCircle.Center = e.Position;
            tmpCircle.Radius = r;
            return !World.PlayerYellow.Bounds.Intersects(tmpCircle) && !World.PlayerGreen.Bounds.Intersects(tmpCircle);
        }
    }
}
