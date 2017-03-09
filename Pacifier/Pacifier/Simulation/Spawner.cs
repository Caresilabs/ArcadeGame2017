using CloudColony.Framework.Tools;
using Pacifier.Entities;
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

        private Random rnd;

        public Spawner(World world)
        {
            this.World = world;
            this.rnd = new Random();
            SpawnDumbbell();
        }

        public void Update(float delta)
        {
            time += delta;
            if (MathUtils.Random(0, 100) < 4)
                SpawnEnemies(1);
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                World.AddEnemy(new Enemy(World, PR.Pixel, MathUtils.Random(1, World.WORLD_WIDTH - 1), MathUtils.Random(1, World.WORLD_HEIGHT - 1), .5f, .5f));
            }
        }

        public void SpawnDumbbell()
        {
            Dumbbell score = new Dumbbell(World, PR.Dumbbell, MathUtils.Random(1, World.WORLD_WIDTH - 1), MathUtils.Random(1, World.WORLD_HEIGHT - 1)) ;
            World.Add(score);
        }
    }
}
