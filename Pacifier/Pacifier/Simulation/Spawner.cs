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
        public World World { get; private set; }

        private float time;

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

                bellTime = MathHelper.Max(2, MathUtils.Random(4, 7f) - (time * 0.03f));
            }

            if (enemyTime < 0)
            {
                SpawnEnemies((int)(Math.Max(1, time / 3f)));

                enemyTime = MathUtils.Random(1.5f, 3.5f);
            }

            //if (MathUtils.Random(0, 1000) < 10)
            //    SpawnEnemies((int)(Math.Max(1, time / 9f)));

            //if (MathUtils.Random(0, 1000) < 5)
            //    SpawnDumbbell();
        }

        private void SpawnEnemies(int count)
        {
            var pos = RandomPointOnEdge();
            for (int i = 0; i < count; i++)
            {
                var enemyPos = pos;
                enemyPos.X += MathUtils.Random(-1f, 1f);
                enemyPos.Y += MathUtils.Random(-1f, 1f);

                var enemy = new Enemy(World, PR.Enemy, enemyPos.X, enemyPos.Y, .45f, .45f);

                if (IsSafeSpawn(enemy, enemy.Bounds.Radius * 2))
                {
                    World.AddEnemy(enemy);
                }
            }
        }

        private Vector2 RandomPointOnEdge()
        {
            var val = MathUtils.Random(0, 100);
            if (val < 25)
            {
                return new Vector2(MathUtils.Random(0, World.WORLD_HEIGHT), 0);
            }
            else if (val < 50)
            {
                return new Vector2(MathUtils.Random(0, World.WORLD_WIDTH), World.WORLD_HEIGHT);
            }
            else if (val < 75)
            {
                return new Vector2(0, MathUtils.Random(0, World.WORLD_HEIGHT));
            }
            else
            {
                return new Vector2(World.WORLD_WIDTH, MathUtils.Random(0, World.WORLD_WIDTH));
            }
        }

        public void SpawnDumbbell()
        {
            Dumbbell score = new Dumbbell(World, PR.Dumbbell, MathUtils.Random(1, World.WORLD_WIDTH - 1), MathUtils.Random(1, World.WORLD_HEIGHT - 1));
            for (int i = 0; i < 10; ++i)
            {
                if (IsSafeSpawn(score, score.Bounds.Radius * 2f))
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
